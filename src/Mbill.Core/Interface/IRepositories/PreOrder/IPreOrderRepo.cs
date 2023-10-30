namespace Mbill.Core.Interface.IRepositories.PreOrder;

public interface IPreOrderRepo : IAuditBaseRepo<PreOrderEntity>
{
    /// <summary>
    /// 获取指定用户、指定状态的预购清单总数
    /// </summary>
    /// <param name="groupBIds">分组</param>
    /// <returns></returns>
    Task<(long done, long unDone)> GetCountByStatusAsync(List<long> groupBIds);

    /// <summary>
    /// 获取分组下预购清单总预购金额
    /// </summary>
    /// <param name="groupBIds"></param>
    /// <returns></returns>
    Task<decimal> GetPreAmountByGroupAsync(List<long> groupBIds);

    /// <summary>
    /// 获取分组下预购清单总实购金额
    /// </summary>
    /// <param name="groupBIds"></param>
    /// <returns></returns>
    Task<decimal> GetRealAmountByGroupAsync(List<long> groupBIds);
}
