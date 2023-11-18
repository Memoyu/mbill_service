namespace Mbill.Core.Domains.Common.Base;

# region Input

public abstract class BaseUpdateInput
{
    /// <summary>
    /// 业务主键Id(雪花Id)
    /// </summary>
    public long BId { get; set; }
}


#endregion

#region EntityDto

public interface IEntityDto
{
}

public abstract class EntityDto : IEntityDto
{
    ///// <summary>
    ///// 自增主键Id
    ///// </summary>
    //public long Id { get; set; }

    /// <summary>
    /// 业务主键Id(雪花Id)
    /// </summary>
    public long BId { get; set; }
}


public class FullEntityDto : EntityDto, IUpdateAuditEntity, IDeleteAduitEntity, ICreateAduitEntity
{
    /// <summary>
    /// 创建者BId
    /// </summary>
    public long CreateUserBId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 删除人BId
    /// </summary>
    public long? DeleteUserBId { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    public DateTime? DeleteTime { get; set; }

    /// <summary>
    /// 最后修改人BId
    /// </summary>
    public long? UpdateUserBId { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}

#endregion EntityDto

#region Entity

public interface IEntity
{
    /// <summary>
    /// 自增主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 业务Id(雪花Id)
    /// </summary>
    public long BId { get; set; }
}

[Serializable]
[Index("index_on_bid", nameof(BId), false)]
public class Entity : IEntity
{
    /// <summary>
    /// 自增主键Id
    /// </summary>
    [Column(IsPrimary = true, IsIdentity = true, Position = 1)]
    public long Id { get; set; }

    /// <summary>
    /// 业务Id(雪花Id)
    /// </summary>
    [Column(Position = 2)]
    public long BId { get; set; }
}


[Serializable]
public class FullAduitEntity : Entity, IUpdateAuditEntity, IDeleteAduitEntity, ICreateAduitEntity
{
    /// <summary>
    /// 创建人BId
    /// </summary>
    [Column(Position = -7)]
    [Description("创建人BId")]
    public long CreateUserBId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Column(Position = -6)]
    [Description("创建时间")]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    [Column(Position = -5)]
    [Description("是否删除 0 未删除，1 已删除")]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 删除人BId
    /// </summary>
    [Column(Position = -4)]
    [Description("删除人BId")]
    public long? DeleteUserBId { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    [Column(Position = -3)]
    [Description("删除时间")]
    public DateTime? DeleteTime { get; set; }

    /// <summary>
    /// 修改人BId
    /// </summary>
    [Column(Position = -2)]
    [Description("修改人BId")]
    public long? UpdateUserBId { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    [Column(Position = -1)]
    [Description("修改时间")]
    public DateTime? UpdateTime { get; set; }
}

public interface ICreateAduitEntity
{
    /// <summary>
    /// 创建者BId
    /// </summary>
    long CreateUserBId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    DateTime CreateTime { get; set; }
}

public interface IUpdateAuditEntity
{
    /// <summary>
    /// 最后修改人BId
    /// </summary>
    long? UpdateUserBId { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    DateTime? UpdateTime { get; set; }
}

public interface IDeleteAduitEntity
{
    /// <summary>
    /// 是否删除
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// 删除人BId
    /// </summary>
    long? DeleteUserBId { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    DateTime? DeleteTime { get; set; }
}
#endregion Entity
