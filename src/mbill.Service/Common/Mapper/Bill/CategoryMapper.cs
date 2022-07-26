namespace mbill.Service.Common.Mapper.Bill;

public class CategoryMapper : Profile
{
    public CategoryMapper()
    {
        CreateMap<CreateCategoryInput, CategoryEntity>();
        CreateMap<EditCategoryInput, CategoryEntity>();
        CreateMap<CategoryEntity, CategoryDto>();
        CreateMap<CategoryEntity, CategoryPageDto>();
    }
}
