/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Modules
*   文件名称 ：ServiceModule.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 14:44:52
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Autofac;
using Autofac.Extras.DynamicProxy;
using Memoyu.Mbill.Application.Core;
using Memoyu.Mbill.Application.Core.Impl;
using Memoyu.Mbill.Domain.Entities.Core;
using Memoyu.Mbill.WebApi.Aop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Memoyu.Mbill.WebApi.Modules
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

            Assembly servicesDllFile = Assembly.Load("Memoyu.Mbill.Application");
            builder.RegisterAssemblyTypes(servicesDllFile)
                .Where(a => a.Name.EndsWith("Service") && !notIncludes.Where(r => r == a.Name).Any() && !a.IsAbstract && !a.IsInterface && a.IsPublic)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired()// 属性注入
                .InterceptedBy(interceptorServiceTypes.ToArray())
                .EnableInterfaceInterceptors();

            //使用名称进行实现注册
            //存储文件
            builder.RegisterType<LocalFileService>().Named<IFileService>(FileEntity.LocalFileService).InstancePerLifetimeScope();

            //授权
            builder.RegisterType<IdentityServer4Service>().Named<ITokenService>(typeof(IdentityServer4Service).Name).InstancePerLifetimeScope();
            builder.RegisterType<JwtTokenService>().Named<ITokenService>(typeof(JwtTokenService).Name).InstancePerLifetimeScope();

        }
    }
}
