namespace Mbill.Infrastructure.Repository.Bill;

public class CategoryRepo : AuditBaseRepo<CategoryEntity>, ICategoryRepo
{
    private readonly ICurrentUser _currentUser;
    public CategoryRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
    {
        _currentUser = currentUser;
    }

    public async Task<CategoryEntity> GetCategoryAsync(long bId)
    {
        return await Select.Where(c => c.BId == bId).ToOneAsync();
    }
}
