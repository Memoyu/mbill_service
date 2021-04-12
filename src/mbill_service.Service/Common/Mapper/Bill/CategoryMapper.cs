using AutoMapper;
using mbill_service.Core.Domains.Entities.Bill;
using mbill_service.Service.Bill.Category.Input;
using mbill_service.Service.Bill.Category.Output;

namespace mbill_service.Service.Common.Mapper.Bill
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<ModifyCategoryDto, CategoryEntity>();
            CreateMap<CategoryEntity, CategoryDto>();
        }
    }
}
