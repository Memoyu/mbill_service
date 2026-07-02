namespace Memo.Bill.Application.Accounts.Common;

internal record AccountBaseResult
{
    /// <summary>
    /// 账户Id
    /// </summary>
    public long AccountId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// 父级Id
    /// </summary>
    public long? ParentId { get; set; }
}


internal record AccountResult : AccountBaseResult
{
    /// <summary>
    /// 是否默认
    /// </summary>
    public bool Default { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}

internal record AccountWithParentResult : AccountResult
{
    /// <summary>
    /// 父级Id
    /// </summary>
    public AccountResult? Parent { get; set; }

}
