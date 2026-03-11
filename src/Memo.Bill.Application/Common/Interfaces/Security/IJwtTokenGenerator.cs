using Memo.Bill.Application.Common.Security;

namespace Memo.Bill.Application.Common.Interfaces.Security;

public interface IJwtTokenGenerator
{
    /// <summary>
    /// 生成JWT Token
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns></returns>
    Task<JwtTokenDto> GenerateTokenAsync(User user, CancellationToken cancellationToken);

    /// <summary>
    /// 刷新JWT Token
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns></returns>
    Task<JwtTokenDto> RefreshTokenAsync(User user, CancellationToken cancellationToken);
}
