using FreeSql;
using Memoyu.Mbill.Domain.Entities.Asset;
using Memoyu.Mbill.Domain.IRepositories.Asset;
using Memoyu.Mbill.Domain.Base.Impl;
using Memoyu.Mbill.Domain.Shared.Security;

namespace Memoyu.Mbill.Domain.Repositories.Asset
{
    public class AssetRepository : AuditBaseRepository<AssetEntity>, IAssetRepository
    {
        private readonly ICurrentUser _currentUser;
        public AssetRepository(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
            _currentUser = currentUser;
        }
    }
}
