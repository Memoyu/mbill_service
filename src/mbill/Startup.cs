namespace Mbill;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddController();//配置注册Controller
        services.AddJwtBearer();//配置Jwt
        services.AddSwagger();//配置注册Swagger
        services.AddMapper(Assembly.Load("Mbill.Service"));//配置实体映射
        services.AddEasyCaching();//配置注册EasyCaching缓存
        services.AddMongoClient();//配置注册MongoDB Client
        services.AddMiniProfilerSetup();//配置注册监控
        services.AddIpRateLimiting();//配置注册限流
        services.AddHealthChecks();//配置注册健康检查
        services.AddCorsConfig();//配置跨域
        services.AddQiniuClient(Configuration); // 配置七牛云服务
        services.AddHttpClients();//配置HttpClient
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule(new AutofacModule());//注册一些杂项
        builder.RegisterModule(new RepositoryModule());//注册仓储
        builder.RegisterModule(new ServiceModule());//注册服务
        builder.RegisterModule(new DependencyModule());//自动注册，类似Abp中的继承对应的接口就会注册对应接口的生命周期
        builder.RegisterModule(new FreeSqlModule());//注册FreeSql
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger().UseSwaggerUI(() => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Mbill.index.html"));
        }
        //跨域
        app.UseCors(Appsettings.Cors.CorsName);

        //静态文件
        app.UseStaticFiles();

        // Ip限流
        app.UseMiddleware<IpLimitMiddleware>();

        // 记录ip请求
        app.UseMiddleware<IPLogMilddleware>();

        ////异常处理中间件
        //app.UseMiddleware<ExceptionHandlerMiddleware>();

        //认证中间件
        app.UseAuthentication();

        // 性能分析
        app.UseMiniProfiler();

        app.UseRouting()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
    }
}
