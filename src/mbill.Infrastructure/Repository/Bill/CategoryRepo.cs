namespace Mbill.Infrastructure.Repository.Bill;

public class CategoryRepo : AuditBaseRepo<CategoryEntity>, ICategoryRepo
{
    private readonly ICurrentUser _currentUser;
    public CategoryRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
    {
        _currentUser = currentUser;
    }

    public async Task<CategoryEntity> GetCategoryAsync(long id)
    {
        return await GetAsync(id);
    }

    public async Task<CategoryEntity> GetCategoryParentAsync(long id)
    {
        var asset = await GetAsync(id);
        if (asset == null) return null;
        return await GetAsync(asset.ParentId);
    }
}
