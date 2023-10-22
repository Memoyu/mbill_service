using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbill.Service.Common.Registers.Bill
{
    public class CategoryRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<CategoryEntity, CategoryDto>()
           .Map(d => d.IconUrl, s => StringUrlConverter(s.Icon));
        }

        public string StringUrlConverter(string url)
        {
            var fileRepo = MapContext.Current.GetService<IFileRepo>();
            if (url.IsNullOrWhiteSpace()) return "";
            return fileRepo.GetFileUrl(url);
        }
    }
}
