namespace mbill_service.Controllers.Core;

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

    /// <summary>
    /// 获取日志信息分页
    /// </summary>
    [HttpGet("pages")]
    [LocalAuthorize("获取分页数据", "管理员")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
    public async Task<ServiceResult<PagedDto<LogDto>>> GetPagesAsync([FromQuery] LogPagingDto pagingDto)
    {
        return ServiceResult<PagedDto<LogDto>>.Successed(await _logService.GetPagesAsync(pagingDto));
    }
}
