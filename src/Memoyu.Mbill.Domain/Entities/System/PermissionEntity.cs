/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Entities.System
*   文件名称 ：PermissionEntity.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-08 15:32:20
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql.DataAnnotations;
using Memoyu.Mbill.Domain.Base;

namespace Memoyu.Mbill.Domain.Entities.System
{
    /// <summary>
    /// 权限表
    /// </summary>
    [Table(Name = "mbill_permission")]
    public class PermissionEntity : FullAduitEntity
    {
        /// <summary>
        /// 所属权限、权限名称，例如：访问首页
        /// </summary>
        [Column(StringLength = 60)]
        public string Name { get; set; }

        /// <summary>
        /// 权限所属模块，例如：人员管理
        /// </summary>
        [Column(StringLength = 50)]
        public string Module { get; set; }

        /// <summary>
        /// 后台路由
        /// </summary>
        [Column(StringLength = 200)]
        public string Router { get; set; }
    }
}
