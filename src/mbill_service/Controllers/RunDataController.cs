using mbill_service.Core.Interface.IRepositories.Bill;
using mbill_service.Core.Interface.IRepositories.Core;

namespace mbill_service.Controllers;

[Route("api/rundata")]
public class RunDataController : ApiControllerBase
{
    private readonly IAssetRepo _assetRepo;
    private readonly ICategoryRepo _categoryRepo;
    private readonly IBaseTypeRepo _baseTypeRepo;
    private readonly IBaseItemRepo _baseItemRepo;
    private readonly IFileRepo _fileRepo;
    private readonly IPermissionRepo _permissionRepo;
    private readonly IRoleRepo _roleRepo;
    private readonly IRolePermissionRepo _rolePermissionRepo;
    private readonly IUserRepo _userRepo;
    private readonly IUserRoleRepo _userRoleRepo;

    public RunDataController(IAssetRepo assetRepo,
        ICategoryRepo categoryRepo,
        IBaseTypeRepo baseTypeRepo,
        IBaseItemRepo baseItemRepo,
        IFileRepo fileRepo,
        IPermissionRepo permissionRepo,
        IRoleRepo roleRepo,
        IRolePermissionRepo rolePermissionRepo,
        IUserRepo userRepo,
        IUserRoleRepo userRoleRepo)
    {
        _assetRepo = assetRepo;
        _categoryRepo = categoryRepo;
        _baseTypeRepo = baseTypeRepo;
        _baseItemRepo = baseItemRepo;
        _fileRepo = fileRepo;
        _permissionRepo = permissionRepo;
        _roleRepo = roleRepo;
        _rolePermissionRepo = rolePermissionRepo;
        _userRepo = userRepo;
        _userRoleRepo = userRoleRepo;
    }

    /* [HttpGet]
     public async Task<ServiceResult> UpdateBIdAsync()
     {
         await UpdateUser();
         await UpdateAsset();
         await UpdateCategory();
         await UpdateType();
         await UpdateTypeItem();
         await UpdateFile();
         await UpdatePermission();
         await UpdateRole();
         await UpdateRolePermission();
         // await UpdateUserRole();
         return ServiceResult.Successed("成功！");
     }

     private async Task UpdateUser()
     {
         var entities = await _userRepo.Select.ToListAsync();
         foreach (var item in entities)
         {
             item.BId = Guid.NewGuid();
             var res = await _userRepo.UpdateAsync(item);
         }
     }

     private async Task UpdateAsset()
     {
         var entities = await _assetRepo.Select.ToListAsync();
         foreach (var item in entities)
         {
             item.BId = Guid.NewGuid();
             var res = await _assetRepo.UpdateAsync(item);
         }
         var latestEntities = await _assetRepo.Select.ToListAsync();
         foreach (var item in latestEntities)
         {
             item.ParentBId = latestEntities.FirstOrDefault(a => a.Id == item.ParentId)?.BId ?? Guid.Empty;
             var res = await _assetRepo.UpdateAsync(item);
         }

     }

     private async Task UpdateCategory()
     {
         var entities = await _categoryRepo.Select.ToListAsync();
         foreach (var item in entities)
         {
             item.BId = Guid.NewGuid();
             var res = await _categoryRepo.UpdateAsync(item);
         }
         var latestEntities = await _categoryRepo.Select.ToListAsync();
         foreach (var item in latestEntities)
         {
             item.ParentBId = latestEntities.FirstOrDefault(a => a.Id == item.ParentId)?.BId ?? Guid.Empty;
             var res = await _categoryRepo.UpdateAsync(item);
         }
     }

     private async Task UpdateType()
     {
         var entities = await _baseTypeRepo.Select.ToListAsync();
         foreach (var item in entities)
         {
             item.BId = Guid.NewGuid();
             var res = await _baseTypeRepo.UpdateAsync(item);
         }
     }

     private async Task UpdateTypeItem()
     {
         var entities = await _baseItemRepo.Select.ToListAsync();
         foreach (var item in entities)
         {
             item.BId = Guid.NewGuid();
             var res = await _baseItemRepo.UpdateAsync(item);
         }
         var latestEntities = await _baseItemRepo.Select.ToListAsync();
         var typeEntities = await _baseTypeRepo.Select.ToListAsync();
         foreach (var item in latestEntities)
         {
             item.BaseTypeBId = typeEntities.FirstOrDefault(a => a.Id == item.BaseTypeId)?.BId ?? Guid.Empty;
             var res = await _baseItemRepo.UpdateAsync(item);
         }

     }

     private async Task UpdateFile()
     {
         var entities = await _fileRepo.Select.ToListAsync();
         foreach (var item in entities)
         {
             item.BId = Guid.NewGuid();
             var res = await _fileRepo.UpdateAsync(item);
         }
     }

     private async Task UpdatePermission()
     {
         var entities = await _permissionRepo.Select.ToListAsync();
         foreach (var item in entities)
         {
             item.BId = Guid.NewGuid();
             var res = await _permissionRepo.UpdateAsync(item);
         }
     }

     private async Task UpdateRole()
     {
         var entities = await _roleRepo.Select.ToListAsync();
         foreach (var item in entities)
         {
             item.BId = Guid.NewGuid();
             var res = await _roleRepo.UpdateAsync(item);
         }
     }

     private async Task UpdateRolePermission()
     {
         var entities = await _rolePermissionRepo.Select.ToListAsync();
         foreach (var item in entities)
         {
             item.BId = Guid.NewGuid();
             var res = await _rolePermissionRepo.UpdateAsync(item);
         }

         var latestEntities = await _rolePermissionRepo.Select.ToListAsync();
         var permissionEntities = await _permissionRepo.Select.ToListAsync();
         var roleEntities = await _roleRepo.Select.ToListAsync();

         foreach (var item in latestEntities)
         {
             item.RoleBId = roleEntities.FirstOrDefault(x => x.Id == item.RoleId)?.BId ?? Guid.Empty;
             item.PermissionBId = permissionEntities.FirstOrDefault(x => x.Id == item.PermissionId)?.BId ?? Guid.Empty;
             var res = await _rolePermissionRepo.UpdateAsync(item);
         }
     }

     private async Task UpdateUserRole()
     {
         var userEntities = await _userRepo.Select.ToListAsync();
         var roleEntities = await _roleRepo.Select.ToListAsync();
         var entities = await _userRoleRepo.Select.ToListAsync();
         foreach (var item in entities)
         {
             item.BId = Guid.NewGuid();
             item.UserBId = userEntities.FirstOrDefault(c => c.Id.ToString() == item.UserBId.ToString())?.BId ?? Guid.Empty;
             item.RoleBId = roleEntities.FirstOrDefault(x => x.Id == item.RoleId)?.BId ?? Guid.Empty;
             var res = await _userRoleRepo.UpdateAsync(item);
         }
     }*/
}
