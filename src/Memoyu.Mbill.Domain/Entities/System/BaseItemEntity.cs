/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Entities.System
*   文件名称 ：BaseItem.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-10 12:09:04
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql.DataAnnotations;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Shared.Const;
using System;

namespace Memoyu.Mbill.Domain.Entities.System
{
    /// <summary>
    /// 字典项详情表
    /// </summary>
    [Table(Name = SystemConst.DbTablePrefix + "_item")]
    public class BaseItemEntity:FullAduitEntity
    {
        public BaseItemEntity()
        {
        }

        public BaseItemEntity(string itemCode, string itemName, bool status, int? sort)
        {
            ItemCode = itemCode ?? throw new ArgumentNullException(nameof(itemCode));
            ItemName = itemName ?? throw new ArgumentNullException(nameof(itemName));       
            Status = status;
            Sort = sort;
        }

        public BaseItemEntity(string itemCode, string itemName, int baseTypeId, bool status, int? sort) : this(itemCode, itemName, status, sort)
        {
            BaseTypeId = baseTypeId;
        }

        /// <summary>
        /// 字典项所属TypeId
        /// </summary>
        public long BaseTypeId { get; set; }

        /// <summary>
        /// 字典项编码
        /// </summary>
        [Column(StringLength = 50)]
        public string ItemCode { get; set; }

        /// <summary>
        /// 字典项名称
        /// </summary>
        [Column(StringLength = 50)]
        public string ItemName { get; set; }

        /// <summary>
        /// 状态：0禁用，1启用
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        public int? Sort { get; set; }

        public virtual BaseTypeEntity BaseType { get; set; }
    }
}
