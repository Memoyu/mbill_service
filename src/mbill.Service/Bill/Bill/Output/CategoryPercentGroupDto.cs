namespace mbill.Service.Bill.Bill.Output;

public class CategoryPercentGroupDto
{
    public string Group { get; set; }

    public string Amount { get; set; }

    public List<CategoryPercentItemDto> Items { get; set; } = new();
}

public class CategoryPercentItemDto
{
    public long Id { get; set; }

    public string Category { get; set; }

    public string CategoryIcon { get; set; }

    public double Percent { get; set; }

    public string Amount { get; set; }
}