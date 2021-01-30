/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Mapper.Bill.Category
*   文件名称 ：CategoryMapper.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-07 0:18:18
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using AutoMapper;
using Memoyu.Mbill.Application.Contracts.Dtos.Bill.Category;
using Memoyu.Mbill.Domain.Entities.Bill.Category;

namespace Memoyu.Mbill.Application.Contracts.Mapper.Bill.Category
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
