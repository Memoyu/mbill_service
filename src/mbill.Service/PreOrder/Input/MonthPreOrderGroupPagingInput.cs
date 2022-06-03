namespace mbill.Service.Bill.Bill.Input;

public class MonthPreOrderGroupPagingInput : PagingDto
{
    /// <summary>
    /// 指定的年月
    /// </summary>
    public DateTime Month { get; set; }

    /// <summary>
    /// 分组名称
    /// </summary>
    public string Name { get; set; }

}
