using Autofac;
using Autofac.Extras.DynamicProxy;
using mbill_service.Core.AOP.Intercepts;
using mbill_service.Service.Core.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace mbill_service.Modules
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWorkAsyncInterceptor>();
            builder.RegisterType<UnitOfWorkInterceptor>();

            builder.RegisterType<CacheIntercept>();

            List<Type> interceptorServiceTypes = new List<Type>()
            {
                typeof(UnitOfWorkInterceptor),
                typeof(CacheIntercept),
            };

            string[] notIncludes = new string[]
            {
                typeof(IdentityServer4Service).Name,
                typeof(JwtTokenService).Name,
            };

            Assembly servicesDllFile = Assembly.Load("mbill_service.Service");
            builder.RegisterAssemblyTypes(servicesDllFile)
                .Where(a => a.Name.EndsWith("Service") && !notIncludes.Where(r => r == a.Name).Any() && !a.IsAbstract && !a.IsInterface && a.IsPublic)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired()// 属性注入
                .InterceptedBy(interceptorServiceTypes.ToArray())
                .EnableInterfaceInterceptors();

            //使用名称进行实现注册
            builder.RegisterType<IdentityServer4Service>().Named<ITokenService>(typeof(IdentityServer4Service).Name).InstancePerLifetimeScope();
            builder.RegisterType<JwtTokenService>().Named<ITokenService>(typeof(JwtTokenService).Name).InstancePerLifetimeScope();
        }
    }
}
