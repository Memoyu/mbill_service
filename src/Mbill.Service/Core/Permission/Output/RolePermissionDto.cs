namespace Mbill.Service.Core.Permission.Output
{
    public class RolePermissionDto : EntityDto
    {
        /// <summary>
        /// 角色BId
        /// </summary>
        public long RoleBId { get; set; }

        /// <summary>
        /// 权限BId
        /// </summary>
        public long PermissionBId { get; set; }
    }
}
