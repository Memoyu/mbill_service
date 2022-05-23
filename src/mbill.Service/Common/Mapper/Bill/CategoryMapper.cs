namespace mbill.Service.Common.Mapper.Bill;

public class CategoryMapper : Profile
{
    public CategoryMapper()
    {
        CreateMap<ModifyCategoryDto, CategoryEntity>();
        CreateMap<CategoryEntity, CategoryDto>();
        CreateMap<CategoryEntity, CategoryPageDto>();
    }
}
