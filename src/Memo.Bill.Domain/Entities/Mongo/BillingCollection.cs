using Memo.Bill.Domain.Constants;
using Memo.Bill.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Memo.Bill.Domain.Entities.Mongo;

/// <summary>
/// 账单记录
/// </summary>
[MongoCollection(AppConst.BillCollectionName)]
public class BillingCollection
{
    /// <summary>
    /// 账单Id
    /// </summary>
    [BsonId]
    public long BillId { get; set; }

    /// <summary>
    /// 分类
    /// </summary>
    public BillingCollCategory Category { get; set; } = new();

    /// <summary>
    /// 账户
    /// </summary>
    public BillingCollAccount Account { get; set; } = new();

    /// <summary>
    /// 账本
    /// </summary>
    public BillingCollLedger Ledger { get; set; } = new();

    /// <summary>
    /// 标签
    /// </summary>
    public List<BillingCollTag> Tags { get; set; } = new();

    /// <summary>
    /// 金额
    /// </summary>
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Amount { get; set; }

    /// <summary>
    /// 类型：0-支出、1-收入
    /// </summary>
    public BillType Type { get; set; }

    /// <summary>
    /// 关键字
    /// 备注、地址 分词
    /// </summary>
    public string Keyword { get; set; } = string.Empty;

    /// <summary>
    /// 日期
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 创建人UserId
    /// </summary>
    public long CreateUserId { get; set; }
}

public class BillingCollCategory
{
    /// <summary>
    /// 分类Id
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 父级Id
    /// </summary>
    public long? ParentId { get; set; }
}

public class BillingCollAccount
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
    /// 父级Id
    /// </summary>
    public long? ParentId { get; set; }
}


public class BillingCollLedger
{
    /// <summary>
    /// 账本Id
    /// </summary>
    public long LedgerId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;
}

public class BillingCollTag
{
    /// <summary>
    /// 标签Id
    /// </summary>
    public long TagId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 父级Id
    /// </summary>
    public long? ParentId { get; set; }
}

