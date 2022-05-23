namespace mbill.Modules;

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
        };

        Assembly servicesDllFile = Assembly.Load("mbill.Service");
        builder.RegisterAssemblyTypes(servicesDllFile)
            .Where(a => a.Name.EndsWith("Svc") && !notIncludes.Where(r => r == a.Name).Any() && !a.IsAbstract && !a.IsInterface && a.IsPublic)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .PropertiesAutowired()// 属性注入
            .InterceptedBy(interceptorServiceTypes.ToArray())
            .EnableInterfaceInterceptors();
    }
}