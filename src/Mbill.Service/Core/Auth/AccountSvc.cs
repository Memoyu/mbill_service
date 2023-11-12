using Mbill.Core.Common;
using Mbill.Service.Bill.Asset;
using Mbill.Service.Core.DataSeed.Output;
using System.Linq;

namespace Mbill.Service.Core.Auth;

public class AccountSvc : ApplicationSvc, IAccountSvc
{
    private readonly ILogger<AccountSvc> _logger;
    private readonly IUserRepo _userRepo;
    private readonly IRoleRepo _roleRepo;
    private readonly IUserRoleRepo _userRoleRepo;
    private readonly IUserIdentityRepo _userIdentityRepo;
    private readonly ICategoryRepo _categoryRepo;
    private readonly IAssetRepo _assetRepo;
    private readonly IUserIdentitySvc _userIdentityService;
    private readonly IJwtTokenSvc _jwtTokenService;
    private readonly IWxSvc _wxService;
    private readonly IDataSeedRepo _dataSeedRepo;

    public AccountSvc(
        ILogger<AccountSvc> logger,
        IUserRepo userRepo,
        IRoleRepo roleRepo,
        IUserRoleRepo userRoleRepo,
        IUserIdentityRepo userIdentityRepo,
        ICategoryRepo categoryRepo,
        IAssetRepo assetRepo,
        IUserIdentitySvc userIdentityService,
        IJwtTokenSvc jwtTokenService,
        IWxSvc wxService,
        IDataSeedRepo dataSeedRepo)
    {
        _logger = logger;
        _userRepo = userRepo;
        _roleRepo = roleRepo;
        _userRoleRepo = userRoleRepo;
        _userIdentityRepo = userIdentityRepo;
        _categoryRepo = categoryRepo;
        _assetRepo = assetRepo;
        _userIdentityService = userIdentityService;
        _jwtTokenService = jwtTokenService;
        _wxService = wxService;
        _dataSeedRepo = dataSeedRepo;
    }

    public async Task<ServiceResult<TokenDto>> AccountLoginAsync(AccountLoginDto loginDto)
    {
        _logger.LogInformation("User Use JwtLogin");

        UserEntity user = await _userRepo.GetUserAsync(r => r.Username == loginDto.Username || r.Email == loginDto.Username);

        if (user == null)
        {
            throw new KnownException("用户不存在", ServiceResultCode.NotFound);
        }

        bool valid = await _userIdentityService.VerifyUserPasswordAsync(user.BId, loginDto.Password);

        if (!valid)
        {
            throw new KnownException("请输入正确密码", ServiceResultCode.ParameterError);
        }

        _logger.LogInformation($"用户{loginDto.Username},登录成功");
        return ServiceResult<TokenDto>.Successed(await _jwtTokenService.CreateTokenAsync(user));
    }

    [Transactional]
    public async Task<ServiceResult<PreLoginUserDto>> WxPreLoginAsync(string code)
    {
        var wxlogin = await _wxService.GetCode2Session(code);
        if (!wxlogin.Success || wxlogin.Result == null)
            return ServiceResult<PreLoginUserDto>.Failed($"微信登录失败，请稍后重试！错误：{wxlogin.Message}");
        var openId = wxlogin.Result.OpenId;
        var identity = await _userIdentityService.VerifyWxOpenIdAsync(openId);
        var user = new UserEntity();
        var isCompletedInfo = 0;
        // 如果绑定信息为空，则未登录过，进行账户信息记录
        if (identity == null)
        {
            var entity = new UserEntity
            {
                BId = SnowFlake.NextId(),
                Username = string.Empty,
                Nickname = string.Empty,
                Gender = 0,
                Email = string.Empty,
                Phone = string.Empty,
                Province = string.Empty,
                City = string.Empty,
                District = string.Empty,
                Street = string.Empty,
                AvatarUrl = string.Empty,
            };

            var role = await _roleRepo.Select.Where(r => r.Type == RoleType.User.GetHashCode()).FirstAsync();
            if (role is null) throw new KnownException("普通用户角色为空，请联系管理员添加！");
            var userRoles = new List<UserRoleEntity>
            {
                new UserRoleEntity()
                {
                    BId= SnowFlake.NextId(),
                    UserBId = entity.BId,
                    RoleBId = role.BId,
                }
            };

            var userIdentitys = new List<UserIdentityEntity>()//构建赋值用户身份认证登录信息
            {
                new(entity.BId, UserIdentityEntity.WeiXin, entity.Nickname, openId, DateTime.Now)
            };

            // 插入数据
            entity = await _userRepo.InsertAsync(entity);
            await _userIdentityRepo.InsertAsync(userIdentitys);
            await _userRoleRepo.InsertAsync(userRoles);

            if (entity == null)
                return ServiceResult<PreLoginUserDto>.Failed($"微信登录失败，请联系开发者！");

            user = await _userRepo.GetUserAsync(entity.BId);
        }
        else
        {
            user = await _userRepo.GetUserAsync(identity.UserBId);
            if (!string.IsNullOrWhiteSpace(user.AvatarUrl) && !string.IsNullOrWhiteSpace(user.Nickname))
                isCompletedInfo = 1;
        }

        var userDto = Mapper.Map<PreLoginUserDto>(user);
        userDto.OpenId = openId;
        userDto.IsCompletedInfo = isCompletedInfo;
        return ServiceResult<PreLoginUserDto>.Successed(userDto);
    }

