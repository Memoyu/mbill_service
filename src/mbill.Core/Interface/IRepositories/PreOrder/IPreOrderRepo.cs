namespace mbill.Core.Interface.IRepositories.PreOrder;

public interface IPreOrderRepo : IAuditBaseRepo<PreOrderEntity>
{
    /// <summary>
    /// 获取指定用户、指定状态的预购清单总数
    /// </summary>
    /// <param name="status">状态</param>
    /// <returns></returns>
    Task<(long none, long unNone)> GetCountByStatusAsync(int status);
}
