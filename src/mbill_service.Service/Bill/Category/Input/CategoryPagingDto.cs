namespace mbill_service.Service.Bill.Category.Input;

public class CategoryPagingDto : PagingDto
{
    public string CategoryName { get; set; }

    public string ParentIds { get; set; }

    public string Type { get; set; }

    public DateTime? CreateStartTime { get; set; }

    public DateTime? CreateEndTime { get; set; }
}
