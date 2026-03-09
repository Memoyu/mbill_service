using Memo.Bill.Application.Categories.Commands;

namespace Memo.Bill.Application.Common.Mappings
{
    public class CategoryRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<UpdateCategoryCommand, Category>()
             .Map(d => d.CategoryId, s => s.CategoryId)
             .Map(d => d.Name, s => s.Name)
             .Map(d => d.Icon, s => s.Icon)
             .Map(d => d.IsDefault, s => s.IsDefault)
             .Map(d => d.ParentId, s => s.ParentId);
        }
    }
}
