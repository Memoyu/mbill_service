using Mbill.Core.Domains.Entities.Core;
using Mbill.Core.Interface.IRepositories.Core;

namespace Mbill.Infrastructure.Repository.Core;

public class NoticeRepo : AuditBaseRepo<NoticeEntity>, INoticeRepo
{
    public NoticeRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
    {
    }
}
