namespace mbill.Service.PreOrder.Output;

public class PreOrderGroupDto : EntityDto
{
    public string Name { get; set; }

    public string Description { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}
