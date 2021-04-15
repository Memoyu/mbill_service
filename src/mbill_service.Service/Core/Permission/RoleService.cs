using mbill_service.Core.Domains.Entities.Core;
using mbill_service.Core.Interface.IRepositories.Core;
using mbill_service.Service.Base;
using mbill_service.Service.Core.Permission.Input;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mbill_service.Service.Core.Permission
{
    public class RoleService : ApplicationService, IRoleService
    {
        private readonly IRoleRepo _roleRepo;
        public RoleService(IRoleRepo roleRepo)
        {
            _roleRepo = roleRepo;
        }


        public async Task<List<RoleDto>> GetAllAsync()
        {
            var entitys = await _roleRepo.Select.Where(r => r.IsDeleted == false).ToListAsync();
            var dtos = entitys.Select(e => Mapper.Map<RoleDto>(e)).ToList();
            return dtos;
        }

        public Task InsertAsync(RoleEntity role)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(RoleEntity role)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<RoleDto>> GetAsync(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}
