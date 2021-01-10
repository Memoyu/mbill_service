/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Entities.System
*   文件名称 ：UserIdentity.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-10 12:29:03
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql.DataAnnotations;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Shared.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Domain.Entities.System
{
    /// <summary>
    /// 用户身份认证登录表
    /// </summary>
    [Table(Name = SystemConst.DbTablePrefix +  "_user_identity")]
    public class UserIdentityEntity : FullAduitEntity<Guid>
    {
        public const string GitHub = "GitHub";
        public const string Password = "Password";
        public const string QQ = "QQ";
        public const string Gitee = "Gitee";
        public const string WeiXin = "WeiXin";

        public UserIdentityEntity()
        {
        }

        public UserIdentityEntity(string identityType, string identifier, string credential, DateTime createTime)
        {
            IdentityType = identityType ?? throw new ArgumentNullException(nameof(identityType));
            Identifier = identifier;
            Credential = credential ?? throw new ArgumentNullException(nameof(credential));
            CreateTime = createTime;
        }

        /// <summary>
        ///认证类型， Password，GitHub、QQ、WeiXin等
        /// </summary>
        [Column(StringLength = 20)]
        public string IdentityType { get; set; }

        /// <summary>
        /// 认证者，例如 用户名,手机号，邮件等，
        /// </summary>
        [Column(StringLength = 24)]
        public string Identifier { get; set; }

        /// <summary>
        ///  凭证，例如 密码,存OpenId、Id，同一IdentityType的OpenId的值是唯一的
        /// </summary>
        [Column(StringLength = 50)]
        public string Credential { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ExtraProperties { get; set; }
    }
}
