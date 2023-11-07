namespace Mbill.Service.Core.User;

public interface IUserSvc
{
    /// <summary>
    /// 注册-新增一个用户
    /// </summary>
    /// <param name="input">用户信息</param>
    Task CreateAsync(ModifyUserDto input);

    /// <summary>
    /// 获取用户信息，id为空时，通过Token获取
    /// </summary>
    /// <param name="bId"></param>
    /// <returns></returns>
    Task<ServiceResult<UserDto>> GetAsync(long? bId);

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
    /// <param name="bId"></param>
    /// <returns></returns>
    Task<ServiceResult> DeleteAsync(long bId);
}