namespace Mbill.Core.Domains.Entities.Bill;

/// <summary>
/// 资产分类实体
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_asset")]
[Index("index_asset_on_amount", nameof(Amount), false)]
[Index("index_asset_on_sort", nameof(Sort), false)]
[Index("index_asset_on_parent_bid", nameof(ParentBId), false)]
[Index("index_asset_on_type", nameof(Type), false)]
public class AssetEntity : FullAduitEntity
{
    /// <summary>
    /// 资产分类名
    /// </summary>
    [Description("资产分类名")]
    [Column(StringLength = 20, IsNullable = false)]
    public string Name { get; set; }

    /// <summary>
    /// 父级BId
    /// </summary>
    [Description("父级BId")]
    public long ParentBId { get; set; }

    /// <summary>
    /// 资产分类类型：存款、负债
    /// </summary>
    [Description("资产分类类型：存款、负债")]
    [Column(IsNullable = false)]
    public int Type { get; set; }

    /// <summary>
    /// 资产金额
    /// </summary>
    [Description("资产金额")]
    [Column(Precision = 12, Scale = 2)]
    public decimal Amount { get; set; }

    /// <summary>
    /// 图标地址
    /// </summary>
    [Description("图标地址")]
    [Column(StringLength = 100)]
    public string Icon { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Description("排序")]
    public int Sort { get; set; }

    [Navigate(nameof(ParentBId), TempPrimary = nameof(AssetEntity.BId))]
    public virtual AssetEntity Parent { get; set; }
}
