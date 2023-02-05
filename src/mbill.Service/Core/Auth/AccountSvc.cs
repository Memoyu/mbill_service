using System.Linq;

namespace mbill.Service.Core.Auth;

public class AccountSvc : ApplicationSvc, IAccountSvc
{
    private readonly ILogger<AccountSvc> _logger;
    private readonly IUserRepo _userRepo;
    private readonly IUserIdentityRepo _userIdentityRepo;
    private readonly ICategoryRepo _categoryRepo;
    private readonly IAssetRepo _assetRepo;
    private readonly IUserIdentitySvc _userIdentityService;
    private readonly IJwtTokenSvc _jwtTokenService;
    private readonly IWxSvc _wxService;
    public AccountSvc(
        ILogger<AccountSvc> logger,
        IUserRepo userRepo,
        IUserIdentityRepo userIdentityRepo,
        ICategoryRepo categoryRepo,
        IAssetRepo assetRepo,
        IUserIdentitySvc userIdentityService,
        IJwtTokenSvc jwtTokenService,
        IWxSvc wxService)
    {
        _logger = logger;
        _userRepo = userRepo;
        _userIdentityRepo = userIdentityRepo;
        _categoryRepo = categoryRepo;
        _assetRepo = assetRepo;
        _userIdentityService = userIdentityService;
        _jwtTokenService = jwtTokenService;
        _wxService = wxService;
    }

    public async Task<ServiceResult<TokenDto>> AccountLoginAsync(AccountLoginDto loginDto)
    {
        _logger.LogInformation("User Use JwtLogin");

        UserEntity user = await _userRepo.GetUserAsync(r => r.Username == loginDto.Username || r.Email == loginDto.Username);

        if (user == null)
        {
            throw new KnownException("用户不存在", ServiceResultCode.NotFound);
        }

        bool valid = await _userIdentityService.VerifyUserPasswordAsync(user.Id, loginDto.Password);

        if (!valid)
        {
            throw new KnownException("请输入正确密码", ServiceResultCode.ParameterError);
        }

        _logger.LogInformation($"用户{loginDto.Username},登录成功");
        return ServiceResult<TokenDto>.Successed(await _jwtTokenService.CreateTokenAsync(user));
    }

