namespace mbill_service.Service.Bill.Category.Output;

public class CategoryDto : FullEntityDto
{
    public string Name { get; set; }

    public long ParentId { get; set; }

    public string Type { get; set; }

    public decimal Budget { get; set; }

    public int Sort { get; set; }

    public string IconUrl { get; set; }
}
