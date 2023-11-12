using EasyCaching.Core;
using Mbill.Core.Domains.Common.Enums;
using Mbill.Core.Security.Dto;

namespace Mbill.Core.Security;

public class CurrentUser : ICurrentUser, ITransientDependency
{
    private static readonly Claim[] EmptyClaimsArray = new Claim[0];
    private readonly ILogger _logger;
    private readonly ClaimsPrincipal _claimsPrincipal;
    private readonly IEasyCachingProvider _cachingProvider;
    private readonly IFreeSql _fsql;

    public CurrentUser(
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor,
        IEasyCachingProvider cachingProvider,
        IFreeSql fsql)
    {
        _logger = loggerFactory.CreateLogger<CurrentUser>();
        _claimsPrincipal = httpContextAccessor.HttpContext?.User ?? Thread.CurrentPrincipal as ClaimsPrincipal;
        _cachingProvider = cachingProvider;
        _fsql = fsql;
    }

    // public long? Id => throw new Exception("不再使用Id");

    public long? BId => _claimsPrincipal?.FindUserBId();

    public string UserName => _claimsPrincipal?.FindUserName();
    public string Nickname => _claimsPrincipal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
    public string Email => _claimsPrincipal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
    public List<UserRoleCacheDto> Roles => GetRoles();

    public virtual Claim FindClaim(string claimType)
    {
        return _claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == claimType);
    }

    public virtual Claim[] FindClaims(string claimType)
    {
        return _claimsPrincipal?.Claims.Where(c => c.Type == claimType).ToArray() ?? EmptyClaimsArray;
    }

    public virtual Claim[] GetAllClaims()
    {
        return _claimsPrincipal?.Claims.ToArray() ?? EmptyClaimsArray;
    }

    public bool IsAdministrator()
    {
        var roles = GetRoles();
        return roles.Any(r => r.RoleType == RoleType.Administrator.GetHashCode());
    }

    private List<UserRoleCacheDto> GetRoles()
    {
        if (!BId.HasValue)
        {
            _logger.LogError($"用户：{UserName} 授权信息中BId为空");
            return new List<UserRoleCacheDto> { };
        }
        var cacheValue = _cachingProvider.Get<List<UserRoleCacheDto>>(CacheKeyConst.UserRole(BId.Value));
        var roles = cacheValue.Value;
        if (roles.Count <= 0)
        {
            _logger.LogError($"用户：{BId} 角色缓存数据为空");
            var userRoleOrm = _fsql.Select<UserRoleEntity>();
            var userRoles = userRoleOrm.Where(r => r.UserBId == BId).Include(r => r.Role).ToList() ?? [];
            var userRoleCaches = userRoles.Select(ur => new UserRoleCacheDto
            {
                RoleBId = ur.RoleBId,
                RoleType = ur.Role.Type,
            }).ToList();

            _cachingProvider.Set(CacheKeyConst.UserRole(BId.Value), userRoleCaches, TimeSpan.FromHours(6));

            if (userRoleCaches.Count <= 0)
                _logger.LogError($"用户：{BId} 角色缓存数据为空,尝试从数据库获取仍然为空");
            
            roles = userRoleCaches;

        }
        return [.. roles ?? []];
    }
}
