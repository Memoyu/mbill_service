namespace Mbill.Infrastructure.Repository.Bill;

public class AssetRepo : AuditBaseRepo<AssetEntity>, IAssetRepo
    {
        private readonly ICurrentUser _currentUser;
        public AssetRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
            _currentUser = currentUser;
        }

        public async Task<AssetEntity> GetAssetAsync(long id)
        {
            return await GetAsync(id);
        }

        public async Task<AssetEntity> GetAssetParentAsync(long id)
        {
            var asset = await GetAsync(id);
            if (asset == null) return null;
            return await GetAsync(asset.ParentBId);
        }
    }
