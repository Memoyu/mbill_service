namespace Mbill.Core.Interface.IRepositories.Bill;

public interface IAssetRepo : IAuditBaseRepo<AssetEntity, long>
{
    /// <summary>
    /// 获取资产信息 By bId
    /// </summary>
    /// <param name="bId">资产Id</param>
    /// <returns></returns>
    Task<AssetEntity> GetAssetAsync(long bId);
}

