/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Entities.System
*   文件名称 ：RolePermissionEntity.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 23:47:20
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql.DataAnnotations;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Shared.Const;

namespace Memoyu.Mbill.Domain.Entities.System
{
    /// <summary>
    /// 角色权限表
    /// </summary>
    [Table(Name = SystemConst.DbTablePrefix + "_role_permission")]
    public class RolePermissionEntity : Entity<long>
    {
        public RolePermissionEntity()
        {

        }

        public RolePermissionEntity(long roleId, long permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }

        public RolePermissionEntity(long permissionId)
        {
            PermissionId = permissionId;
        }

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
