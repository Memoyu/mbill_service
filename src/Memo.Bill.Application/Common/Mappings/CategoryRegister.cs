using Memo.Bill.Application.Categories.Commands;

namespace Memo.Bill.Application.Common.Mappings
{
    public class CategoryRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<UpdateCategoryCommand, Category>().IgnoreNullValues(true);
        }
    }
}
