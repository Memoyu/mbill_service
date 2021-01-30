/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Entities.Core
*   文件名称 ：FileEntity.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-23 19:11:21
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql.DataAnnotations;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Shared.Const;
using System.Collections.Generic;

namespace Memoyu.Mbill.Domain.Entities.Core
{
    /// <summary>
    /// 上传文件表
    /// </summary>
    [Table(Name = SystemConst.DbTablePrefix + "_file")]
    public class FileEntity : FullAduitEntity
    {
        /// <summary>
        /// 后缀
        /// </summary>
        [Column(StringLength = 50)]
        public string Extension { get; set; } = string.Empty;

        /// <summary>
        /// 图片md5值，防止上传重复图片
        /// </summary>
        [Column(StringLength = 40)]
        public string Md5 { get; set; } = string.Empty;

        /// <summary>
        /// 名称
        /// </summary>
        [Column(StringLength = 300)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 路径
        /// </summary>
        [Column(StringLength = 500)]
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// 大小
        /// </summary>
        public long? Size { get; set; }

        /// <summary>
        /// 1 local
        /// </summary>
        public short? Type { get; set; }

        public static string LocalFileService = "LocalFileService";
        public static Dictionary<string, string> SourceFileDic = new Dictionary<string, string>
        {
            { "asset","core/images/asset"},
            { "category","core/images/category"},
            { "avatar","core/images/avatar"},
            { "other","core/images/other"}
        };
    }
}
