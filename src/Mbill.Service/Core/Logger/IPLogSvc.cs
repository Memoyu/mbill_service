namespace Mbill.Service.Core.Logger;

public class IPLogSvc : ApplicationSvc, IIPLogSvc
{
    private readonly IIPLogRepo _ipLogRepo;
    public IPLogSvc(IIPLogRepo ipLogRepo)
    {
        _ipLogRepo = ipLogRepo;
    }
}
