namespace Memo.Bill.Application.Roles.Commands;

[Authorize(Permissions = ApiPermission.Role.Create)]
public record CreateRoleCommand(
    string Name,
    string Description,
    List<long> Permissions
    ) : IAuthorizeableRequest<Result>;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage("角色名称不能为空");

        RuleFor(x => x.Name)
            .MinimumLength(1)
            .MaximumLength(10)
            .WithMessage("角色名称长度在1-10个字符之间");

        RuleFor(x => x.Description)
          .NotEmpty()
          .WithMessage("角色描述不能为空");

        RuleFor(x => x.Description)
            .MinimumLength(1)
            .MaximumLength(100)
            .WithMessage("角色描述长度在1-100个字符之间");


        RuleFor(x => x.Permissions)
          .Must(x => x != null && x.Count > 0)
          .WithMessage("角色权限不能为空");
    }
}

public class CreateRoleCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Role> roleRepo,
    IBaseDefaultRepository<Permission> permissionRepo,
    IBaseDefaultRepository<RolePermission> rolePermissionRepo
    ) : IRequestHandler<CreateRoleCommand, Result>
{
    public async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var exist = await roleRepo.Select.AnyAsync(r => r.Name == request.Name, cancellationToken);
        if (exist) throw new ApplicationException("同名角色已存在");

        var permission = await permissionRepo.Select.Where(p => request.Permissions.Contains(p.PermissionId)).ToListAsync(cancellationToken);
        foreach (var permissionId in request.Permissions)
        {
            if (!permission.Any(t => t.PermissionId == permissionId)) throw new ApplicationException($"{permissionId}权限不存在");
        }

        var role = mapper.Map<Role>(request);
        role = await roleRepo.InsertAsync(role, cancellationToken);
        if (role.Id <= 0) throw new ApplicationException("保存角色失败");

        var rolePermissions = request.Permissions.Select(p => new RolePermission { RoleId = role.RoleId, PermissionId = p }).ToList();
        await rolePermissionRepo.InsertAsync(rolePermissions, cancellationToken);

        return Result.Success(role.RoleId);
    }
}