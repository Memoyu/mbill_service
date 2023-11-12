using Mbill.Core.Security.Dto;

namespace Mbill.Core.Security;

public interface ICurrentUser
{
    // long? Id { get; }

    long? BId { get; }

    string UserName { get; }
    List<UserRoleCacheDto> Roles { get; }


    Claim FindClaim(string claimType);

    Claim[] FindClaims(string claimType);

    Claim[] GetAllClaims();

    /// <summary>
    /// 是否超级管理员
    /// </summary>
    /// <returns></returns>
    bool IsAdministrator();
}
