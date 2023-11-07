namespace Mbill.Core.Interface.IRepositories.PreOrder;

public interface IPreOrderGroupRepo : IAuditBaseRepo<PreOrderGroupEntity>
{
    /// <summary>
    /// 获取预购分组
    /// </summary>
    /// <param name="bId"></param>
    /// <returns></returns>
    Task<PreOrderGroupEntity> GetPreOrderGroupAsync(long bId);
}
