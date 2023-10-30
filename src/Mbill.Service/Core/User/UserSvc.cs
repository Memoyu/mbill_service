using Mapster;
using Mbill.Core.Common;
using Mbill.Service.Core.User.Output;

namespace Mbill.Service.Core.User;

public class UserSvc : ApplicationSvc, IUserSvc
{
    private readonly IUserRepo _userRepo;
    private readonly IUserRoleRepo _userRoleRepo;
    private readonly IUserIdentityRepo _userIdentityRepo;
    private readonly IFileRepo _fileRepo;

    public UserSvc(IUserRepo userRepo,
        IUserRoleRepo userRoleRepo,
        IUserIdentityRepo userIdentityRepo,
        IFileRepo fileRepo)
    {
        _userRepo = userRepo;
        _userRoleRepo = userRoleRepo;
        _userIdentityRepo = userIdentityRepo;
        _fileRepo = fileRepo;
    }

    [Transactional]
    public async Task CreateAsync(ModifyUserDto input)
    {
        var user = Mapper.Map<UserEntity>(input);

        var roleBIds = input.RoleBIds;
        var password = input.Password;

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

        user.BId = SnowFlake.NextId();
        var userRoles = new List<UserRoleEntity>();
        roleBIds?.ForEach(roleBId =>//遍历构建赋值角色
        {
            userRoles.Add(new UserRoleEntity()
            {
                BId = user.BId,
                RoleBId = roleBId
            });
        });

        var userIdentitys = new List<UserIdentityEntity>()//构建赋值用户身份认证登录信息
        {
            new UserIdentityEntity(user.BId, UserIdentityEntity.Password,user.Username,EncryptUtil.Encrypt(password), DateTime.Now)
        };
        await _userRepo.InsertAsync(user);
        await _userIdentityRepo.InsertAsync(userIdentitys);
        await _userRoleRepo.InsertAsync(userRoles);
    }

    public async Task<ServiceResult<UserDto>> GetAsync(long? bId)
    {
        var userBId = bId ?? CurrentUser.BId;
        var user = await _userRepo.GetUserAsync(userBId.Value);
        var roles = await _userRoleRepo.Where(r => r.UserBId == user.BId).Include(r => r.Role).ToListAsync(r => r.Role);

        user.AvatarUrl = _fileRepo.GetFileUrl(user.AvatarUrl);
        var dto = Mapper.Map<UserDto>(user);
        dto.Roles = roles.Adapt<List<RoleDto>>();

        return ServiceResult<UserDto>.Successed(dto);
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
            .IncludeMany(u => u.UserRoles)
            .WhereIf(!string.IsNullOrWhiteSpace(pagingDto.Username), u => u.Username.Contains(pagingDto.Username))
            .WhereIf(!string.IsNullOrWhiteSpace(pagingDto.Nickname), u => u.Nickname.Contains(pagingDto.Nickname))
            .WhereIf(isEnable != null, u => u.IsEnable == isEnable)
            .WhereIf(pagingDto.CreateStartTime != null, a => a.CreateTime >= pagingDto.CreateStartTime && a.CreateTime <= pagingDto.CreateEndTime)
            .WhereIf(pagingDto.RoleBId > 0, u => u.UserRoles.AsSelect().Any(r => r.RoleBId == pagingDto.RoleBId))
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
        var entity = await _userRepo.GetUserAsync(input.BId);
        if (entity == null) return ServiceResult.Failed("用户不存在");
        input.Adapt(entity);
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