namespace mbill_service.Service.Base;

public class ApplicationSvc : IApplicationSvc
{
    //provider Lock
    protected readonly object ServiceProviderLock = new object();
    public IServiceProvider ServiceProvider { get; set; }


    //懒加载
    protected TService LazyGetRequiredService<TService>(ref TService reference)
        => LazyGetRequiredService(typeof(TService), ref reference);
    protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
    {
        if (reference == null)
        {
            lock (ServiceProviderLock)
            {
                if (reference == null)
                {
                    reference = (TRef)ServiceProvider.GetRequiredService(serviceType);
                }
            }
        }

        return reference;
    }

    //当前用户
    private ICurrentUser _currentUser;
    public ICurrentUser CurrentUser => LazyGetRequiredService(ref _currentUser);

    //实体映射
    private IMapper _mapper;
    public IMapper Mapper => LazyGetRequiredService(ref _mapper);

    //工作单元
    private UnitOfWorkManager unitOfWorkManager;
    public UnitOfWorkManager UnitOfWorkManager => LazyGetRequiredService(ref unitOfWorkManager);

    //日志工厂
    private ILoggerFactory _loggerFactory;
    public ILoggerFactory LoggerFactory => LazyGetRequiredService(ref _loggerFactory);

    //日志
    protected ILogger Logger => _lazyLogger.Value;
    private Lazy<ILogger> _lazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);

    //授权
    private IAuthorizationService _authorizationService;
    public IAuthorizationService AuthorizationService => LazyGetRequiredService(ref _authorizationService);


}
