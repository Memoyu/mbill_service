namespace mbill.Service.Core.User;

public interface IUserSvc
{
    /// <summary>
    /// 注册-新增一个用户
    /// </summary>
    /// <param name="user">用户</param>
    /// <param name="roleIds">分组Id集合</param>
    /// <param name="password">密码</param>
    Task CreateAsync(UserEntity user, List<long> roleIds, string password);

    /// <summary>
    /// 获取用户信息，id为空时，通过Token获取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ServiceResult<UserDto>> GetAsync(long? id);

    /// <summary>
    /// 获取用户分页信息
    /// </summary>
    /// <param name="pagingDto">分页数据</param>
    /// <returns></returns>
    Task<ServiceResult<PagedDto<UserDto>>> GetPagesAsync(UserPagingDto pagingDto);

    /// <summary>
    /// 更新用户信息
    /// </summary>
    /// <param name="input">实体数据</param>
    /// <returns></returns>
    Task<ServiceResult> UpdateAsync(ModifyUserBaseDto input);

    /// <summary>
    /// 删除用户（软删除）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ServiceResult> DeleteAsync(long id);
}