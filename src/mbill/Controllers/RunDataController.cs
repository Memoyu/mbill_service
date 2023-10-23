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

    [HttpGet("update/bid")]
    public async Task<ServiceResult> WriteToTableBIdAsync()
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
}
