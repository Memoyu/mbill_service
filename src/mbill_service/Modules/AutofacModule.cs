﻿using Autofac;
using mbill_service.Core.Security;
using mbill_service.Modules.Configs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace mbill_service.Modules
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.RegisterType<PermissionAuthorizationHandler>().As<IAuthorizationHandler>().InstancePerLifetimeScope();
            builder.RegisterType<CurrentUser>().As<ICurrentUser>().InstancePerDependency();

            builder.RegisterType<MigrationStartupTask>().SingleInstance();
            builder.RegisterBuildCallback(async (c) => await c.Resolve<MigrationStartupTask>().StartAsync());
        }
    }
}
