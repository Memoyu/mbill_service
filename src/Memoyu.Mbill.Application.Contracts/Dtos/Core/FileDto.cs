/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Dtos.Core
*   文件名称 ：FileDto.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-23 19:01:19
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/

using Memoyu.Mbill.Application.Contracts.Dtos.Base;

namespace Memoyu.Mbill.Application.Contracts.Dtos.Core
{
    public class FileDto : EntityDto
    {
        public string Key { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
    }
}
