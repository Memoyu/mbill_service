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

namespace Memoyu.Mbill.Domain.Entities.User
{
    /// <summary>
    /// 用户实体
    /// </summary>
    [Table(Name = "mbill_user")]
    public class UserEntity : FullAduitEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Column(StringLength = 20, IsNullable = false)]
        public string Name { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Column(StringLength = 20, IsNullable = false)]
        public string NickName { get; set; }

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
        [Column(DbType = "nvarchar(20)")]
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        [Column(DbType = "nvarchar(20)")]
        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        [Column(DbType = "nvarchar(20)")]
        public string District { get; set; }

        /// <summary>
        /// 街道
        /// </summary>
        [Column(DbType = "nvarchar(50)")]
        public string Street { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        [Column(StringLength = 100)]
        public string AvatarUrl { get; set; }
    }
}
