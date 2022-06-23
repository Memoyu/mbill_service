namespace mbill.Service.PreOrder.Output;

public class GroupPreOrderStatDto
{
    public string GroupName { get; set; }

    /// <summary>
    /// 分组创建时间
    /// </summary>
    public string Time { get; set; }

    public decimal PreAmount { get; set; }

    public long Done { get; set; }

    public long UnDone { get; set; }
}
