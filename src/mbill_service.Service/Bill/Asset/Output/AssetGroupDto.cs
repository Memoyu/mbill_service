using System.Collections.Generic;

namespace mbill_service.Service.Bill.Asset.Output
{
    public class AssetGroupDto
    {
        public string Name { get; set; }

        public List<AssetDto> Childs { get; set; }
    }
}
