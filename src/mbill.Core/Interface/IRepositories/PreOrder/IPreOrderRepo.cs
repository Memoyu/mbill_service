namespace mbill.Core.Interface.IRepositories.PreOrder;

public interface IPreOrderRepo : IAuditBaseRepo<PreOrderEntity>
{
    /// <summary>
    /// 获取指定用户、指定状态的预购清单总数
    /// </summary>
    /// <param name="groupId">分组</param>
    /// <returns></returns>
    Task<(long done, long unDone)> GetCountByStatusAsync(long groupId);

    /// <summary>
    /// 获取分组下预购清单总金额
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    Task<decimal> GetAmountByGroupAsync(long groupId);
}
