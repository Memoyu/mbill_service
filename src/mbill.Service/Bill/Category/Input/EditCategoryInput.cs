namespace Mbill.Service.Bill.Category.Input;

public class EditCategoryInput
{
    /// <summary>
    /// 分组/分类Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 账单分类名
    /// </summary>
    [Required(ErrorMessage = "必须传入分类名称")]
    public string Name { get; set; }

    /// <summary>
    /// 预算金额
    /// </summary>
    public decimal Budget { get; set; }

    /// <summary>
    /// 图标地址
    /// </summary>
    public string Icon { get; set; }
}
