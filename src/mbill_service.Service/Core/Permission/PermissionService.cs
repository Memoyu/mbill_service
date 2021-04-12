using mbill_service.Core.Domains.Entities.Core;
using mbill_service.Core.Interface.IRepositories.Core;
using mbill_service.Service.Base;
using mbill_service.Service.Core.Permission.Input;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mbill_service.Service.Core.Permission
{
    public class PermissionService : ApplicationService, IPermissionService
    {
        private readonly IPermissionRepo _permissionRepo;
        private readonly IRolePermissionRepo _rolePermissionRepo;

        public PermissionService(IPermissionRepo permissionRepo , IRolePermissionRepo rolePermissionRepo )
        {
            _permissionRepo = permissionRepo;
            _rolePermissionRepo = rolePermissionRepo;
        }

        public async Task<IDictionary<string, IEnumerable<PermissionDto>>> GetAllStructual()
        {
            return (await _permissionRepo.Select.ToListAsync())
                   .GroupBy(r => r.Module)
                   .ToDictionary(
                       group => group.Key,
                       group =>
                           Mapper.Map<IEnumerable<PermissionDto>>(group.ToList())
                   );
        }

        public async Task<bool> CheckAsync(string permission)
        {
            long[] roleIds = CurrentUser.Roles;
            PermissionEntity permissionEntity = await _permissionRepo.Where(r => r.Name == permission).FirstAsync();
            bool existPermission = await _rolePermissionRepo.Select
                .AnyAsync(r => roleIds.Contains(r.RoleId) && r.PermissionId == permissionEntity.Id);
            return existPermission;
        }
    }
}
