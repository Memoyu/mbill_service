namespace mbill_service.Service.Bill.Asset.Input;

public class ModifyAssetDto
{
    //[Range(1, 200, ErrorMessage = "年龄应该在1-200之间")]
    //[StringLength(100, ErrorMessage = "地址应小于100字符")]

    /// <summary>
    /// 资产分类名
    /// </summary>
    [Required(ErrorMessage = "必须传入资产分类名称")]
    public string Name { get; set; }

    /// <summary>
    /// 父级Id，默认0
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 资产分类类型：存款、负债
    /// </summary>
    [Required(ErrorMessage = "必须传入资产分类类型")]
    public string Type { get; set; }

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
