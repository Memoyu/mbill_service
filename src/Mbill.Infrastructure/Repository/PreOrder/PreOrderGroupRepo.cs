namespace Mbill.Infrastructure.Repository.PreOrder;

public class PreOrderGroupRepo : AuditBaseRepo<PreOrderGroupEntity>, IPreOrderGroupRepo
{
    public PreOrderGroupRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
    {
    }

    public async Task<PreOrderGroupEntity> GetPreOrderGroupAsync(long bId)
    {
        return await Select.Where(a => a.BId == bId).ToOneAsync();
    }

}
