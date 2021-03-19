/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Dtos.Bill.Statement
*   文件名称 ：StatementPagingDto.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-22 22:06:32
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/

using Memoyu.Mbill.ToolKits.Base.Page;
using System;

namespace Memoyu.Mbill.Application.Contracts.Dtos.Bill.Statement
{
    public class StatementPagingDto : PagingDto
    {
        public long? UserId { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }

        public int? Day { get; set; }

        public string  Type { get; set; }

        public long? CategoryId { get; set; }

        public long? AssetId { get; set; }

    }
}
