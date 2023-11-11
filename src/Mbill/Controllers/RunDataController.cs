using Mbill.Core.Domains.Entities.Bill;
using Mbill.Core.Domains.Entities.Core;
using Mbill.Core.Domains.Entities.PreOrder;
using Mbill.Core.Interface.IRepositories.Bill;

namespace Mbill.Controllers;

[Route("api/rundata")]
public class RunDataController : ApiControllerBase
{
    private readonly IBillRepo _billRepo;
    private readonly IBillMongoRepo _billMongoRepo;

    public RunDataController(IBillRepo billRepo,
        IBillMongoRepo billMongoRepo)
    {

        _billRepo = billRepo;
        _billMongoRepo = billMongoRepo;
    }

    //[HttpGet]
    //public async Task<ServiceResult> BillWriteToMongoDBAsync()
    //{
    //    var list = await _billRepo.Select.Where(b => b.IsDeleted == false).ToListAsync();
    //    var s = await _billMongoRepo.InsertManyAsync(list);
    //    return ServiceResult.Successed("成功！");
    //}

    [HttpGet("refactor/migration/genbid")]
    public async Task<ServiceResult> MigrationGenTableBIdAsync()
    {
        var orm = _billRepo.Orm;

        var bills = await orm.Select<BillEntity>().DisableGlobalFilter("IsDeleted").Where(i => i.BId <= 0).ToListAsync();
        foreach (var item in bills)
        {
            item.BId = SnowFlake.NextId();
            await orm.Update<BillEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var assets = await orm.Select<AssetEntity>().DisableGlobalFilter("IsDeleted").Where(i => i.BId <= 0).ToListAsync();
        foreach (var item in assets)
        {
            item.BId = SnowFlake.NextId();
            await orm.Update<AssetEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var cas = await orm.Select<CategoryEntity>().DisableGlobalFilter("IsDeleted").Where(i => i.BId <= 0).ToListAsync();
        foreach (var item in cas)
        {
            item.BId = SnowFlake.NextId();
            await orm.Update<CategoryEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var fs = await orm.Select<FileEntity>().DisableGlobalFilter("IsDeleted").Where(i => i.BId <= 0).ToListAsync();
        foreach (var item in fs)
        {
            item.BId = SnowFlake.NextId();
            await orm.Update<FileEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var bis = await orm.Select<BaseItemEntity>().DisableGlobalFilter("IsDeleted").Where(i => i.BId <= 0).ToListAsync();
        foreach (var item in bis)
        {
            item.BId = SnowFlake.NextId();
            await orm.Update<BaseItemEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var logs = await orm.Select<LogEntity>().DisableGlobalFilter("IsDeleted").Where(i => i.BId <= 0).ToListAsync();
        foreach (var item in logs)
        {
            item.BId = SnowFlake.NextId();
            await orm.Update<LogEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var mis = await orm.Select<MediaImageEntity>().DisableGlobalFilter("IsDeleted").Where(i => i.BId <= 0).ToListAsync();
        foreach (var item in mis)
        {
            item.BId = SnowFlake.NextId();
            await orm.Update<MediaImageEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var ps = await orm.Select<PermissionEntity>().DisableGlobalFilter("IsDeleted").Where(i => i.BId <= 0).ToListAsync();
        foreach (var item in ps)
        {
            item.BId = SnowFlake.NextId();
            await orm.Update<PermissionEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var pos = await orm.Select<PreOrderEntity>().DisableGlobalFilter("IsDeleted").Where(i => i.BId <= 0).ToListAsync();
        foreach (var item in pos)
        {
            item.BId = SnowFlake.NextId();
            await orm.Update<PreOrderEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var pogs = await orm.Select<PreOrderGroupEntity>().DisableGlobalFilter("IsDeleted").Where(i => i.BId <= 0).ToListAsync();
        foreach (var item in pogs)
        {
            item.BId = SnowFlake.NextId();
            await orm.Update<PreOrderGroupEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var rs = await orm.Select<RoleEntity>().DisableGlobalFilter("IsDeleted").Where(i => i.BId <= 0).ToListAsync();
        foreach (var item in rs)
        {
            item.BId = SnowFlake.NextId();
            await orm.Update<RoleEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var rps = await orm.Select<RolePermissionEntity>().DisableGlobalFilter("IsDeleted").Where(i => i.BId <= 0).ToListAsync();
        foreach (var item in rps)
        {
            item.BId = SnowFlake.NextId();
            await orm.Update<RolePermissionEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var bts = await orm.Select<BaseTypeEntity>().DisableGlobalFilter("IsDeleted").Where(i => i.BId <= 0).ToListAsync();
        foreach (var item in bts)
        {
            item.BId = SnowFlake.NextId();
            await orm.Update<BaseTypeEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var us = await orm.Select<UserEntity>().DisableGlobalFilter("IsDeleted").Where(i => i.BId <= 0).ToListAsync();
        foreach (var item in us)
        {
            item.BId = SnowFlake.NextId();
            await orm.Update<UserEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var uis = await orm.Select<UserIdentityEntity>().DisableGlobalFilter("IsDeleted").Where(i => i.BId <= 0).ToListAsync();
        foreach (var item in uis)
        {
            item.BId = SnowFlake.NextId();
            await orm.Update<UserIdentityEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var urs = await orm.Select<UserRoleEntity>().DisableGlobalFilter("IsDeleted").Where(i => i.BId <= 0).ToListAsync();
        foreach (var item in urs)
        {
            item.BId = SnowFlake.NextId();
            await orm.Update<UserRoleEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }
        return ServiceResult.Successed("成功！");
    }

    [HttpGet("refactor/migration/relationbid")]
    public async Task<ServiceResult> MigrationRelationBIdAsync()
    {
        var orm = _billRepo.Orm;

        var assets = await orm.Select<AssetEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in assets)
        {
            if (item.ParentId == 0) continue;
            item.ParentBId = assets.FirstOrDefault(a => a.Id == item.ParentId).BId;
            await orm.Update<AssetEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var categories = await orm.Select<CategoryEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in categories)
        {
            if (item.ParentId == 0) continue;
            item.ParentBId = categories.FirstOrDefault(a => a.Id == item.ParentId).BId;
            await orm.Update<CategoryEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var bills = await orm.Select<BillEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in bills)
        {
            item.CategoryBId = categories.FirstOrDefault(a => a.Id == item.CategoryId)?.BId ?? 0;
            item.AssetBId = assets.FirstOrDefault(a => a.Id == item.AssetId)?.BId ?? 0;
            await orm.Update<BillEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var items = await orm.Select<BaseItemEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        var types = await orm.Select<BaseTypeEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in items)
        {
            item.BaseTypeBId = types.FirstOrDefault(a => a.Id == item.BaseTypeId).BId;
            await orm.Update<BaseItemEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var files = await orm.Select<FileEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        var medias = await orm.Select<MediaImageEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in medias)
        {
            item.FileBId = files.FirstOrDefault(a => a.Id == item.FileId).BId;
            await orm.Update<MediaImageEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var roles = await orm.Select<RoleEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        var permissions = await orm.Select<PermissionEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        var rolePermissions = await orm.Select<RolePermissionEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in rolePermissions)
        {
            item.RoleBId = roles.FirstOrDefault(a => a.Id == item.RoleId).BId;
            item.PermissionBId = permissions.FirstOrDefault(a => a.Id == item.PermissionId).BId;
            await orm.Update<RolePermissionEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var users = await orm.Select<UserEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        var userIdentities = await orm.Select<UserIdentityEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in userIdentities)
        {
            item.UserBId = users.FirstOrDefault(a => a.Id == item.UserId).BId;
            await orm.Update<UserIdentityEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var userRoles = await orm.Select<UserRoleEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in userRoles)
        {
            item.UserBId = users.FirstOrDefault(a => a.Id == item.UserId).BId;
            item.RoleBId = roles.FirstOrDefault(a => a.Id == item.RoleId).BId;
            await orm.Update<UserRoleEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var preGroups = await orm.Select<PreOrderGroupEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        var pres = await orm.Select<PreOrderEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in pres)
        {
            item.GroupBId = preGroups.FirstOrDefault(a => a.Id == item.GroupId).BId;
            await orm.Update<PreOrderEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        foreach (var item in preGroups)
        {
            item.BillBId = bills.FirstOrDefault(a => a.Id == item.BillId)?.BId ?? 0;
            await orm.Update<PreOrderGroupEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }
        

        return ServiceResult.Successed("成功！");
    }

    [HttpGet("refactor/migration/userbid")]
    public async Task<ServiceResult> MigrationUserIdToBIdAsync()
    {
        var orm = _billRepo.Orm;
        var users = await orm.Select<UserEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();

        var assets = await orm.Select<AssetEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in assets)
        {
            item.CreateUserBId = users.FirstOrDefault(a => a.Id == item.CreateUserId)?.BId ?? 0;
            item.UpdateUserBId = users.FirstOrDefault(a => a.Id == item.UpdateUserId)?.BId ?? 0;
            item.DeleteUserBId = users.FirstOrDefault(a => a.Id == item.DeleteUserId)?.BId ?? 0;
            await orm.Update<AssetEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var categories = await orm.Select<CategoryEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in categories)
        {
            item.CreateUserBId = users.FirstOrDefault(a => a.Id == item.CreateUserId)?.BId ?? 0;
            item.UpdateUserBId = users.FirstOrDefault(a => a.Id == item.UpdateUserId)?.BId ?? 0;
            item.DeleteUserBId = users.FirstOrDefault(a => a.Id == item.DeleteUserId)?.BId ?? 0;
            await orm.Update<CategoryEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var bills = await orm.Select<BillEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in bills)
        {
            item.CreateUserBId = users.FirstOrDefault(a => a.Id == item.CreateUserId)?.BId ?? 0;
            item.UpdateUserBId = users.FirstOrDefault(a => a.Id == item.UpdateUserId)?.BId ?? 0;
            item.DeleteUserBId = users.FirstOrDefault(a => a.Id == item.DeleteUserId)?.BId ?? 0;
            await orm.Update<BillEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var items = await orm.Select<BaseItemEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in items)
        {
            item.CreateUserBId = users.FirstOrDefault(a => a.Id == item.CreateUserId)?.BId ?? 0;
            item.UpdateUserBId = users.FirstOrDefault(a => a.Id == item.UpdateUserId)?.BId ?? 0;
            item.DeleteUserBId = users.FirstOrDefault(a => a.Id == item.DeleteUserId)?.BId ?? 0;
            await orm.Update<BaseItemEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var types = await orm.Select<BaseTypeEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in types)
        {
            item.CreateUserBId = users.FirstOrDefault(a => a.Id == item.CreateUserId)?.BId ?? 0;
            item.UpdateUserBId = users.FirstOrDefault(a => a.Id == item.UpdateUserId)?.BId ?? 0;
            item.DeleteUserBId = users.FirstOrDefault(a => a.Id == item.DeleteUserId)?.BId ?? 0;
            await orm.Update<BaseTypeEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var medias = await orm.Select<MediaImageEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in medias)
        {
            item.CreateUserBId = users.FirstOrDefault(a => a.Id == item.CreateUserId)?.BId ?? 0;
            item.UpdateUserBId = users.FirstOrDefault(a => a.Id == item.UpdateUserId)?.BId ?? 0;
            item.DeleteUserBId = users.FirstOrDefault(a => a.Id == item.DeleteUserId)?.BId ?? 0;
            await orm.Update<MediaImageEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var files = await orm.Select<FileEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in files)
        {
            item.CreateUserBId = users.FirstOrDefault(a => a.Id == item.CreateUserId)?.BId ?? 0;
            item.UpdateUserBId = users.FirstOrDefault(a => a.Id == item.UpdateUserId)?.BId ?? 0;
            item.DeleteUserBId = users.FirstOrDefault(a => a.Id == item.DeleteUserId)?.BId ?? 0;
            await orm.Update<FileEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var permissions = await orm.Select<PermissionEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in permissions)
        {
            item.CreateUserBId = users.FirstOrDefault(a => a.Id == item.CreateUserId)?.BId ?? 0;
            item.UpdateUserBId = users.FirstOrDefault(a => a.Id == item.UpdateUserId)?.BId ?? 0;
            item.DeleteUserBId = users.FirstOrDefault(a => a.Id == item.DeleteUserId)?.BId ?? 0;
            await orm.Update<PermissionEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var roles = await orm.Select<RoleEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in roles)
        {
            item.CreateUserBId = users.FirstOrDefault(a => a.Id == item.CreateUserId)?.BId ?? 0;
            item.UpdateUserBId = users.FirstOrDefault(a => a.Id == item.UpdateUserId)?.BId ?? 0;
            item.DeleteUserBId = users.FirstOrDefault(a => a.Id == item.DeleteUserId)?.BId ?? 0;
            await orm.Update<RoleEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        foreach (var item in users)
        {
            item.CreateUserBId = users.FirstOrDefault(a => a.Id == item.CreateUserId)?.BId ?? 0;
            item.UpdateUserBId = users.FirstOrDefault(a => a.Id == item.UpdateUserId)?.BId ?? 0;
            item.DeleteUserBId = users.FirstOrDefault(a => a.Id == item.DeleteUserId)?.BId ?? 0;
            await orm.Update<UserEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var userIdentities = await orm.Select<UserIdentityEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in userIdentities)
        {
            item.CreateUserBId = users.FirstOrDefault(a => a.Id == item.CreateUserId)?.BId ?? 0;
            item.UpdateUserBId = users.FirstOrDefault(a => a.Id == item.UpdateUserId)?.BId ?? 0;
            item.DeleteUserBId = users.FirstOrDefault(a => a.Id == item.DeleteUserId)?.BId ?? 0;
            await orm.Update<UserIdentityEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var pres = await orm.Select<PreOrderEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in pres)
        {
            item.CreateUserBId = users.FirstOrDefault(a => a.Id == item.CreateUserId)?.BId ?? 0;
            item.UpdateUserBId = users.FirstOrDefault(a => a.Id == item.UpdateUserId)?.BId ?? 0;
            item.DeleteUserBId = users.FirstOrDefault(a => a.Id == item.DeleteUserId)?.BId ?? 0;
            await orm.Update<PreOrderEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        var preGroups = await orm.Select<PreOrderGroupEntity>().DisableGlobalFilter("IsDeleted").ToListAsync();
        foreach (var item in preGroups)
        {
            item.CreateUserBId = users.FirstOrDefault(a => a.Id == item.CreateUserId)?.BId ?? 0;
            item.UpdateUserBId = users.FirstOrDefault(a => a.Id == item.UpdateUserId)?.BId ?? 0;
            item.DeleteUserBId = users.FirstOrDefault(a => a.Id == item.DeleteUserId)?.BId ?? 0;
            await orm.Update<PreOrderGroupEntity>().DisableGlobalFilter("IsDeleted").SetSource(item).ExecuteAffrowsAsync();
        }

        return ServiceResult.Successed("成功！");
    }

    [HttpGet("refactor/migration/mongodb/bid")]
    public async Task<ServiceResult> MigrationMongoDbBIdAsync()
    {
        var list = await _billRepo.Select.DisableGlobalFilter("IsDeleted").ToListAsync();
        var s = await _billMongoRepo.InsertManyAsync(list);
        return ServiceResult.Successed("成功！");
    }
}
