/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Data
*   文件名称 ：MigrationStartupTask.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-09 14:55:23
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Application.Contracts.Base;
using Memoyu.Mbill.Domain.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Memoyu.Mbill.WebApi.Data
{
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
                IDataSeedContributor dataSeedContributor = scope.ServiceProvider.GetRequiredService<IDataSeedContributor>();

                var permissions = DomainReflexUtil.GetAssemblyPermissionAttributes();
                await dataSeedContributor.InitPermissionAsync(permissions);
                await dataSeedContributor.InitAdministratorPermissionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"初始化数据失败！！！{ex.Message}{ex.StackTrace}{ex.InnerException}");
            };
        }
    }
}
