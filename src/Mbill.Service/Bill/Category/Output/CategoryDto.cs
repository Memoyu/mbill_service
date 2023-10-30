namespace Mbill.Service.Bill.Category.Output;

public class CategoryDto
{
    public long BId { get; set; }

    public string Name { get; set; }

    public long ParentId { get; set; }

    public int Type { get; set; }

    public decimal Budget { get; set; }

    public int Sort { get; set; }

    public string IconUrl { get; set; }

    public string Icon { get; set; }
}