    public async Task<ServiceResult<PreLoginUserDto>> WxPreLoginAsync(string code)
    {
        var wxlogin = await _wxService.GetCode2Session(code);
        if (!wxlogin.Success || wxlogin.Result == null)
            return ServiceResult<PreLoginUserDto>.Failed($"微信登录失败，请稍后重试！错误：{wxlogin.Message}");
        var openId = wxlogin.Result.OpenId;
        var identity = await _userIdentityService.VerifyWxOpenIdAsync(openId);
        var user = new UserEntity();
        var isExist = 0;
        // 如果绑定信息为空，则未登录过，进行账户信息记录
        if (identity == null)
        {
            var entity = new UserEntity
            {
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
            entity.UserRoles = new List<UserRoleEntity>
            {
                new UserRoleEntity()
                {
                    RoleId = Role.User
                }
            };

            entity.UserIdentitys = new List<UserIdentityEntity>()//构建赋值用户身份认证登录信息
            {
                new UserIdentityEntity(UserIdentityEntity.WeiXin,entity.Nickname,openId,DateTime.Now)
            };
            entity = await _userRepo.InsertAsync(entity);
            if (entity == null)
                return ServiceResult<PreLoginUserDto>.Failed($"微信登录失败，请联系开发者！");
            _ = InitUserDataAsync(entity.Id);
            user = await _userRepo.GetUserAsync(c => c.Id == entity.Id);
        }
        else
        {
            user = await _userRepo.GetUserAsync(c => c.Id == identity.UserId);
            isExist = 1;
        }

        var userDto = Mapper.Map<PreLoginUserDto>(user);
        userDto.OpenId = openId;
        userDto.IsExist = isExist;
        return ServiceResult<PreLoginUserDto>.Successed(userDto);
    }

    [Transactional]
    public async Task<ServiceResult<TokenWithUserDto>> WxLoginAsync(WxLoginInput input)
    {
        var openId = input.OpenId;
        var identity = await _userIdentityService.VerifyWxOpenIdAsync(openId);
        var user = new UserEntity();
        // 如果绑定信息为空，则未登录过，进行账户信息记录
        if (identity == null)
            return ServiceResult<TokenWithUserDto>.Failed($"微信用户不存在，请重新授权！");

        user = await _userRepo.GetUserAsync(c => c.Id == identity.UserId);

        user.AvatarUrl = input.AvatarUrl;
        user.Nickname = input.Nickname;
        identity.Identifier = input.Nickname;
        await _userIdentityRepo.UpdateAsync(identity);
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
    /// <param name="id">用户Id</param>
    /// <returns></returns>
    private async Task InitUserDataAsync(long id)
    {
        try
        {
            // 初始化账单分类、资产数据
            // 获取userid = 1 的用户所有相关数据
            var categories = await _categoryRepo.Select.Where(c => c.CreateUserId == 1).ToListAsync();
            var assets = await _assetRepo.Select.Where(c => c.CreateUserId == 1).ToListAsync();

            // 获取父分类、子类分组
            var parentCategories = categories.Where(c => c.Type == 1 && c.ParentId == 0).ToList();
            parentCategories.AddRange(categories.Where(c => c.Type == 0 && c.ParentId == 0).Take(4).ToList());
            var groupsCategories = parentCategories.Select(c =>
              {
                  return new
                  {
                      Id = 0,
                      Name = c.Name,
                      Type = c.Type,
                      Sort = c.Sort,
                      Childs = categories.FindAll(d => d.ParentId == c.Id).ToList()
                  };
              }).ToList();

            var parentAssets = assets.Where(c => c.ParentId == 0).ToList();
            var groupsAssets = parentAssets.Select(c =>
            {
                return new
                {
                    Id = 0,
                    Name = c.Name,
                    Type = c.Type,
                    Sort = c.Sort,
                    Childs = assets.FindAll(d => d.ParentId == c.Id).ToList()
                };
            }).ToList();

            // 构造父类实体
            var categoryEntities = groupsCategories.Select(c => new CategoryEntity
            {
                Name = c.Name,
                Type = c.Type,
                Sort = c.Sort,
                Icon = "",
                CreateUserId = id
            }).ToList();

            var assetEntities = groupsAssets.Select(c => new AssetEntity
            {
                Name = c.Name,
                Type = c.Type,
                Sort = c.Sort,
                Icon = "",
                CreateUserId = id
            }).ToList();

            // 插入父类数据
            var userParentCategories = await _categoryRepo.InsertAsync(categoryEntities);
            var userParentAssets = await _assetRepo.InsertAsync(assetEntities);

            // 构造子项实体
            categoryEntities = new List<CategoryEntity>();
            assetEntities = new List<AssetEntity>();
            foreach (var g in groupsCategories)
            {
                var p = userParentCategories.FirstOrDefault(c => c.Name == g.Name)?.Id;
                if (p == null) continue;
                foreach (var item in g.Childs)
                {
                    item.Id = 0;
                    item.ParentId = p.Value;
                    item.CreateUserId = id;
                    categoryEntities.Add(item);
                }
            }

            foreach (var g in groupsAssets)
            {
                var p = userParentAssets.FirstOrDefault(c => c.Name == g.Name)?.Id;
                if (p == null) continue;
                foreach (var item in g.Childs)
                {
                    item.Id = 0;
                    item.ParentId = p.Value;
                    item.CreateUserId = id;
                    assetEntities.Add(item);
                }
            }

            _ = await _categoryRepo.InsertAsync(categoryEntities);
            _ = await _assetRepo.InsertAsync(assetEntities);

        }
        catch (Exception ex)
        {
            throw;
        }

    }

    #endregion


}
