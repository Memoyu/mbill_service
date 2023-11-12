namespace Mbill.Core.Domains.Common.Consts;

public class CacheKeyConst
{
    public static string UserRole(long userBId) =>  $"user:role:{userBId}";
}
