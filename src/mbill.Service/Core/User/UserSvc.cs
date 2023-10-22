using Mbill.Service.Core.User.Output;

namespace Mbill.Service.Core.User;

public class UserSvc : ApplicationSvc, IUserSvc
{
    private readonly IUserRepo _userRepo;
    private readonly IFileRepo _fileRepo;

    public UserSvc(IUserRepo userRepo, IFileRepo fileRepo)
    {
        _userRepo = userRepo;
        _fileRepo = fileRepo;
    }

    [Transactional]
    public async Task CreateAsync(UserEntity user, List<long> roleIds, string password)
    {
        if (!string.IsNullOrEmpty(user.Username))
        {
            bool isRepeatName = await _userRepo.Select.AnyAsync(r => r.Username == user.Username);
            if (isRepeatName)//用户名重复
            {
                throw new KnownException("用户名重复，请重新输入", ServiceResultCode.RepeatField);
            }
        }

        if (!string.IsNullOrEmpty(user.Email.Trim()))
        {
            var isRepeatEmail = await _userRepo.Select.AnyAsync(r => r.Email == user.Email.Trim());
            if (isRepeatEmail)//邮箱重复
            {
                throw new KnownException("注册邮箱重复，请重新输入", ServiceResultCode.RepeatField);
            }
        }
        user.UserRoles = new List<UserRoleEntity>();
        roleIds?.ForEach(roleId =>//遍历构建赋值角色
        {
            user.UserRoles.Add(new UserRoleEntity()
            {
                RoleId = roleId
            });
        });

        user.UserIdentitys = new List<UserIdentityEntity>()//构建赋值用户身份认证登录信息
            {
                new UserIdentityEntity(UserIdentityEntity.Password,user.Username,EncryptUtil.Encrypt(password), DateTime.Now)
            };
        await _userRepo.InsertAsync(user);
    }

    public async Task<ServiceResult<UserDto>> GetAsync(long? id)
    {
        var userId = id ?? CurrentUser.Id;
        var user = await _userRepo
            .Select
            .IncludeMany(u => u.Roles)
            .Where(r => r.Id == userId).FirstAsync();
        user.AvatarUrl = _fileRepo.GetFileUrl(user.AvatarUrl);
        return ServiceResult<UserDto>.Successed(Mapper.Map<UserDto>(user));
    }

    public async Task<ServiceResult<PagedDto<UserDto>>> GetPagesAsync(UserPagingDto pagingDto)
    {
        if (pagingDto.CreateStartTime != null && pagingDto.CreateEndTime == null) throw new KnownException("创建时间参数有误", ServiceResultCode.ParameterError);
        pagingDto.Sort = pagingDto.Sort.IsNullOrEmpty() ? "id ASC" : pagingDto.Sort.Replace("-", " ");
        bool? isEnable = pagingDto.IsEnable switch
        {
            1 => true,
            0 => false,
            _ => null
        };
        var users = await _userRepo
            .Select
            .IncludeMany(u => u.Roles)
            .WhereIf(!string.IsNullOrWhiteSpace(pagingDto.Username), u => u.Username.Contains(pagingDto.Username))
            .WhereIf(!string.IsNullOrWhiteSpace(pagingDto.Nickname), u => u.Nickname.Contains(pagingDto.Nickname))
            .WhereIf(isEnable != null, u => u.IsEnable == isEnable)
            .WhereIf(pagingDto.CreateStartTime != null, a => a.CreateTime >= pagingDto.CreateStartTime && a.CreateTime <= pagingDto.CreateEndTime)
            .WhereIf(pagingDto.RoleId > 0, u => u.Roles.AsSelect().Any(r => r.Id == pagingDto.RoleId))
            .OrderBy(pagingDto.Sort)
            .ToPageListAsync(pagingDto, out long totalCount);
        var userDtos = users.Select(u =>
         {
             var dto = Mapper.Map<UserDto>(u);
             dto.AvatarUrl = _fileRepo.GetFileUrl(dto.AvatarUrl);
             return dto;
         }).ToList();

        return ServiceResult<PagedDto<UserDto>>.Successed(new PagedDto<UserDto>(userDtos, totalCount));
    }

    public async Task<ServiceResult> UpdateAsync(ModifyUserBaseDto input)
    {
        var entity = await _userRepo.GetAsync(input.Id);
        if (entity == null) return ServiceResult.Failed("用户不存在");
        entity.AvatarUrl = input.AvatarUrl;
        entity.Nickname = input.Nickname;
        entity.Gender = input.Gender;
        entity.Email = input.Email;
        entity.Phone = input.Phone;
        var rows = await _userRepo.UpdateAsync(entity);
        if (rows <= 0)
            return ServiceResult.Failed("更新用户失败");
        return ServiceResult.Successed();
    }

    public Task<ServiceResult> DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }
}