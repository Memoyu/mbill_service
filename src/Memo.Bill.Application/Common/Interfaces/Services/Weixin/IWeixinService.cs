using Memo.Bill.Application.Common.Models.Services.Weixin;

namespace Memo.Bill.Application.Common.Interfaces.Services.Weixin;

public interface IWeixinService
{
    /// <summary>
    /// 微信小程序登录凭证校验
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    Task<WeixinCode2SessionResponse> Code2SessionAsync(string code);
}
