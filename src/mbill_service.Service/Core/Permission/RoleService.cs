using mbill_service.Core.AOP.Attributes;
using mbill_service.Core.Domains.Common.Enums.Base;
using mbill_service.Core.Domains.Entities.Core;
using mbill_service.Core.Exceptions;
using mbill_service.Core.Interface.IRepositories.Core;
using mbill_service.Service.Base;
using mbill_service.Service.Core.Permission.Input;
using mbill_service.Service.Core.Permission.Output;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mbill_service.Service.Core.Permission
{
    public class RoleService : ApplicationService, IRoleService
    {
        private readonly IRoleRepo _roleRepo;
        private readonly IRolePermissionRepo _rolePermissionRepo;
        private readonly IPermissionRepo _permissionRepo;
        public RoleService(IRoleRepo roleRepo, IRolePermissionRepo rolePermissionRepo, IPermissionRepo permissionRepo)
        {
            _roleRepo = roleRepo;
            _rolePermissionRepo = rolePermissionRepo;
            _permissionRepo = permissionRepo;
        }


        public async Task<List<RoleDto>> GetAllAsync()
        {
            var entitys = await _roleRepo.Select.Where(r => r.IsDeleted == false).ToListAsync();
            var dtos = entitys.Select(e => Mapper.Map<RoleDto>(e)).ToList();
            return dtos;
        }

        [Transactional]
        public async Task InsertAsync(ModifyRoleDto role)
        {
            bool isRepeatName = await _roleRepo.Select.AnyAsync(r => r.Name == role.Name);
            if (isRepeatName)//角色名重复
                throw new KnownException("角色名称重复，请重新输入", ServiceResultCode.RepeatField);
            var permissions = await _permissionRepo.Select.Where(p => p.IsDeleted == false).ToListAsync();
            foreach (var permissionId in role.PermissionIds)
            {
                if (!permissions.Any(p => p.Id == permissionId)) 
                    throw new KnownException($"Id:{permissionId} 权限不存在！", ServiceResultCode.NotFound);
            }

            var input = Mapper.Map<RoleEntity>(role);
            var entity = await _roleRepo.InsertAsync(input);
            await _rolePermissionRepo.InsertAsync(role.PermissionIds.Select(p => new RolePermissionEntity
            {
                RoleId = entity.Id,
                PermissionId = p
            }));  
        }

        [Transactional]
        public async Task DeleteAsync(long id)
        {
            var dto = await GetAsync(id);

            await _rolePermissionRepo.DeleteAsync(entity)
        }

        public Task UpdateAsync(ModifyRoleDto role)
        {
            throw new System.NotImplementedException();
        }

        public async Task<RolePermissionDto> GetAsync(long id)
        {
            var role = await _roleRepo
                .Select
                .Where(r =>r.IsDeleted == false)
                .Where(r => r.Id == id).FirstAsync();
            if (role == null)
                throw new KnownException("角色不存在！", ServiceResultCode.NotFound);
            var dto = Mapper.Map<RolePermissionDto>(role);
            return 
            
        }
    }
}
