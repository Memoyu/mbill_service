/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.ToolKits.Base.Page
*   文件名称 ：PagedDto.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-02 12:09:38
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System.Collections.Generic;

namespace Memoyu.Mbill.ToolKits.Base.Page
{
    public class PagedDto<T>
    {
        public long Total { get; set; }
        public IReadOnlyList<T> Items { get; set; }
        public long Page { get; set; }
        public long Size { get; set; }

        public PagedDto()
        {
        }
        public PagedDto(IReadOnlyList<T> items)
        {
            Total = items.Count;
            Items = items;
        }
        public PagedDto(IReadOnlyList<T> items, long total) : this(items)
        {
            Total = total;
        }
    }
}
