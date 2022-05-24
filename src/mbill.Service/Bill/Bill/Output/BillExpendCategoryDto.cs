namespace mbill.Service.Bill.Bill.Output;

public class BillExpendCategoryDto
{
    public IEnumerable<StatisticsDto> ParentCategoryStas { get; set; }
    public IEnumerable<ChildGroupDto> ChildCategoryStas { get; set; }
}

public class ChildGroupDto
{
    public string ParentName { get; set; }

    public IEnumerable<object> Childs { get; set; }
}
