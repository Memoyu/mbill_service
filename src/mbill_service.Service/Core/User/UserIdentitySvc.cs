namespace mbill_service.Service.Core.User;

public class UserIdentitySvc : IUserIdentitySvc
{
    private readonly IUserIdentityRepo _userIdentityRepo;
    public UserIdentitySvc(IUserIdentityRepo userIdentityRepository)
    {
        _userIdentityRepo = userIdentityRepository;
    }
    public Task ChangePasswordAsync(long userId, string newpassword)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> VerifyUserPasswordAsync(long userId, string password)
    {
        UserIdentityEntity userIdentity = await GetFirstByUserIdAsync(userId);
        return userIdentity != null && EncryptUtil.Verify(userIdentity.Credential, password);
    }

    public async Task<UserIdentityEntity> VerifyWxOpenIdAsync(string openId)
    {
        return await GetFirstByOpenIdAsync(openId); ;
    }

    /// <summary>
    /// 通过UserId获取用户绑定信息
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<UserIdentityEntity> GetFirstByUserIdAsync(long userId)
    {
        return await _userIdentityRepo
            .Where(r => r.UserId == userId && r.IdentityType == UserIdentityEntity.Password)
            .ToOneAsync();
    }

    /// <summary>
    /// 通过OpenId获取用户绑定信息
    /// </summary>
    /// <param name="openId">Wx OpenId</param>
    /// <returns></returns>
    public async Task<UserIdentityEntity> GetFirstByOpenIdAsync(string openId)
    {
        return await _userIdentityRepo
            .Where(r => r.Credential.Equals(openId) && r.IdentityType == UserIdentityEntity.WeiXin)
            .ToOneAsync();
    }
}