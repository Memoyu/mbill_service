namespace Mbill.Core.Interface.IRepositories.Core;

public interface IUserRepo : IAuditBaseRepo<UserEntity>
{
    /// <summary>
    /// 获取用户信息 By bId
    /// </summary>
    /// <param name="bId">用户bId</param>
    /// <returns></returns>
    Task<UserEntity> GetUserAsync(long bId);

    /// <summary>
    /// 根据条件得到用户信息
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    Task<UserEntity> GetUserAsync(Expression<Func<UserEntity, bool>> expression);

    /// <summary>
    /// 根据用户Id更新用户的最后登录时间
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task UpdateLastLoginTimeAsync(long userId);
}
