using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Domain.Entities.System
{
    public class RolePermissionEntity
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        public long PermissionId { get; set; }

    }
}
