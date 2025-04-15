using Castle.DynamicProxy;

namespace Kabanosi.Extensions;

public static class InvocationExtensions
{
    public static string GetMethodSignature(this IInvocation invocation)
    {
        return $"{invocation.TargetType.FullName}.{invocation.Method.Name}";
    }

    public static string GetArguments(this IInvocation invocation)
    {
        return string.Join(", ", invocation.Arguments.Select(a => a?.ToString() ?? "null"));
    }
}