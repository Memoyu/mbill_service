namespace Mbill.Service.Bill.Asset;

public interface IAssetSvc
{
    /// <summary>
    /// 新增资产分类
    /// </summary>
    /// <param name="input">数据源</param>
    /// <returns></returns>
    Task<ServiceResult<AssetDto>> InsertAsync(CreateAssetInput input);

    /// <summary>
    /// 删除资产分类
    /// </summary>
    /// <param name="bId">资产分类bId</param>
    /// <returns></returns>
    Task<ServiceResult> DeleteAsync(long bId);

    /// <summary>
    /// 更新资产分类
    /// </summary>
    /// <param name="input">资产分类信息</param>
    /// <returns></returns>
    Task<ServiceResult<AssetDto>> EditAsync(EditAssetInput input);

    /// <summary>
    /// 获取父项 by bId
    /// </summary>
    /// <param name="bId">资产bId</param>
    /// <returns></returns>
    Task<ServiceResult<AssetDto>> GetAsync(long bId);

    /// <summary>
    /// 获取父项 by 子项 bId
    /// </summary>
    /// <param name="bId">资产子项bId</param>
    /// <returns></returns>
    Task<ServiceResult<AssetDto>> GetParentAsync(long bId);

    /// <summary>
    /// 获取父项集合
    /// </summary>
    /// <returns></returns>
    Task<ServiceResult<IEnumerable<AssetDto>>> GetParentsAsync();

    /// <summary>
    /// 获取分级后的组合类别数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    Task<ServiceResult<IEnumerable<AssetGroupDto>>> GetGroupsAsync(int? type);

    /// <summary>
    /// 获取资产分类分页
    /// </summary>
    /// <param name="pagingDto">分页参数</param>
    /// <returns></returns>
    Task<ServiceResult<PagedDto<AssetPageDto>>> GetPageAsync(AssetPagingDto pagingDto);

    /// <summary>
    /// 排序资产
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult> SortAsync(SortAssetInput input);
}