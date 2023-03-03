using mbill.Core.Common.Configs;
using Serilog.Events;

namespace mbill;

public class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(LogEventLevel.Verbose, "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}")
            .WriteTo.MongoDBBson(Appsettings.MongoDBCon, "logs", LogEventLevel.Warning, 50, null, 1024, 50000)
            .Enrich.FromLogContext()
            .CreateLogger();
        try
        {
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
            .UseUrls("http://*:9000");
#endif
                ;
            })
            .UseSerilog((context, logger) =>
            {
                logger.Enrich.FromLogContext();
                logger.WriteTo.Console(LogEventLevel.Verbose, "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}");
                logger.WriteTo.MongoDBBson(Appsettings.MongoDBCon, "logs", LogEventLevel.Warning, 50, null, 1024, 50000);
            });//构建Serilog;
}
