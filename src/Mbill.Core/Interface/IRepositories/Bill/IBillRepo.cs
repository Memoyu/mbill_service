namespace Mbill.Core.Interface.IRepositories.Bill;

public interface IBillRepo : IAuditBaseRepo<BillEntity>
{
    /// <summary>
    /// 获取账单信息
    /// </summary>
    /// <param name="bId">账单BId</param>
    /// <returns></returns>
    Task<BillEntity> GetBillAsync(long bId);
}
