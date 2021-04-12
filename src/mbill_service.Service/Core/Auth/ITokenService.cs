using mbill_service.Service.Core.Auth.Input;
using System.Threading.Tasks;

namespace mbill_service.Service.Core.Auth
{
    public interface ITokenService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        Task<TokenDto> LoginAsync(LoginDto loginDto);

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<TokenDto> GetTokenByRefreshAsync(string refreshToken);
    }
}
