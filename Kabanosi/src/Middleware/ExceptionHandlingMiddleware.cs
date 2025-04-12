using System.Net;

namespace Kabanosi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDictionary<Type, HttpStatusCode> _exceptionMappings;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        IDictionary<Type, HttpStatusCode> exceptionMappings)
    {
        _next = next;
        _exceptionMappings = exceptionMappings;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception e)
        {
            await HandleException(httpContext, e);
        }
    }

    private async Task HandleException(HttpContext httpContext, Exception exception)
    {
        var type = exception.GetType();
        var baseType = exception.GetBaseException().GetType();
        
        if (_exceptionMappings?.TryGetValue(type, out var statusCode) == true)
        {
            await SetResponse(httpContext, statusCode, exception.Message);
        }
        else if (_exceptionMappings?.TryGetValue(baseType, out statusCode) == true)
        {
            await SetResponse(httpContext, statusCode, exception.Message);
        }
        else
        {
            statusCode = HttpStatusCode.InternalServerError;
            await SetResponse(httpContext, statusCode, exception.Message, exception.StackTrace);
        }
    }

    private static Task SetResponse(
        HttpContext httpContext,
        HttpStatusCode statusCode,
        string message,
        string? stackTrace = null)
    {
        httpContext.Response.ContentType = "application/text";
        httpContext.Response.StatusCode = (int)statusCode;

        if (stackTrace == null)
        {
            return httpContext.Response.WriteAsync($"Error: {message}");
        }

        return httpContext.Response.WriteAsync(
            $"Error: {message}\n" +
            $"StackTrace: {stackTrace}");
    }
}