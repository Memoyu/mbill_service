/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Dtos.Bill.Category
*   文件名称 ：CategoryGroupDto.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-23 0:11:24
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System.Collections.Generic;

namespace Memoyu.Mbill.Application.Contracts.Dtos.Bill.Category
{
    public class CategoryGroupDto
    {
        public string Name { get; set; }

        public List<CategoryDto> Childs { get; set; }
    }
}
