using AutoMapper;
using mbill_service.Core.Domains.Entities.Bill;
using mbill_service.Service.Bill.Asset.Input;
using mbill_service.Service.Bill.Asset.Output;

namespace mbill_service.Service.Common.Mapper.Bill
{
    public class AssetMapper : Profile
    {
        public AssetMapper()
        {
            CreateMap<ModifyAssetDto, AssetEntity>();
            CreateMap<AssetEntity, AssetDto>();
            CreateMap<AssetEntity, AssetPageDto>();
        }
    }
}
