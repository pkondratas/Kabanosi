using Castle.DynamicProxy;

namespace Kabanosi.Interceptors;

public class InterceptorAdapter<TAsyncInterceptor> : AsyncDeterminationInterceptor
    where TAsyncInterceptor : IAsyncInterceptor
{
    public InterceptorAdapter(TAsyncInterceptor interceptor)
        : base(interceptor)
    {
    }
}