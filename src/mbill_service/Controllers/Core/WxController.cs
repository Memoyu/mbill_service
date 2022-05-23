namespace mbill_service.Controllers.Core;

/// <summary>
/// 账户相关
/// </summary>
[Route("api/wx")]
[ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v3)]
public class WxController : ApiControllerBase
{
    private readonly IWxSvc _wxSvc;

    public WxController(IWxSvc wxSvc)
    {
        _wxSvc = wxSvc;
    }

    /// <summary>
    /// 小程序 GetCode2Session
    /// </summary>
    /// <param name="code">wx.login获取到的code</param>
    /// <returns></returns>
    [HttpGet("getcode2session")]
    public async Task<ServiceResult<WxCode2SessionDto>> GetCode2Session(string code)
    {
        return await _wxSvc.GetCode2Session(code);
    }
}
