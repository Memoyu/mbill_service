using Memoyu.Mbill.Domain.Base;

namespace Memoyu.Mbill.Application.Contracts.Dtos.Bill.Statement
{
    public class MapDto
    {
        public string CategoryName { get; set; }

        public string CategoryIconPath { get; set; }

        public string AssetName { get; set; }

        public string TargetAssetName { get; set; }

        public string TypeName { get; set; }
    }
}
