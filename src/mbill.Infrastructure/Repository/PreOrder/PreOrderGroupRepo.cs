namespace mbill.Infrastructure.Repository.PreOrder;

public class PreOrderGroupRepo : AuditBaseRepo<PreOrderGroupEntity>, IPreOrderGroupRepo
{
    public PreOrderGroupRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
    {
    }
}
