namespace Mbill.Service.Core.Logger;

public class LogSvc : ApplicationSvc, ILogSvc
{
    private readonly IIpLogRepo _ipLogRepo;
    public LogSvc(IIpLogRepo ipLogRepo)
    {
        _ipLogRepo = ipLogRepo;
    }
}
