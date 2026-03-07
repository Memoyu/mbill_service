namespace Memo.Bill.Application.Roles.Commands.Create;

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
