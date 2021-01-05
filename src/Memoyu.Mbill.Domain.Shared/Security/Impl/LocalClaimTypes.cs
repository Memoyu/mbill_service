/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Shared.Security.Impl
*   文件名称 ：LocalClaimTypes.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 16:21:19
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/

namespace Memoyu.Mbill.Domain.Shared.Security.Impl
{
    public static class LocalClaimTypes
    {
        public static string Groups { get; set; } = "LinCmsClaimTypes.Groups";
        public static string IsActive { get; set; } = "LinCmsClaimTypes.IsActive";
        public static string IsAdmin { get; set; } = "LinCmsClaimTypes.IsAdmin";
    }
}
