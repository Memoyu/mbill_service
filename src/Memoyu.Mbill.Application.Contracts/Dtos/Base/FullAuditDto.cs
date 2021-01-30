/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Dtos.Base
*   文件名称 ：FullAuditDto.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-23 15:00:24
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Contracts.Dtos.Base
{

    #region EntityDto
    public interface IEntityDto
    {
    }

    public interface IEntityDto<TKey> : IEntityDto
    {
        TKey Id { get; set; }
    }

    public abstract class EntityDto<TKey> : IEntityDto<TKey>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public TKey Id { get; set; }
    }

    public abstract class EntityDto : EntityDto<long>
    {
    }
    #endregion EntityDto

}
