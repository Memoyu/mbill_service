/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Entities.Core
*   文件名称 ：LogEntity.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-09 11:54:11
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql.DataAnnotations;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Shared.Const;

namespace Memoyu.Mbill.Domain.Entities.Core
{
    /// <summary>
    /// 日志表
    /// </summary>
    [Table(Name = SystemConst.DbTablePrefix + "_log")]
    public class LogEntity : FullAduitEntity
    {
        /// <summary>
        /// 访问哪个权限
        /// </summary>
        [Column(StringLength = 100)]
        public string Authority { get; set; }

        /// <summary>
        /// 日志信息
        /// </summary>
        [Column(StringLength = 500)]
        public string Message { get; set; }

        /// <summary>
        /// 请求方法
        /// </summary>
        [Column(StringLength = 20)]
        public string Method { get; set; }

        /// <summary>
        /// 请求路径
        /// </summary>
        [Column(StringLength = 100)]
        public string Path { get; set; }

        /// <summary>
        /// 请求的http返回码
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户当时的昵称
        /// </summary>
        [Column(StringLength = 24)]
        public string Username { get; set; }

    }
}
