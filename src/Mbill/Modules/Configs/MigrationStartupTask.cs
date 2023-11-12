namespace Mbill.Modules.Configs;

public class MigrationStartupTask
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MigrationStartupTask> _logger;
    public MigrationStartupTask(IServiceProvider serviceProvider, ILogger<MigrationStartupTask> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            IDataSeedSvc dataSeedSvc = scope.ServiceProvider.GetRequiredService<IDataSeedSvc>();

            await dataSeedSvc.InitDataSeedAsync();
            var defPermissions = DomainReflexUtil.GetAssemblyPermissionAttributes();
            await dataSeedSvc.InitPermissionAsync(defPermissions);
            await dataSeedSvc.InitAdministratorPermissionAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"初始化数据失败！！！{ex.Message}{ex.StackTrace}{ex.InnerException}");
        };
    }
}
