namespace mbill_service.Core.AOP.Intercepts;

public class UnitOfWorkInterceptor : IInterceptor
{
    private readonly UnitOfWorkAsyncInterceptor asyncInterceptor;

    public UnitOfWorkInterceptor(UnitOfWorkAsyncInterceptor interceptor)
    {
        asyncInterceptor = interceptor;
    }

    public void Intercept(IInvocation invocation)
    {
        asyncInterceptor.ToInterceptor().Intercept(invocation);
    }
}
