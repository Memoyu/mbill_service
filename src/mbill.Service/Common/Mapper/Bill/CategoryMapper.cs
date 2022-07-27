using mbill.Service.Common.Converter;

namespace mbill.Service.Common.Mapper.Bill;

public class CategoryMapper : Profile
{
    public CategoryMapper()
    {
        CreateMap<CreateCategoryInput, CategoryEntity>();
        CreateMap<EditCategoryInput, CategoryEntity>();
        CreateMap<CategoryEntity, CategoryDto>()
            .ForMember(d => d.IconUrl, opt => opt.ConvertUsing<StringUrlConverter, string>(c => c.Icon));
        CreateMap<CategoryEntity, CategoryPageDto>();
    }
}
