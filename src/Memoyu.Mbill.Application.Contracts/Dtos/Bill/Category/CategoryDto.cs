/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Dtos.Bill.Category
*   文件名称 ：CategoryDto.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 23:47:51
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/

using Memoyu.Mbill.Application.Contracts.Dtos.Base;

namespace Memoyu.Mbill.Application.Contracts.Dtos.Bill.Category
{
    public class CategoryDto : EntityDto
    {
        public string Name { get; set; }

        public long ParentId { get; set; }

        public string Type { get; set; }

        public decimal Budget { get; set; }

        public string IconUrl { get; set; }
    }
}
