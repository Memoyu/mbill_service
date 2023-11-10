using Mapster;

namespace Mbill.Service.Common.Registers.Bill
{
    public class CategoryRegister : BaseRegister
    {
        protected override void TypeRegister(TypeAdapterConfig config)
        {
            config.ForType<CategoryEntity, CategoryDto>()
                .Map(d => d.IconUrl, s => UrlConverter(s.Icon));

            config.ForType<CategoryEntity, CategoryPageDto>()
                .Map(d => d.ParentName, s => s.Parent == null ? string.Empty : s.Parent.Name)
                .Map(d => d.TypeName, s => CategoryTypeConverter(s.Type))
                .Map(d => d.IconUrl, s => UrlConverter(s.Icon));
        }
    }
}
