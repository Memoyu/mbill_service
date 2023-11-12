using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Mbill.Core.Domains.Entities.Bill;

/// <summary>
/// 流水记录实体
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_bill")]
[MongoCollection(Name = "bill")]
[Index("index_bill_on_type", "Type", false)]
[Index("index_bill_on_create_user_id_and_type", "Sort,CreateUserId", false)]
[Index("index_bill_on_create_user_id_and_category_bid", "CategoryBId,CreateUserId", false)]
[Index("index_bill_on_create_user_id_and_asset_bid", "AssetBId,CreateUserId", false)]
[Index("index_bill_on_time", "Time", false)]
public class BillEntity : FullAduitEntity
{
    /// <summary>
    /// 分类BId
    /// </summary>
    public long CategoryBId { get; set; }

    /// <summary>
    /// 资产BId
    /// </summary>
    public long AssetBId { get; set; }

    /// <summary>
    /// 目标资产BId
    /// </summary>
    public long? TargetAssetBId { get; set; }

    /// <summary>
    /// 分类Id
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// 资产Id
    /// </summary>
    public long AssetId { get; set; }

    /// <summary>
    /// 目标资产Id
    /// </summary>
    public long? TargetAssetId { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    [BsonRepresentation(BsonType.Decimal128)]
    [Column(Precision = 12, Scale = 2)]
    public decimal Amount { get; set; }

    /// <summary>
    /// 账户余额
    /// </summary>
    [BsonRepresentation(BsonType.Decimal128)]
    [Column(Precision = 12, Scale = 2)]
    public decimal AssetResidue { get; set; }

    /// <summary>
    /// 记录类型：支出、收入、转账、还款
    /// </summary>
    [Column(IsNullable = false)]
    public int Type { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    [Column(StringLength = 40)]
    public string Description { get; set; }

    /// <summary>
    /// 地点
    /// </summary>
    [Column(StringLength = 200)]
    public string Address { get; set; }

    /// <summary>
    /// 记录日期：时间
    /// </summary>
    [Column(StringLength = 10)]
    public DateTime Time { get; set; }


    [Navigate(nameof(CategoryBId), TempPrimary = nameof(CategoryEntity.BId))]
    public virtual CategoryEntity Category { get; set; }

    [Navigate(nameof(AssetBId), TempPrimary = nameof(AssetEntity.BId))]
    public virtual AssetEntity Asset { get; set; }

}