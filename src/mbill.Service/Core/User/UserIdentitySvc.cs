namespace Mbill.Service.Core.User;

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

    public async Task<bool> VerifyUserPasswordAsync(long userBId, string password)
    {
        UserIdentityEntity userIdentity = await GetFirstByUserIdAsync(userBId);
        return userIdentity != null && EncryptUtil.Verify(userIdentity.Credential, password);
    }

    public async Task<UserIdentityEntity> VerifyWxOpenIdAsync(string openId)
    {
        return await GetFirstByOpenIdAsync(openId); ;
    }

    /// <summary>
    /// 通过UserId获取用户绑定信息
    /// </summary>
    /// <param name="userBId"></param>
    /// <returns></returns>
    public async Task<UserIdentityEntity> GetFirstByUserIdAsync(long userBId)
    {
        return await _userIdentityRepo
            .Where(r => r.UserBId == userBId && r.IdentityType == UserIdentityEntity.Password)
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
            .Where(r => r.Credential.Equals(openId) && r.IdentityType == UserIdentityEntity.WeiXin && r.IsDeleted == false)
            .ToOneAsync();
    }
}