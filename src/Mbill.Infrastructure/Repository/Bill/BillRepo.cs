namespace Mbill.Infrastructure.Repository.Bill;

public class BillRepo : AuditBaseRepo<BillEntity>, IBillRepo
{
    private readonly ICurrentUser _currentUser;

    public BillRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
    {
        _currentUser = currentUser;
    }

    public async Task<BillEntity> GetBillAsync(long bId)
    {
        return await Select.Where(a => a.BId == bId).ToOneAsync();
    }
}
