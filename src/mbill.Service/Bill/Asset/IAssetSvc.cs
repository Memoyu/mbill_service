namespace mbill.Service.Bill.Asset;

public interface IAssetSvc
{
    /// <summary>
    /// 新增资产分类
    /// </summary>
    /// <param name="input">数据源</param>
    /// <returns></returns>
    Task InsertAsync(AssetEntity input);

    /// <summary>
    /// 删除资产分类
    /// </summary>
    /// <param name="id">资产分类id</param>
    /// <returns></returns>
    Task DeleteAsync(long id);

    /// <summary>
    /// 更新资产分类
    /// </summary>
    /// <param name="entity">资产分类信息</param>
    /// <returns></returns>
    Task UpdateAsync(AssetEntity entity);

    /// <summary>
    /// 获取分级后的组合类别数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    Task<IEnumerable<AssetGroupDto>> GetGroupsAsync(int? type);

    /// <summary>
    /// 获取资产分类分页
    /// </summary>
    /// <param name="pagingDto">分页参数</param>
    /// <returns></returns>
    Task<PagedDto<AssetPageDto>> GetPageAsync(AssetPagingDto pagingDto);

    /// <summary>
    /// 获取父项 by id
    /// </summary>
    /// <param name="id">资产Id</param>
    /// <returns></returns>
    Task<AssetDto> GetAsync(long id);

    /// <summary>
    /// 获取父项 by 子项 id
    /// </summary>
    /// <param name="id">资产子项Id</param>
    /// <returns></returns>
    Task<AssetDto> GetParentAsync(long id);

    /// <summary>
    /// 获取父项集合
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<AssetDto>> GetParentsAsync();
}