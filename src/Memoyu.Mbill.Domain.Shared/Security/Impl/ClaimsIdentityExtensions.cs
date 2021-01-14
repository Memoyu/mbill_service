/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Shared.Security.Impl
*   文件名称 ：ClaimsIdentityExtensions.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 16:20:31
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System.Linq;
using System.Security.Claims;

namespace Memoyu.Mbill.Domain.Shared.Security.Impl
{
    public static class ClaimsIdentityExtensions
    {
        public static int? FindUserId(this ClaimsPrincipal principal)
        {
            Claim userIdOrNull = principal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdOrNull == null || string.IsNullOrWhiteSpace(userIdOrNull.Value))
            {
                return null;
            }

            return int.Parse(userIdOrNull.Value);
        }

        public static bool? IsAdmin(this ClaimsPrincipal principal)
        {
            Claim isAdminOrNull = principal.Claims?.FirstOrDefault(c => c.Type == CoreClaimTypes.IsAdmin);
            if (isAdminOrNull == null || string.IsNullOrWhiteSpace(isAdminOrNull.Value))
            {
                return null;
            }
            return bool.Parse(isAdminOrNull.Value);
        }

        public static string FindUserName(this ClaimsPrincipal principal)
        {
            Claim userNameOrNull = principal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            return userNameOrNull?.Value;
        }

    }
}
