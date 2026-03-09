using Memo.Bill.Domain.Events.Roles;

namespace Memo.Bill.Application.Roles.Commands;

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

public class DeleteRoleCommandHandler(IBaseDefaultRepository<Role> roleRepo) : IRequestHandler<DeleteRoleCommand, Result>
{
    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepo.Select.Where(t => t.RoleId == request.RoleId).FirstAsync(cancellationToken) ?? throw new ApplicationException("标签不存在");

        role.AddDomainEvent(new DeletedRoleEvent(request.RoleId));

        var affrows = await roleRepo.DeleteAsync(role, cancellationToken);

        return affrows > 0 ? Result.Success() : throw new ApplicationException("删除角色失败");
    }
}
