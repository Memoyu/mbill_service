namespace Memo.Bill.Application.Roles.Commands.Update;

[Authorize(Permissions = ApiPermission.Role.Update)]
public record UpdateRoleCommand(
    long RoleId,
    string Name,
    string Description,
    List<long> Permissions
    ) : IAuthorizeableRequest<Result>;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
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
    }
}
