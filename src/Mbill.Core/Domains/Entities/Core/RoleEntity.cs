﻿using Mbill.Core.Domains.Common.Enums;

namespace Mbill.Core.Domains.Entities.Core;

/// <summary>
/// 角色表
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_role")]
public class RoleEntity : FullAduitEntity
{
    /// <summary>
    /// 角色唯一标识字符
    /// </summary>
    [Column(StringLength = 60)]
    public string Name { get; set; }

    /// <summary>
    /// 角色类型
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 角色描述
    /// </summary>
    [Column(StringLength = 100)]
    public string Info { get; set; }

    /// <summary>
    /// 是否是静态分组,是静态时无法删除此分组
    /// </summary>
    public bool IsStatic { get; set; } = false;

    /// <summary>
    /// 排序码，升序
    /// </summary>
    public int SortCode { get; set; }

    [Navigate(ManyToMany = typeof(UserRoleEntity))]
    public virtual ICollection<UserEntity> Users { get; set; }

    [Navigate(nameof(RolePermissionEntity.RoleBId), TempPrimary = nameof(BId))]
    public virtual ICollection<RolePermissionEntity> RolePermissions { get; set; }
}
