/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Core.Account
*   文件名称 ：ITokenService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-10 15:13:28
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Application.Contracts.Dtos.Core;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Core
{
    public interface ITokenService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginInputDto"></param>
        /// <returns></returns>
        Task<TokenDto> LoginAsync(LoginInputDto loginInputDto);

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<TokenDto> GetTokenByRefreshAsync(string refreshToken);
    }
}
