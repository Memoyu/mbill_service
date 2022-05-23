namespace mbill.Service.Core.User;

public interface IUserIdentitySvc
{
    /// <summary>
    /// 验证用户密码是否正确
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="password">密码</param>
    /// <returns></returns>
    Task<bool> VerifyUserPasswordAsync(long userId, string password);

    /// <summary>
    /// 验证微信用户OpenId是否存在
    /// </summary>
    /// <param name="openId">Wx OpenId</param>
    /// <returns></returns>
    Task<UserIdentityEntity> VerifyWxOpenIdAsync(string openId);

    /// <summary>
    /// 根据用户ID，修改用户的密码
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="newpassword"></param>
    Task ChangePasswordAsync(long userId, string newpassword);
}