/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Entities.Bill.Category
*   文件名称 ：CategoryEntity.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 9:50:20
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql.DataAnnotations;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Shared.Const;

namespace Memoyu.Mbill.Domain.Entities.Bill.Category
{
    /// <summary>
    /// 账单分类实体
    /// </summary>
    [Table(Name = SystemConst.DbTablePrefix + "_category")]
    [Index("index_category_on_sort", "Sort", false)]
    [Index("index_category_on_parent_id", "ParentId", false)]
    [Index("index_category_on_type", "Type", false)]
    public class CategoryEntity : FullAduitEntity
    {
        /// <summary>
        /// 账单分类名
        /// </summary>
        [Column(StringLength = 20, IsNullable = false)]
        public string Name { get; set; }

        /// <summary>
        /// 父级Id，默认0
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 分类类型：支出、收入
        /// </summary>
        [Column(StringLength = 10, IsNullable = false)]
        public string Type { get; set; }

        /// <summary>
        /// 预算金额
        /// </summary>
        [Column(Precision = 12, Scale = 2)]
        public decimal Budget { get; set; }

        /// <summary>
        /// 图标地址
        /// </summary>
        [Column(StringLength = 100)]
        public string IconUrl { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
