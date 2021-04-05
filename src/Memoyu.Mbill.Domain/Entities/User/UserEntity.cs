/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Entities.User
*   文件名称 ：UserEntity.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 9:50:20
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql.DataAnnotations;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Entities.Core;
using Memoyu.Mbill.Domain.Shared.Const;
using System;
using System.Collections.Generic;

namespace Memoyu.Mbill.Domain.Entities.User
{
    /// <summary>
    /// 用户实体
    /// </summary>
    [Table(Name = SystemConst.DbTablePrefix + "_user")]
    public class UserEntity : FullAduitEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Column(StringLength = 20, IsNullable = false)]
        public string Username { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Column(StringLength = 20, IsNullable = false)]
        public string Nickname { get; set; }

        /// <summary>
        /// 性别，0：未知，1：男，2：女
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Column(StringLength = 60)]
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Column(StringLength = 20)]
        public string Phone { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        [Column(StringLength = 20)]
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        [Column(StringLength = 20)]
        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        [Column(StringLength = 20)]
        public string District { get; set; }

        /// <summary>
        /// 街道
        /// </summary>
        [Column(StringLength = 50)]
        public string Street { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        [Column(StringLength = 100)]
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 最后一次登录的时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// JWT 登录，保存生成的随机token值。
        /// </summary>
        [Column(StringLength = 200)]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; } = true;


        [Navigate(ManyToMany = typeof(UserRoleEntity))]
        public virtual IEnumerable<RoleEntity> Roles { get; set; }

        [Navigate("UserId")]
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }

        [Navigate("CreateUserId")]
        public virtual ICollection<UserIdentityEntity> UserIdentitys { get; set; }


        /// <summary>
        /// 登录后用户状态变化
        /// </summary>
        /// <param name="refreshToken"></param>
        public void ChangeLoginStatus(string refreshToken)
        {
            LastLoginTime = DateTime.Now;
            RefreshToken = refreshToken;
        }
    }
}
