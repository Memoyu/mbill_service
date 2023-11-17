namespace Mbill.Controllers.Core;

/// <summary>
/// 日志管理
/// </summary>
[Route("api/admin/log")]
[ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
public class LogController : ApiControllerBase
{
    private readonly ILogSvc _logService;
    public LogController(ILogSvc logService)
    {
        _logService = logService;
    }
}
