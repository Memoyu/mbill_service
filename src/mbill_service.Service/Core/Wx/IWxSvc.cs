namespace mbill_service.Service.Core.Wx;

public interface IWxSvc
{
    /// <summary>
    /// GetCode2Session
    /// </summary>
    /// <param name="code">wx.login获取到的code</param>
    /// <returns></returns>
    Task<ServiceResult<WxCode2SessionDto>> GetCode2Session(string code);
}