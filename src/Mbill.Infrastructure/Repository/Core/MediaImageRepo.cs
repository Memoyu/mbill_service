using Mbill.Core.Domains.Entities.Core;
using Mbill.Core.Interface.IRepositories.Core;

namespace Mbill.Infrastructure.Repository.Core
{
    public class MediaImageRepo : AuditBaseRepo<MediaImageEntity>, IMediaImageRepo
    {
        public MediaImageRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
        }
    }
}
