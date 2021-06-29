using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mbill_service.Core.Domains.Common;

namespace mbill_service.Service.Bill.Asset.Input
{
    public class AssetPagingDto : PagingDto
    {
        public string AssetName { get; set; }

        public string ParentIds { get; set; }

        public string Type { get; set; }


        public DateTime? CreateStartTime { get; set; }

        public DateTime? CreateEndTime { get; set; }
    }
}
