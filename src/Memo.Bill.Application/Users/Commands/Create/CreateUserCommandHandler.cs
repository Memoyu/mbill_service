using Memo.Bill.Application.Users.Common;
using Memo.Bill.Domain.Enums;

namespace Memo.Bill.Application.Users.Commands.Create;

public class CreateUserCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<User> userRepo,
    IBaseDefaultRepository<UserIdentity> userIdentityRepo,
    IBaseDefaultRepository<UserRole> userRoleRepo,
    IBaseDefaultRepository<Role> roleRepo
    ) : IRequestHandler<CreateUserCommand, Result>
{
    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var exist = await userRepo.Select.AnyAsync(u => request.Username == u.Username, cancellationToken);
        if (exist) throw new ApplicationException("同名用户已存在");

        var roles = await roleRepo.Select.Where(r => request.Roles.Contains(r.RoleId)).ToListAsync(cancellationToken);
        foreach (var roleId in request.Roles)
        {
            if (!roles.Any(r => r.RoleId == roleId)) throw new ApplicationException($"{roleId}角色不存在");
        }

        // 用户信息
        var user = mapper.Map<User>(request);
        user = await userRepo.InsertAsync(user, cancellationToken);
        if (user.Id <= 0) throw new ApplicationException("保存用户失败");

        // 用户身份认
        var identity = new UserIdentity
        {
            UserId = user.UserId,
            IdentityType = request.UserIdentityType,
            Identifier = string.IsNullOrWhiteSpace(request.Email) ? request.PhoneNumber! : request.Email!,
            Credential = request.UserIdentityType == UserIdentityType.Password ? EncryptUtil.Encrypt(request.Credential) : request.Credential,
        };
        identity = await userIdentityRepo.InsertAsync(identity, cancellationToken);
        if (identity.Id <= 0) throw new ApplicationException("保存用户身份认证失败");

        // 用户角色
        var userRoles = new List<UserRole>();
        request.Roles.ForEach(id => userRoles.Add(new UserRole
        {
            UserId = user.UserId,
            RoleId = id
        }));
        userRoles = await userRoleRepo.InsertAsync(userRoles, cancellationToken);
        if (userRoles.Any(ur => ur.Id <= 0)) throw new ApplicationException("保存用户角色失败"); 

        var result = mapper.Map<UserResult>(user);

        return Result.Success(result);
    }
}
