namespace Memo.Bill.Domain.Common;

public class BaseEntity
{
    /// <summary>
    /// 自增主键Id
    /// </summary>
    [Column(IsPrimary = true, IsIdentity = true, Position = 1)]
    [Description("主键Id")]
    public long Id { get; set; }

    #region 领域事件

    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear(); 

    #endregion
}

public class BaseAuditEntity : BaseEntity
{
    /// <summary>
    /// 创建人UserId
    /// </summary>
    [Column(Position = -7, CanUpdate = false)]
    [Description("创建人UserId")]
    public long CreateUserId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Column(Position = -6, CanUpdate = false)]
    [Description("创建时间")]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改人UserId
    /// </summary>
    [Column(Position = -5)]
    [Description("修改人UserId")]
    public long? UpdateUserId { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    [Column(Position = -4)]
    [Description("修改时间")]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    [Column(Position = -3)]
    [Description("是否删除")]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 删除人UserId
    /// </summary>
    [Column(Position = -2)]
    [Description("删除人UserId")]
    public long? DeleteUserId { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    [Column(Position = -1)]
    [Description("删除时间")]
    public DateTime? DeleteTime { get; set; }

}
