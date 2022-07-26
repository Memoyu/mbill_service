namespace mbill.Service.Bill.Asset.Input;

public class EditAssetInput
{
    public long Id { get; set; }

    /// <summary>
    /// 资产分类名
    /// </summary>
    [Required(ErrorMessage = "必须传入资产分类名称")]
    public string Name { get; set; }

    /// <summary>
    /// 资产金额
    /// </summary>

    public decimal Amount { get; set; }

    /// <summary>
    /// 图标地址
    /// </summary>

    public string IconUrl { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}
