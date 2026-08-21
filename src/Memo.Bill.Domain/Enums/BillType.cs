namespace Memo.Bill.Domain.Enums;

public enum BillType
{
    /// <summary>
    /// 支出
    /// </summary>
    Expend = 0,

    /// <summary>
    /// 收入
    /// </summary>
    Income = 1
}


public enum BillPropUsageRecordType
{
    /// <summary>
    /// 分类
    /// </summary>
    Category = 0,

    /// <summary>
    /// 账户
    /// </summary>
    Account = 1,

    /// <summary>
    /// 标签
    /// </summary>
    Tag = 2,
}