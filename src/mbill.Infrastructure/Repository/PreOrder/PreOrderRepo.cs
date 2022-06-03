using mbill.Core.Domains.Common.Enums;

namespace mbill.Infrastructure.Repository.PreOrder;

public class PreOrderRepo : AuditBaseRepo<PreOrderEntity>, IPreOrderRepo
{
    private readonly ICurrentUser _currentUser;

    public PreOrderRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
    {
        _currentUser = currentUser;
    }

    public async Task<(long done, long unDone)> GetCountByStatusAsync(long groupId)
    {
        var results = await Select.Where(g => g.CreateUserId == _currentUser.Id).Where(g => g.GroupId == groupId).ToListAsync();
        var done = results.Where(g => g.Status == (int)PreOrderStatusEnum.Done).Count();
        var unDone = results.Where(g => g.Status == (int)PreOrderStatusEnum.UnDone).Count();
        return (done, unDone);
    }

    public async Task<decimal> GetAmountByGroupAsync(long groupId)
    {
        var amount = await Select.Where(g => g.CreateUserId == _currentUser.Id).Where(g => g.GroupId == groupId).SumAsync(g => g.Amount);
        return amount;
    }
}
