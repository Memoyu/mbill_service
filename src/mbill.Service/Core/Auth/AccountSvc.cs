namespace mbill.Service.Core.Auth;

public class AccountSvc : ApplicationSvc, IAccountSvc
{
    private readonly ILogger<AccountSvc> _logger;
    private readonly IUserRepo _userRepo;
    private readonly IUserIdentitySvc _userIdentityService;
    private readonly IJwtTokenSvc _jwtTokenService;
    private readonly IWxSvc _wxService;
    public AccountSvc(
        ILogger<AccountSvc> logger,
        IUserRepo userRepo,
        IUserIdentitySvc userIdentityService,
        IJwtTokenSvc jwtTokenService,
        IWxSvc wxService)
    {
        _logger = logger;
        _userRepo = userRepo;
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

    public async Task<ServiceResult<TokenWithUserDto>> WxLoginAsync(WxLoginInput input)
    {
        var wxlogin = await _wxService.GetCode2Session(input.Code);
        if (!wxlogin.Success || wxlogin.Result == null)
            return ServiceResult<TokenWithUserDto>.Failed($"微信登录失败，请稍后重试！错误：{wxlogin.Message}");
        var openId = wxlogin.Result.OpenId;
        var exist = await _userIdentityService.VerifyWxOpenIdAsync(openId);
        var user = new UserEntity();
        // 如果绑定信息为空，则未登录过，进行账户信息记录
        if (exist == null)
        {
            var entity = new UserEntity
            {
                Username = "",
                Nickname = input.Nickname,
                Gender = input.Gender,
                Email = "",
                Phone = "",
                Province = input.Province,
                City = input.City,
                District = "",
                Street = "",
                AvatarUrl = input.AvatarUrl,
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
                    new UserIdentityEntity(UserIdentityEntity.WeiXin,input.Nickname,openId,DateTime.Now)
                };
            await _userRepo.InsertAsync(entity);
            user = await _userRepo.GetUserAsync(c => c.Id == entity.Id);
        }
        else
        {
            user = await _userRepo.GetUserAsync(c => c.Id == exist.UserId);
            // 在没有实现前台用户信息更改功能前，需要替换一下登录人的微信登录信息
            user.AvatarUrl = input.AvatarUrl;
            user.Nickname = input.Nickname;
            await _userRepo.Orm.Update<UserEntity>().SetSource(user).UpdateColumns(e => new { e.AvatarUrl, e.Nickname }).ExecuteAffrowsAsync();
        }


        var token = await _jwtTokenService.CreateTokenAsync(user);
        var userDto = Mapper.Map<UserSimpleDto>(user);
        userDto.Days = (DateTime.Now - user.CreateTime).Days;
        return ServiceResult<TokenWithUserDto>.Successed(new TokenWithUserDto(token, userDto));
    }

    public async Task<ServiceResult<TokenDto>> GetTokenByRefreshAsync(string refreshToken)
    {
        return ServiceResult<TokenDto>.Successed(await _jwtTokenService.RefreshTokenAsync(refreshToken));
    }

}
