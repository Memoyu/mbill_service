/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Entities.System
*   文件名称 ：RoleEntity.cs
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
using System;
using System.Collections.Generic;

namespace Memoyu.Mbill.Domain.Entities.System
{
    /// <summary>
    /// 角色表
    /// </summary>
    [Table(Name = SystemConst.DbTablePrefix + "_role")]
    public class RoleEntity : FullAduitEntity
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        public const string Administrator = "Administrator";

        /// <summary>
        /// 管理员
        /// </summary>
        public const string Admin = "Admin";

        /// <summary>
        /// 普通用户
        /// </summary>
        public const string User = "User";

        public RoleEntity()
        {

        }

        public RoleEntity(string name, string info, bool isStatic, long createUserId)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Info = info ?? throw new ArgumentNullException(nameof(info));
            IsStatic = isStatic;
            CreateUserId = createUserId;
            CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 角色唯一标识字符
        /// </summary>
        [Column(StringLength = 60)]
        public string Name { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        [Column(StringLength = 100)]
        public string Info { get; set; }

        /// <summary>
        /// 是否是静态分组,是静态时无法删除此分组
        /// </summary>
        public bool IsStatic { get; set; } = false;

        /// <summary>
        /// 排序码
        /// </summary>
        public int Sort { get; set; }


        [Navigate(ManyToMany = typeof(UserRoleEntity))]
        public virtual ICollection<UserEntity> Users { get; set; }


    }
}
