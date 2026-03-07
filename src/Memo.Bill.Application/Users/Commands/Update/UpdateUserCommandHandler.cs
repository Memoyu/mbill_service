namespace Memo.Bill.Application.Users.Commands.Update;

public class UpdateUserCommandHandler(
     IMapper mapper,
     IBaseDefaultRepository<User> userRepo,
     IBaseDefaultRepository<UserRole> userRoleRepo,
     IBaseDefaultRepository<Role> roleRepo
    ) : IRequestHandler<UpdateUserCommand, Result>
{
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepo.Select.Where(u => u.UserId == request.UserId).FirstAsync(cancellationToken) ?? throw new ApplicationException("用户不存在");

        var exist = await userRepo.Select.AnyAsync(u => request.Username == u.Username && u.UserId != request.UserId, cancellationToken);
        if (exist) throw new ApplicationException("同名用户已存在");

        var roles = await roleRepo.Select.Where(r => request.Roles.Contains(r.RoleId)).ToListAsync(cancellationToken);
        foreach (var roleId in request.Roles)
        {
            if (!roles.Any(r => r.RoleId == roleId)) throw new ApplicationException($"{roleId}角色不存在");
        }

        var updateUser = mapper.Map<User>(request);
        updateUser.Id = user.Id;
        updateUser.LastLoginTime = user.LastLoginTime;
        var affrows = await userRepo.UpdateAsync(updateUser, cancellationToken);
        if (affrows <= 0) throw new ApplicationException("更新用户失败");

        #region 用户关联角色管理

        var addUserRoles = new List<UserRole>();
        var currentUserRoles = await userRoleRepo.Select.Where(ur => ur.UserId == updateUser.UserId).ToListAsync(cancellationToken);
        foreach (var role in roles)
        {
            if (!currentUserRoles.Any(rp => rp.RoleId == role.RoleId))
            {
                addUserRoles.Add(new UserRole { RoleId = role.RoleId, UserId = request.UserId });
            }
            else
            {
                currentUserRoles.RemoveAll(t => t.RoleId == role.RoleId);
            }
        }
        addUserRoles = await userRoleRepo.InsertAsync(addUserRoles, cancellationToken);
        if (addUserRoles.Any(ur => ur.Id <= 0)) throw new ApplicationException("添加用户角色失败");

        var delUserRoleAffrows = await userRoleRepo.DeleteAsync(currentUserRoles, cancellationToken);
        if (delUserRoleAffrows != currentUserRoles.Count) throw new ApplicationException("删除用户角色失败");

        #endregion

        return Result.Success(updateUser.UserId);
    }
}
