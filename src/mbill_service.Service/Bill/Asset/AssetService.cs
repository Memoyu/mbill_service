using AutoMapper;
using mbill_service.Core.Domains.Common.Enums.Base;
using mbill_service.Core.Domains.Entities.Bill;
using mbill_service.Core.Exceptions;
using mbill_service.Core.Interface.IRepositories.Bill;
using mbill_service.Core.Interface.IRepositories.Core;
using mbill_service.Service.Base;
using mbill_service.Service.Bill.Asset.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbill_service.Service.Bill.Asset
{
    public class AssetService : ApplicationService, IAssetService
    {
        private readonly IAssetRepo _assetRepo;
        private readonly IFileRepo _fileRepo;
        private readonly IMapper _mapeer;

        public AssetService(IAssetRepo assetRepo, IFileRepo fileRepo, IMapper mapper)
        {
            _assetRepo = assetRepo;
            _fileRepo = fileRepo;
            _mapeer = mapper;
        }


        public async Task<IEnumerable<AssetGroupDto>> GetGroupsAsync(string type)
        {
            List<AssetEntity> entities = await _assetRepo
                .Select
                .Where(c => c.IsDeleted == false)
                .WhereIf(type.IsNotNullOrEmpty(), c => c.Type.Equals(type))
                .ToListAsync();
            List<AssetEntity> parents = entities.FindAll(c => c.ParentId == 0);
            List<AssetGroupDto> dtos = parents
                .Select(c =>
                {
                    var dto = new AssetGroupDto();
                    dto.Name = c.Name;
                    dto.Childs = entities
                        .FindAll(d => d.ParentId == c.Id)
                         .Select(d =>
                         {
                             var s = Mapper.Map<AssetDto>(d);
                             s.IconUrl = _fileRepo.GetFileUrl(s.IconUrl);
                             return s;
                         })
                        .ToList();
                    return dto;
                })
                .ToList();
            return dtos;
        }

        public async Task<AssetDto> GetParentAsync(long id)
        {
            var asset = await _assetRepo.GetAssetAsync(id) ?? throw new KnownException("资产信息不存在或已删除！", ServiceResultCode.NotFound);
            var parentAsset = await _assetRepo.GetAssetAsync(asset.ParentId) ?? throw new KnownException("资产父项信息不存在或已删除！", ServiceResultCode.NotFound);
            return _mapeer.Map<AssetDto>(parentAsset);

        }

        public async Task<AssetDto> GetAsync(long id)
        {
            var asset = await _assetRepo.GetAssetParentAsync(id) ?? throw new KnownException("资产信息不存在或已删除！", ServiceResultCode.NotFound);
            return _mapeer.Map<AssetDto>(asset);
        }


        public async Task InsertAsync(AssetEntity asset)
        {
            if (!string.IsNullOrEmpty(asset.Name))
            {
                bool isRepeatName = await _assetRepo.Select.AnyAsync(r => r.Name == asset.Name);
                if (isRepeatName)//资产名重复
                {
                    throw new KnownException("资产名称重复，请重新输入", ServiceResultCode.RepeatField);
                }
            }
            await _assetRepo.InsertAsync(asset);
        }
    }
}
