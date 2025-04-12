using System.Net;
using Kabanosi.Middleware;

namespace Kabanosi.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(
        this IApplicationBuilder builder,
        IDictionary<Type, HttpStatusCode> exceptionMappings)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>(exceptionMappings);
    }
}