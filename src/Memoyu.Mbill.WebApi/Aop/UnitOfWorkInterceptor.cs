/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Aop
*   文件名称 ：UnitOfWorkInterceptor.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 11:33:53
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Castle.DynamicProxy;

namespace Memoyu.Mbill.WebApi.Aop
{
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
}
