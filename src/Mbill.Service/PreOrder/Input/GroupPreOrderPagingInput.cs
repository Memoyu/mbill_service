namespace Mbill.Service.Bill.Bill.Input;

public class GroupPreOrderPagingInput : PagingDto
{
    /// <summary>
    /// 指定的分组
    /// </summary>
    public long BId { get; set; }

    /// <summary>
    /// 预购状态
    /// </summary>
    public int? Status { get; set; }

}
