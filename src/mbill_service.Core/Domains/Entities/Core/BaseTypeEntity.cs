using mbill_service.Core.Domains.Common.Base;
using mbill_service.Core.Domains.Common.Consts;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;

namespace mbill_service.Core.Domains.Entities.Core
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
