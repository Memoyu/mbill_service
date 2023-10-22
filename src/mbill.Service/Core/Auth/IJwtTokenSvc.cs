namespace Mbill.Service.Core.Auth;

public interface IJwtTokenSvc
{
    /// <summary>
    /// 创建Token和RefreshToken
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<TokenDto> CreateTokenAsync(UserEntity user);

    /// <summary>
    /// 刷新token
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    Task<TokenDto> RefreshTokenAsync(string refreshToken);
}
