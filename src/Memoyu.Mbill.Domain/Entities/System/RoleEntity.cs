using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Domain.Entities.System
{
    /// <summary>
    /// 用户角色
    /// </summary>
    [Table(Name = "mbill_role")]
    public class RoleEntity
    {
        /// <summary>
        /// 角色唯一标识字符
        /// </summary>
        [Column(StringLength = 60)]
        public string Name { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        [Column(StringLength = 100)]
        public string Info { get; set; }

        /// <summary>
        /// 是否是静态分组,是静态时无法删除此分组
        /// </summary>
        public bool IsStatic { get; set; } = false;

        /// <summary>
        /// 排序码，升序
        /// </summary>
        public int SortCode { get; set; }
    }
}
