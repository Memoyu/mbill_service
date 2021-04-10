using Memoyu.Mbill.Application.Base.Impl;
using Memoyu.Mbill.Application.Contracts.Dtos.Core;
using Memoyu.Mbill.Domain.IRepositories.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Core.Impl
{
    public class RoleService : ApplicationService, IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<RoleDto>> GetAllAsync()
        {
            var entitys = await _roleRepository.Select.Where(r => r.IsDeleted == false).ToListAsync();
            var dtos = entitys.Select(e => Mapper.Map<RoleDto>(e)).ToList();
            return dtos;
        }
    }
}
