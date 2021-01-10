/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Data
*   文件名称 ：DataSeedContributor.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-09 14:59:50
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Entities.System;
using Memoyu.Mbill.Domain.Shared.Const;
using Memoyu.Mbill.Domain.Shared.Security;
using Memoyu.Mbill.ToolKits.Base.Dependency;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Domain.Data
{
    public class DataSeedContributor : IDataSeedContributor, ISingletonDependency
    {

        private readonly IAuditBaseRepository<PermissionEntity> _permissionRepository;
        private readonly IAuditBaseRepository<RolePermissionEntity> _rolePermissionRepository;
        private readonly ILogger<DataSeedContributor> _logger;
        public DataSeedContributor(IAuditBaseRepository<PermissionEntity> permissionRepository, IAuditBaseRepository<RolePermissionEntity> rolePermissionRepository, ILogger<DataSeedContributor> logger)
        {
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _logger = logger;
        }

        public async Task InitAdministratorPermissionAsync()
        {
            bool valid = await _rolePermissionRepository.Select.AnyAsync();
            if (valid) return;
            List<PermissionEntity> allPermissions = await _permissionRepository.Select.ToListAsync();//获取所有权限
            List<RolePermissionEntity> rolePermissions = allPermissions.Select(u => new RolePermissionEntity(SystemConst.Role.Administrator, u.Id)).ToList();//构建超级管理员角色权限
            await _rolePermissionRepository.InsertAsync(rolePermissions);//插入全部的超级管理员角色权限
        }

        public async Task InitPermissionAsync(List<PermissionDefinition> permissions)
        {
            List<PermissionEntity> insertPermissions = new List<PermissionEntity>();//新增权限集合
            List<PermissionEntity> updatePermissions = new List<PermissionEntity>();//更新权限集合

            Expression<Func<RolePermissionEntity, bool>> rolePermissionExpression = u => false;
            Expression<Func<PermissionEntity, bool>> permissionExpression = u => false;

            List<PermissionEntity> allPermissions = await _permissionRepository.Select.ToListAsync();//已持久化的权限数据

            allPermissions.ForEach(per =>//过滤已持久化的权限数据，获得需要删除的权限、角色权限数据
            {
                if (permissions.All(r => r.Permission != per.Name))//持久化的权限数据是否存在于在本次获取到的权限数据
                {
                    permissionExpression = permissionExpression.Or(r => r.Id == per.Id);//拼接表达式，权限Id
                    rolePermissionExpression = rolePermissionExpression.Or(r => r.PermissionId == per.Id);//拼接表达式，角色权限Id
                }
            });

            int effectPerRows = await _permissionRepository.DeleteAsync(permissionExpression);//删除权限数据
            int effectRolePerRows = await _rolePermissionRepository.DeleteAsync(rolePermissionExpression);//删除角色权限数据
            _logger.LogInformation($"操 作 权 限 表：删除了{effectPerRows}条数据");
            _logger.LogInformation($"操作角色权限表：删除了{effectRolePerRows}条数据");

            permissions.ForEach(per =>//过滤本次获取到的权限数据，获得需要新增、更新的权限数据
            {
                PermissionEntity permissionEntity = allPermissions.FirstOrDefault(u => u.Module == per.Module && u.Name == per.Permission);//在已持久化的权限数据中获取符合条件数据
                if (permissionEntity == null)//如果权限数据为空，则可新增
                {
                    insertPermissions.Add(new PermissionEntity(per.Permission, per.Module, per.Router));
                }
                else//否则
                {
                    bool routerExist = allPermissions.Any(u => u.Module == per.Module && u.Name == per.Permission && u.Router == per.Router);//是否存在符合条件的数据
                    if (!routerExist)//不存在则证明Router发生了改变，则更新Router，
                    {
                        permissionEntity.Router = per.Router;
                        updatePermissions.Add(permissionEntity);
                    }
                }
            });

            await _permissionRepository.InsertAsync(insertPermissions);
            _logger.LogInformation($"操 作 权 限 表：新增了{insertPermissions.Count}条数据");

            await _permissionRepository.UpdateAsync(updatePermissions);
            _logger.LogInformation($"操 作 权 限 表：更新了{updatePermissions.Count}条数据");

        }
    }


}
