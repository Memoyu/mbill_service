/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Entities.System
*   文件名称 ：UserRoleEntity.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-09 11:49:40
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql.DataAnnotations;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Entities.User;
using Memoyu.Mbill.Domain.Shared.Const;

namespace Memoyu.Mbill.Domain.Entities.System
{
    /// <summary>
    /// 用户角色表
    /// </summary>
    [Table(Name = SystemConst.DbTablePrefix + "_user_role")]
    public class UserRoleEntity:Entity
    {
        public UserRoleEntity()
        {
        }
        public UserRoleEntity(long userId, long roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }


        [Navigate("UserId")]
        public UserEntity User { get; set; }

        [Navigate("RoleId")]
        public RoleEntity Role { get; set; }

    }
}
