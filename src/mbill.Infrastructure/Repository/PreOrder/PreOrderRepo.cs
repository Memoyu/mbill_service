using mbill.Core.Domains.Common.Enums;

namespace mbill.Infrastructure.Repository.PreOrder;

public class PreOrderRepo : AuditBaseRepo<PreOrderEntity>, IPreOrderRepo
{
    private readonly ICurrentUser _currentUser;

    public PreOrderRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
    {
        _currentUser = currentUser;
    }

    public async Task<(long done, long unDone)> GetCountByStatusAsync(int status)
    {
        var select = Select.Where(g => g.CreateUserId == _currentUser.Id);
        var done = await select.Where(g => g.Status == (int)PreOrderStatusEnum.Done).CountAsync();
        var unDone = await select.Where(g => g.Status == (int)PreOrderStatusEnum.UnDone).CountAsync();
        return (done, unDone);
    }
}
