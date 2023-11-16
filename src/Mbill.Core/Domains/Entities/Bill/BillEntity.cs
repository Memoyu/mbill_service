using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Mbill.Core.Domains.Entities.Bill;

/// <summary>
/// 流水记录实体
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_bill")]
[MongoCollection(Name = "bill")]
[Index("index_bill_on_type", nameof(Type), false)]
[Index("index_bill_on_create_user_id_and_type", $"{nameof(CreateUserBId)},{nameof(Type)}", false)]
[Index("index_bill_on_create_user_id_and_category_bid", $"{nameof(CreateUserBId)},{nameof(CategoryBId)}", false)]
[Index("index_bill_on_create_user_id_and_asset_bid", $"{nameof(CreateUserBId)},{nameof(AssetBId)}", false)]
[Index("index_bill_on_time", nameof(Time), false)]
public class BillEntity : FullAduitEntity
{
    /// <summary>
    /// 分类BId
    /// </summary>
    [Description("分类BId")]
    public long CategoryBId { get; set; }

    /// <summary>
    /// 资产BId
    /// </summary>
    [Description("资产BId")]
    public long AssetBId { get; set; }

    /// <summary>
    /// 目标资产BId
    /// </summary>
    [Description("目标资产BId")]
    public long? TargetAssetBId { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    [BsonRepresentation(BsonType.Decimal128)]
    [Description("金额")]
    [Column(Precision = 12, Scale = 2)]
    public decimal Amount { get; set; }

    /// <summary>
    /// 账户余额
    /// </summary>
    [BsonRepresentation(BsonType.Decimal128)]
    [Description("账户余额")]
    [Column(Precision = 12, Scale = 2)]
    public decimal AssetResidue { get; set; }

    /// <summary>
    /// 记录类型：支出、收入、转账、还款
    /// </summary>
    [Description("记录类型：支出、收入、转账、还款")]
    [Column(IsNullable = false)]
    public int Type { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    [Description("说明")]
    [Column(StringLength = 40)]
    public string Description { get; set; }

    /// <summary>
    /// 地点
    /// </summary>
    [Description("地点")]
    [Column(StringLength = 200)]
    public string Address { get; set; }

    /// <summary>
    /// 记录日期
    /// </summary>
    [Description("记录日期")]
    [Column(StringLength = 10)]
    public DateTime Time { get; set; }


    [Navigate(nameof(CategoryBId), TempPrimary = nameof(CategoryEntity.BId))]
    public virtual CategoryEntity Category { get; set; }

    [Navigate(nameof(AssetBId), TempPrimary = nameof(AssetEntity.BId))]
    public virtual AssetEntity Asset { get; set; }

}