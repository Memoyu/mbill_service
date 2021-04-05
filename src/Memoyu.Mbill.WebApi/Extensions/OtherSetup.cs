using Memoyu.Mbill.Domain.Shared.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Memoyu.Mbill.WebApi.Extensions
{
    public static class OtherSetup
    {
        public static void AddCorsConfig(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(AppSettings.Cors.CorsName, builder =>
                {
                    builder
                        .WithOrigins(
                            AppSettings.Cors
                                      .CorsOrigins
                                      .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                      .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }
    }
}
