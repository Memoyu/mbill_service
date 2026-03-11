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
    /// 分类Id
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// 账户Id
    /// </summary>
    public long AccountId { get; set; }

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
    /// 备注
    /// </summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>
    /// 坐标
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 地点
    /// </summary>
    public string Address { get; set; } = string.Empty;

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
