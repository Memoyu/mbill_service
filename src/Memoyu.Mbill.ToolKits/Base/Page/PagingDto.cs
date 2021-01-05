/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.ToolKits.Base.Page
*   文件名称 ：PagingDto.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-02 12:09:49
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System;
using System.ComponentModel.DataAnnotations;

namespace Memoyu.Mbill.ToolKits.Base.Page
{

    /// <summary>
    /// 分页加排序
    /// </summary>
    public class PagedAndSortedRequestDto : PagingDto, ISortedResultRequest
    {
        public string Sorting { get; set; }
    }

    /// <summary>
    /// 常规分页
    /// </summary>
    public class PagingDto : IPagingDto
    {
        /// <summary>
        /// 每页个数
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "每页个数最小为1")]
        public int Size { get; set; } = 15;
        /// <summary>
        /// 从0开始，0时取第1页，1时取第二页
        /// </summary>
        public int Page { get; set; } = 0;

        public string Sort { get; set; }
    }


    public interface IPagingDto : ILimitedResultRequest
    {
        int Page { get; set; }
    }

    public interface ILimitedResultRequest
    {
        int Size { get; set; }
    }

    public interface ISortedResultRequest
    {
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <example>
        /// 例子:
        /// "Name"
        /// "Name DESC"
        /// "Name ASC, Age DESC"
        /// </example>
        string Sorting { get; set; }
    }

}
