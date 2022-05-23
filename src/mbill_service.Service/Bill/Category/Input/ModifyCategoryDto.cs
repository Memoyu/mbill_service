namespace mbill_service.Service.Bill.Category.Input;

public class ModifyCategoryDto
{
    /// <summary>
    /// 账单分类名
    /// </summary>
    [Required(ErrorMessage = "必须传入分类名称")]
    public string Name { get; set; }

    /// <summary>
    /// 父级Id，默认0
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 分类类型：支出、收入
    /// </summary>
    [Required(ErrorMessage = "必须传入分类类型")]
    public string Type { get; set; }

    /// <summary>
    /// 预算金额
    /// </summary>
    public decimal Budget { get; set; }

    /// <summary>
    /// 图标地址
    /// </summary>
    public string IconUrl { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}
