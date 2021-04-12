using FreeSql;
using mbill_service.Core.Domains.Entities.Bill;
using mbill_service.Core.Interface.IRepositories.Bill;
using mbill_service.Core.Security;
using mbill_service.Infrastructure.Repository.Base;
using System.Threading.Tasks;

namespace mbill_service.Infrastructure.Repository.Bill
{
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
            return await GetAsync(asset.ParentId);
        }
    }
}
