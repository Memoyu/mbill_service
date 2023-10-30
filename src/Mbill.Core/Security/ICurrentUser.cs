namespace Mbill.Core.Security;

public interface ICurrentUser
{
    // long? Id { get; }

    long? BId { get; }

    string UserName { get; }
    long[] Roles { get; }


    Claim FindClaim(string claimType);

    Claim[] FindClaims(string claimType);

    Claim[] GetAllClaims();


    bool IsInGroup(long groupId);
}
