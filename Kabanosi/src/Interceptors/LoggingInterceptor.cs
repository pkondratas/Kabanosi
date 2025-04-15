using System.Diagnostics;
using System.Text.Json;
using Castle.DynamicProxy;
using Kabanosi.Extensions;

namespace Kabanosi.Interceptors;

public class LoggingInterceptor : IAsyncInterceptor
{   
    private readonly ILogger<LoggingInterceptor> _logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public void InterceptSynchronous(IInvocation invocation)
    {
        var stopwatch = Invoke(invocation);
        
        InvocationCompleted(invocation, stopwatch, invocation.ReturnValue);
    }

    public void InterceptAsynchronous(IInvocation invocation)
    {
        var stopwatch = Invoke(invocation);

        invocation.ReturnValue = StartTask(invocation, stopwatch);
    }

    public void InterceptAsynchronous<TResult>(IInvocation invocation)
    {
        var stopwatch = Invoke(invocation);
        
        invocation.ReturnValue = StartTask<TResult>(invocation, stopwatch);
    }

    private async Task StartTask(IInvocation invocation, Stopwatch stopwatch)
    {
        var returnValue = (Task?)invocation.ReturnValue;

        try
        {
            await returnValue;
            InvocationCompleted(invocation, stopwatch);
        }
        catch (Exception e)
        {
            InvocationFailed(invocation, e, stopwatch);
            throw;
        }
    }

    private async Task<TResult> StartTask<TResult>(IInvocation invocation, Stopwatch stopwatch)
    {
        var returnValue = (Task<TResult?>)invocation.ReturnValue;

        try
        {
            var result = await returnValue;
            InvocationCompleted(invocation, stopwatch, result);
            
            return result;
        }
        catch (Exception e)
        {
            InvocationFailed(invocation, e, stopwatch);
            throw;
        }
    }
    
    private Stopwatch Invoke(IInvocation invocation)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            invocation.Proceed();
        }
        catch (Exception e)
        {
            InvocationFailed(invocation, e, stopwatch);
            throw;
        }
        
        return stopwatch;
    }
    
    private void InvocationCompleted(IInvocation invocation, Stopwatch stopwatch, object? result = null)
    {
        stopwatch.Stop();
        
        _logger.LogInformation(
            "Method {method} called. Arguments: {arguments}. " +
            "Result: {result}. ElapsedMilliseconds: {elapsedMilliseconds}.",
            invocation.GetMethodSignature(),
            invocation.GetArguments(),
            JsonSerializer.Serialize(result),
            stopwatch.ElapsedMilliseconds);
    }

    private void InvocationFailed(IInvocation invocation, Exception exception, Stopwatch stopwatch)
    {
        stopwatch.Stop();
        
        _logger.LogError(
            "Method {method} threw an exception. Arguments: {arguments}. " +
            "Exception: {exception}. ElapsedMilliseconds: {elapsedMilliseconds}.",
            invocation.GetMethodSignature(),
            invocation.GetArguments(),
            exception.Message,
            stopwatch.ElapsedMilliseconds);
    }
}