    [Transactional]
    public async Task<ServiceResult<TokenWithUserDto>> WxLoginAsync(WxLoginInput input)
    {
        var openId = input.OpenId;
        var identity = await _userIdentityService.VerifyWxOpenIdAsync(openId);
        // 如果绑定信息为空，则未登录过，进行账户信息记录
        if (identity == null)
            return ServiceResult<TokenWithUserDto>.Failed($"微信用户不存在，请重新授权！");

        var user = await _userRepo.GetUserAsync(identity.UserBId);

        user.AvatarUrl = input.AvatarUrl;
        user.Nickname = input.Nickname;
        identity.Identifier = input.Nickname;
        await _userIdentityRepo.UpdateAsync(identity);

        // 没有初始化则进行数据初始化
        if (!user.IsInit)
            await InitUserDataAsync(user.BId);

        user.IsInit = true;
        await _userRepo.UpdateAsync(user);

        var token = await _jwtTokenService.CreateTokenAsync(user);
        var userDto = Mapper.Map<LoginUserDto>(user);
        return ServiceResult<TokenWithUserDto>.Successed(new TokenWithUserDto(token, userDto));
    }

    public async Task<ServiceResult<TokenDto>> GetTokenByRefreshAsync(string refreshToken)
    {
        return ServiceResult<TokenDto>.Successed(await _jwtTokenService.RefreshTokenAsync(refreshToken));
    }

    #region Private

    /// <summary>
    /// 初始化新用户数据
    /// </summary>
    /// <param name="userBId">用户BId</param>
    /// <returns></returns>
    private async Task InitUserDataAsync(long userBId)
    {
        try
        {
            // 获取分类种子数据
            var categorySeed = await _dataSeedRepo.Select.Where(ds => ds.Type == DataSeedType.BillCategory.GetHashCode()).ToOneAsync();
            var assetSeed = await _dataSeedRepo.Select.Where(ds => ds.Type == DataSeedType.AssetCategory.GetHashCode()).ToOneAsync();

            var categorySeeds = JsonConvert.DeserializeObject<List<BillCategoryDataSeedDto>>(categorySeed?.Data ?? string.Empty) ?? new List<BillCategoryDataSeedDto>();
            var assetSeeds = JsonConvert.DeserializeObject<List<BillAssetDataSeedDto>>(assetSeed?.Data ?? string.Empty) ?? new List<BillAssetDataSeedDto>();

            // 初始化用户账单分类数据
            if (categorySeeds.Count != 0)
            {
                var userCategories = await _categoryRepo.Select.Where(c => c.CreateUserBId == userBId).DisableGlobalFilter("IsDeleted").ToListAsync();
                var categories = new List<CategoryEntity>();
                foreach (var category in categorySeeds)
                {
                    var exist = userCategories.FirstOrDefault(c => c.Name.Trim() == category.Name.Trim());
                    var parentBId = SnowFlake.NextId();
                    if (exist is null)
                    {
                        categories.Add(category.ToEntity(parentBId, null, userBId));
                    }
                    else
                    {
                        parentBId = exist.BId;
                    }

                    foreach (var child in category.Childs)
                    {
                        var childExist = userCategories.FirstOrDefault(c => c.Name.Trim() == child.Name.Trim());
                        if (childExist is null)
                            categories.Add(child.ToEntity(null, parentBId, userBId));
                    }
                }

                if (categories.Count > 0)
                    await _categoryRepo.InsertAsync(categories);
            }

            // 初始化用户资产分类数据源
            if (assetSeeds.Count != 0)
            {
                var userAssets = await _assetRepo.Select.Where(c => c.CreateUserBId == userBId).DisableGlobalFilter("IsDeleted").ToListAsync();
                var assets = new List<AssetEntity>();
                foreach (var asset in assetSeeds)
                {
                    var exist = userAssets.FirstOrDefault(c => c.Name.Trim() == asset.Name.Trim());
                    var parentBId = SnowFlake.NextId();
                    if (exist is null)
                    {
                        assets.Add(asset.ToEntity(parentBId, null, userBId));
                    }
                    else
                    {
                        parentBId = exist.BId;
                    }

                    foreach (var child in asset.Childs)
                    {
                        var childExist = userAssets.FirstOrDefault(c => c.Name.Trim() == child.Name.Trim());
                        if (childExist is null)
                            assets.Add(child.ToEntity(null, parentBId, userBId));
                    }
                }

                if (assets.Count > 0)
                    await _assetRepo.InsertAsync(assets);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"写入用户:{userBId}初始化数据失败");
        }

    }

    #endregion


}
