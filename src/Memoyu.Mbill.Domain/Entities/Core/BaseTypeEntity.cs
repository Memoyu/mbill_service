/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Entities.Core
*   文件名称 ：BaseType.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-10 12:08:55
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql.DataAnnotations;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Shared.Const;
using System;
using System.Collections.Generic;

namespace Memoyu.Mbill.Domain.Entities.Core
{
    /// <summary>
    /// 字典类型表
    /// </summary>
    [Table(Name = SystemConst.DbTablePrefix + "_type")]
    public class BaseTypeEntity:FullAduitEntity
    {
        public BaseTypeEntity()
        {
        }

        public BaseTypeEntity(string typeCode, string fullName, int? sort)
        {
            TypeCode = typeCode ?? throw new ArgumentNullException(nameof(typeCode));
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            Sort = sort;
        }

        /// <summary>
        /// 字典类型编码
        /// </summary>
        [Column(StringLength = 50)]
        public string TypeCode { get; set; }

        /// <summary>
        /// 字典类型名
        /// </summary>
        [Column(StringLength = 50)]
        public string FullName { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        public int? Sort { get; set; }

        public virtual ICollection<BaseItemEntity> BaseItems { get; set; }
    }
}
