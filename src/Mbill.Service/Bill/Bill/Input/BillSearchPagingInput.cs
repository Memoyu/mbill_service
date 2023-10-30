namespace Mbill.Service.Bill.Bill.Input;

public class BillSearchPagingInput : PagingDto
{
    /// <summary>
    /// 账单类型
    /// </summary>
    public List<int> Types { get; set; }

    /// <summary>
    /// 账单分类
    /// </summary>
    public List<long?> CategoryBIds { get; set; }

    /// <summary>
    /// 账单账户
    /// </summary>
    public List<long> AssetBIds { get; set; }

    /// <summary>
    /// 金额区间
    /// </summary>
    public SearchAmount Amount { get; set; }

    /// <summary>
    /// 日期区间
    /// </summary>
    public SearchDate Date { get; set; }

    /// <summary>
    /// 搜索关键字(地址、备注)
    /// </summary>
    public string KeyWord { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }


}

public class SearchAmount
{
    /// <summary>
    /// 金额区间最小值
    /// </summary>
    public decimal? Min { get; set; }

    /// <summary>
    /// 金额区间最大值
    /// </summary>
    public decimal? Max { get; set; }

}
public class SearchDate
{

    /// <summary>
    /// 账单时间起始
    /// </summary>
    public DateTime? Begin { get; set; }

    /// <summary>
    /// 账单时间截止
    /// </summary>
    public DateTime? End { get; set; }
}

