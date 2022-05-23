namespace mbill_service.Core.Interface.IRepositories.Bill;

public interface IAssetRepo : IAuditBaseRepo<AssetEntity, long>
{
    /// <summary>
    /// 获取资产信息 By Id
    /// </summary>
    /// <param name="id">资产Id</param>
    /// <returns></returns>
    Task<AssetEntity> GetAssetAsync(long id);

    /// <summary>
    /// 获取资产父项信息 By 子项 Id
    /// </summary>
    /// <param name="id">资产子项Id</param>
    /// <returns></returns>
    Task<AssetEntity> GetAssetParentAsync(long id);
}

