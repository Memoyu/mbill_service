using Mbill.Core.Domains.Entities.Core;
using Mbill.Core.Interface.IRepositories.Core;
using Mbill.ToolKits.Qiniu;
using Microsoft.Extensions.Options;

namespace Mbill.Infrastructure.Repository.Core
{
    public class MediaImageRepo : AuditBaseRepo<MediaImageEntity>, IMediaImageRepo
    {
        public MediaImageRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
        }
    }
}
