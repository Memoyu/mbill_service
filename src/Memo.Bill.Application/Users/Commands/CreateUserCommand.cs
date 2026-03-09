using Memo.Bill.Application.Users.Common;

namespace Memo.Bill.Application.Users.Commands;

[Authorize(Permissions = ApiPermission.User.Create)]
[Transactional]
public record CreateUserCommand(
    string Username,
    string Nickname,
    UserIdentityType UserIdentityType,
    string Credential,
    string? Avatar,
    string? PhoneNumber,
    string? Email,
    List<long> Roles
    ) : IAuthorizeableRequest<Result>;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .MinimumLength(1)
            .MaximumLength(20)
            .WithMessage("用户名称长度在1-20个字符之间");

        RuleFor(x => x.Nickname)
            .MinimumLength(1)
            .MaximumLength(20)
            .WithMessage("用户昵称长度在1-20个字符之间");

        RuleFor(x => x.UserIdentityType)
            .IsInEnum()
            .WithMessage("用户认证方式错误");

        RuleFor(x => x.Credential)
          .Matches("^[A-Za-z0-9_*&$#@]{4,20}$")
          .WithMessage("密码长度必须在6~22位之间，包含字符、数字和 _")
          .When(x => !string.IsNullOrWhiteSpace(x.Credential) && x.UserIdentityType == UserIdentityType.Password);

        RuleFor(x => x)
          .Must(x => !string.IsNullOrWhiteSpace(x.Email) || !string.IsNullOrWhiteSpace(x.PhoneNumber))
          .WithMessage("认证方式不为密码时，需要填入邮箱或电话")
          .When(x => x.UserIdentityType != UserIdentityType.Password);

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("邮箱格式有误")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Roles)
            .NotEmpty()
            .WithMessage("角色不能为空");
    }
}

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
