using mbill.Core.Domains.Common.Enums;

namespace mbill.Infrastructure.Repository.PreOrder;

public class PreOrderRepo : AuditBaseRepo<PreOrderEntity>, IPreOrderRepo
{
    private readonly ICurrentUser _currentUser;

    public PreOrderRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
    {
        _currentUser = currentUser;
    }

    public async Task<(long none, long unNone)> GetCountByStatusAsync(int status)
    {
        var select = Select.Where(g => g.CreateUserId == _currentUser.Id);
        var none = await select.Where(g => g.Status == (int)PreOrderStatusEnum.None).CountAsync();
        var unNone = await select.Where(g => g.Status == (int)PreOrderStatusEnum.UnNone).CountAsync();
        return (none, unNone);
    }
}
