/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Extensions
*   文件名称 ：AutoMapperSetup.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 13:23:32
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Application.Contracts.Mapper;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;


namespace Memoyu.Mbill.WebApi.Extensions
{
    /// <summary>
    /// 注册Automapper 服务
    /// </summary>
    public static class AutoMapperSetup
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperConfig));
            AutoMapperConfig.RegisterMappings();
        }
    }
}
