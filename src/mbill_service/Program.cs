using AspNetCoreRateLimit;
using Autofac.Extensions.DependencyInjection;
using mbill_service.Core.Common.Configs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mbill_service
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Appsettings.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                Log.Information("init main");
                IHost webHost = CreateHostBuilder(args).Build();
                try
                {
                    using var scope = webHost.Services.CreateScope();
                    // get the IpPolicyStore instance
                    var ipPolicyStore = scope.ServiceProvider.GetRequiredService<IIpPolicyStore>();
                    // seed IP data from appsettings
                    await ipPolicyStore.SeedAsync();
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "IIpPolicyStore RUN Error");
                }
                await webHost.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())//添加Autofac服务工厂
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
#if DEBUG
                .UseUrls("http://*:10085");
#endif
                    ;
                })
                .UseSerilog();//构建Serilog;
    }
}
