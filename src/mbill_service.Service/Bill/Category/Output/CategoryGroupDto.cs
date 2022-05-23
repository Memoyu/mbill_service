namespace mbill_service.Service.Bill.Category.Output;

public class CategoryGroupDto
{
    public string Name { get; set; }

    public List<CategoryDto> Childs { get; set; }
}
