namespace Memo.Bill.Application.Roles.Commands.Delete;

[Authorize(Permissions = ApiPermission.Role.Delete)]
public record DeleteRoleCommand(long RoleId) : IAuthorizeableRequest<Result>;

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .Must(x => x > 0)
            .WithMessage("角色Id必须大于0");
    }
}
