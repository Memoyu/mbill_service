using Mbill.Core.Domains.Common.Enums;

namespace Mbill.Infrastructure.Repository.PreOrder;

public class PreOrderRepo : AuditBaseRepo<PreOrderEntity>, IPreOrderRepo
{
    private readonly ICurrentUser _currentUser;

    public PreOrderRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
    {
        _currentUser = currentUser;
    }

    public async Task<(long done, long unDone)> GetCountByStatusAsync(List<long> groupBIds)
    {
        var results = await Select.Where(g => g.CreateUserBId == _currentUser.BId).Where(g => groupBIds.Contains(g.GroupBId)).ToListAsync();
        var done = results.Where(g => g.Status == (int)PreOrderStatusEnum.Done).Count();
        var unDone = results.Where(g => g.Status == (int)PreOrderStatusEnum.UnDone).Count();
        return (done, unDone);
    }

    public async Task<decimal> GetPreAmountByGroupAsync(List<long> groupBIds)
    {
        var amount = await Select.Where(g => g.CreateUserBId == _currentUser.BId).Where(g => groupBIds.Contains(g.GroupBId)).SumAsync(g => g.PreAmount);
        return amount;
    }

    public async Task<decimal> GetRealAmountByGroupAsync(List<long> groupIds)
    {
        var amount = await Select.Where(g => g.CreateUserBId == _currentUser.BId).Where(g => groupIds.Contains(g.GroupBId)).SumAsync(g => g.RealAmount);
        return amount;
    }
}
