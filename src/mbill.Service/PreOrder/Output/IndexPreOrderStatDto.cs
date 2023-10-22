namespace Mbill.Service.PreOrder.Output;

public class IndexPreOrderStatDto
{
    public long Total { get; set; }

    /// <summary>
    /// 转账单分组数
    /// </summary>
    public int ToBill { get; set; }

    public long Done { get; set; }

    public long UnDone { get; set; }
}
