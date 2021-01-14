/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Dtos.Core
*   文件名称 ：ModifyPermissionDto.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-14 22:53:10
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/

namespace Memoyu.Mbill.Application.Contracts.Dtos.Core
{
    public class ModifyPermissionDto
    {
        public string Name { get; set; }
        public string Module { get; set; }
        public string Router { get; set; }
    }
}
