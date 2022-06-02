namespace mbill.Service.PreOrder.Input;
public class CreatePreOrderGroupInput
{
    /// <summary>
    /// 分组名
    /// </summary>
    [Required(ErrorMessage = "必须传入分组名")]
    [MaxLength(20, ErrorMessage ="分组名长度不超过20")]
    public string Name { get; set; }

    /// <summary>
    /// 分组描述
    /// </summary>
    [MaxLength(200, ErrorMessage = "分组描述度不超过200")]
    public string Description { get; set; }
}
