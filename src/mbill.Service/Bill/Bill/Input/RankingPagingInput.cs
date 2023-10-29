namespace Mbill.Service.Bill.Bill.Input;

public class RankingPagingInput : PagingDto
{
    public DateTime Date { get; set; }

    /// <summary>
    /// 查询类型：
    /// 0 月统计
    /// 1 年统计
    /// </summary>
    public int DateType { get; set; }

    /// <summary>
    /// 账单类型
    /// 0 支出
    /// 1 收入
    /// </summary>
    public int BillType { get; set; }

    public long? CategoryBId { get; set; }
}
