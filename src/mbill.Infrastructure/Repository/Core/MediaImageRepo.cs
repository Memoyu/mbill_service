using mbill.Core.Domains.Entities.Core;
using mbill.Core.Interface.IRepositories.Core;
using mbill.ToolKits.Qiniu;
using Microsoft.Extensions.Options;

namespace mbill.Infrastructure.Repository.Core
{
    public class MediaImageRepo : AuditBaseRepo<MediaImageEntity>, IMediaImageRepo
    {
        private readonly QiniuClientOption _qiniuOption;

        public MediaImageRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
        }
    }
}
