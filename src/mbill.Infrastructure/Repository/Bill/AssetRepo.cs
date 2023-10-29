namespace Mbill.Infrastructure.Repository.Bill;

public class AssetRepo : AuditBaseRepo<AssetEntity>, IAssetRepo
{
    private readonly ICurrentUser _currentUser;
    public AssetRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
    {
        _currentUser = currentUser;
    }

    public async Task<AssetEntity> GetAssetAsync(long bId)
    {
        return await Select.Where(a => a.BId == bId).ToOneAsync();
    }
}
