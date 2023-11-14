using Mbill.Service.Core.Notice;
using Mbill.Service.Core.Notice.Input;
using Mbill.Service.Core.Notice.Output;

namespace Mbill.Controllers.Core;

[Route("api/notice")]
[Authorize]
public class NoticeController : ApiControllerBase
{
    private readonly INoticeSvc _noticeSvc;

    public NoticeController(INoticeSvc noticeSvc)
    {
        _noticeSvc = noticeSvc;
    }

    /// <summary>
    /// 新增公告
    /// </summary>
    /// <param name="input">公告</param>
    [HttpPost]
    [LocalAuthorize("新增公告", "管理员")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
    public async Task<ServiceResult<NoticeDto>> CreateAsync([FromBody] ModifyNoticeDto input)
    {
        return await _noticeSvc.InsertAsync(input);
    }

    /// <summary>
    /// 获取最新公告
    /// </summary>
    [HttpGet("latest")]
    [LocalAuthorize("获取最新公告", "公告")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<NoticeDto>> GetLatestAsync()
    {
        return await _noticeSvc.GetLatestAsync();
    }
}
