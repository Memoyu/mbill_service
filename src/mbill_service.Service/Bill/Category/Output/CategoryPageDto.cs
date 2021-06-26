using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbill_service.Service.Bill.Category.Output
{
    public class CategoryPageDto : CategoryDto
    {
        public string ParentName { get; set; }

        public string TypeName { get; set; }
    }
}
