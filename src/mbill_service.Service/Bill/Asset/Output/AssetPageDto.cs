using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbill_service.Service.Bill.Asset.Output
{
    public class AssetPageDto : AssetDto
    {
        public string ParentName { get; set; }

        public string TypeName { get; set; }
    }
}
