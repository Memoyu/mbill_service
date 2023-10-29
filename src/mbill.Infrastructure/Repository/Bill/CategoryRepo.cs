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
        return await GetAsync(bId);
    }

    public async Task<CategoryEntity> GetCategoryParentAsync(long bId)
    {
        var asset = await GetAsync(bId);
        if (asset == null) return null;
        return await GetAsync(asset.ParentBId);
    }
}
