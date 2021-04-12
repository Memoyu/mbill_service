﻿using mbill_service.Core.Domains.Entities.Core;
using mbill_service.Core.Interface.IRepositories.Core;
using mbill_service.Core.Security;
using mbill_service.Infrastructure.Repository.Base;
using FreeSql;

namespace mbill_service.Infrastructure.Repository.Core
{
    public class RoleRepo : AuditBaseRepo<RoleEntity>, IRoleRepo
    {
        public RoleRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
        }
    }
}
