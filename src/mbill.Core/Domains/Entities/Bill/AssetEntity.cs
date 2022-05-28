namespace mbill.Core.Domains.Entities.Bill;

/// <summary>
/// 资产分类实体
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_asset")]
[Index("index_asset_on_amount", "Amount", false)]
[Index("index_asset_on_sort", "false", false)]
[Index("index_asset_on_parent_id", "ParentId", false)]
[Index("index_asset_on_type", "Type", false)]
public class AssetEntity : FullAduitEntity
{
    /// <summary>
    /// 资产分类名
    /// </summary>
    [Column(StringLength = 20, IsNullable = false)]
    public string Name { get; set; }

    /// <summary>
    /// 父级Id，默认0
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 资产分类类型：存款、负债
    /// </summary>
    [Column(IsNullable = false)]
    public int Type { get; set; }

    /// <summary>
    /// 资产金额
    /// </summary>
    [Column(Precision = 12, Scale = 2)]
    public decimal Amount { get; set; }

    /// <summary>
    /// 图标地址
    /// </summary>
    [Column(StringLength = 100)]
    public string IconUrl { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}
