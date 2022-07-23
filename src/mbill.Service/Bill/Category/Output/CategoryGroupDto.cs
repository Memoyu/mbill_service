namespace mbill.Service.Bill.Category.Output;

public class CategoryGroupDto
{
    public long Id { get; set; }

    public string Name { get; set; }

    public int Label { get; set; }

    public List<CategoryDto> Childs { get; set; }
}
