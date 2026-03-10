namespace Memo.Bill.Application.Common.Security;

public interface ICurrentUserProvider
{
    /// <summary>
    /// 获取当前登录用户Id
    /// </summary>
    long UserId => GetCurrentUser().Id;

    /// <summary>
    /// 获取当前登录用户
    /// </summary>
    /// <returns></returns>
    CurrentUser GetCurrentUser();

    /// <summary>
    /// 获取当前请求Ip
    /// </summary>
    /// <returns></returns>
    string GetClientIp();
}
