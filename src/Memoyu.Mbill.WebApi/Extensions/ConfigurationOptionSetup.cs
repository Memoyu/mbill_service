/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Extensions
*   文件名称 ：ConfigurationOptionSetup.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-23 18:57:02
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Domain.Shared.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyu.Mbill.WebApi.Extensions
{
    public static class ConfigurationOptionSetup
    {
        public static void AddConfigurationOption(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FileStorageOption>(configuration.GetSection("FileStorage"));
        }
    }
}
