namespace mbill.Service.PreOrder.Output;

public class PreOrderGroupWithStatDto : PreOrderGroupDto
{
    /// <summary>
    /// 分组中已完成预购总数
    /// </summary>
    public long Done { get; set; }

    /// <summary>
    /// 分组中未完成预购总数
    /// </summary>
    public long UnDone { get; set; }
}
