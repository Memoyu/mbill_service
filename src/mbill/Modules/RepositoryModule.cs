namespace mbill.Modules;

public class RepositoryModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        Assembly assemblysRepository = Assembly.Load("mbill.Infrastructure");
        builder.RegisterAssemblyTypes(assemblysRepository)
                .Where(a => a.Name.EndsWith("Repo"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
    }
}
