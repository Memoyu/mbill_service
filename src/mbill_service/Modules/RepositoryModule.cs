using Autofac;
using System.Reflection;

namespace mbill_service.Modules
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Assembly assemblysRepository = Assembly.Load("mbill_service.Infrastructure");
            builder.RegisterAssemblyTypes(assemblysRepository)
                    .Where(a => a.Name.EndsWith("Repo"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
        }
    }
}
