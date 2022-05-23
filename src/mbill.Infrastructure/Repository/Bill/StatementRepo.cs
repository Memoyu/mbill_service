namespace mbill.Infrastructure.Repository.Bill;

public class StatementRepo : AuditBaseRepo<BillEntity>, IBillRepo
{
    private readonly ICurrentUser _currentUser;
    public StatementRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
    {
        _currentUser = currentUser;
    }
}
