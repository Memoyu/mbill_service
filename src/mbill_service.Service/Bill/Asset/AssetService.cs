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
using System.Linq.Expressions;
using System.Threading.Tasks;
using mbill_service.Core.Domains.Common;
using mbill_service.Core.Domains.Common.Consts;
using mbill_service.Core.Extensions;
using mbill_service.Service.Bill.Asset.Input;
using Newtonsoft.Json;

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

        public async Task<PagedDto<AssetPageDto>> GetPageAsync(AssetPagingDto pagingDto)
        {
            if (pagingDto.CreateStartTime != null && pagingDto.CreateEndTime == null) throw new KnownException("创建时间参数有误", ServiceResultCode.ParameterError);
            pagingDto.Sort = pagingDto.Sort.IsNullOrEmpty() ? "id ASC" : pagingDto.Sort.Replace("-", " ");
            var assets = await _assetRepo
                .Select
                .WhereIf(!string.IsNullOrWhiteSpace(pagingDto.AssetName), a => a.Name.Contains(pagingDto.AssetName))
                .WhereIf(pagingDto.ParentIds != null && pagingDto.ParentIds.Any(), a => pagingDto.ParentIds.Contains(a.ParentId))
                .WhereIf(!string.IsNullOrWhiteSpace(pagingDto.Type), c => c.Type.Equals(pagingDto.Type))
                .WhereIf(pagingDto.CreateStartTime != null, a => a.CreateTime >= pagingDto.CreateStartTime && a.CreateTime <= pagingDto.CreateEndTime)
                .OrderBy(pagingDto.Sort)
                .ToPageListAsync(pagingDto, out long totalCount);
            var assetDtos = assets.Select(a =>
            {
                var dto = Mapper.Map<AssetPageDto>(a);
                AssetEntity category = null;
                if (a.ParentId != 0)
                    category = _assetRepo.Get(a.ParentId);
                dto.ParentName = category?.Name;
                dto.TypeName = SystemConst.Switcher.AssetType(a.Type);
                dto.IconUrl = _fileRepo.GetFileUrl(a.IconUrl);
                return dto;
            }).ToList();

            return new PagedDto<AssetPageDto>(assetDtos, totalCount);
        }


        public async Task<AssetDto> GetParentAsync(long id)
        {
            var asset = await _assetRepo.GetAssetAsync(id) ?? throw new KnownException("资产信息不存在或已删除！", ServiceResultCode.NotFound);
            var parentAsset = await _assetRepo.GetAssetAsync(asset.ParentId) ?? throw new KnownException("资产父项信息不存在或已删除！", ServiceResultCode.NotFound);
            return _mapeer.Map<AssetDto>(parentAsset);

        }

        public async Task<IEnumerable<AssetDto>> GetParentsAsync()
        {
            var assets = await _assetRepo
                .Select
                .Where(a => a.ParentId == 0)
                .OrderBy(a => a.CreateTime)
                .ToListAsync();
            var assetDtos = assets.Select(a => Mapper.Map<AssetDto>(a)).ToList();
            return assetDtos;
        }

        public async Task<AssetDto> GetAsync(long id)
        {
            var asset = await _assetRepo.GetAssetParentAsync(id) ?? throw new KnownException("资产信息不存在或已删除！", ServiceResultCode.NotFound);
            return _mapeer.Map<AssetDto>(asset);
        }


        public async Task InsertAsync(AssetEntity asset)
        {
            bool isRepeatName = await _assetRepo.Select.AnyAsync(r => r.Name == asset.Name);
            if (isRepeatName)//资产名重复
                throw new KnownException("资产名称重复，请重新输入", ServiceResultCode.RepeatField);
            await _assetRepo.InsertAsync(asset);
        }

        public async Task DeleteAsync(long id)
        {
            var exist = await _assetRepo.Select.AnyAsync(s => s.Id == id && !s.IsDeleted);
            if (!exist) throw new KnownException("没有找到该账单分类信息", ServiceResultCode.NotFound);
            await _assetRepo.DeleteAsync(id);
        }

        public async Task UpdateAsync(AssetEntity asset)
        {
            var exist = await _assetRepo.Select.AnyAsync(s => s.Id == asset.Id && !s.IsDeleted);
            if (!exist) throw new KnownException("没有找到该资产分类信息", ServiceResultCode.NotFound);
            Expression<Func<AssetEntity, object>> ignoreExp = e => new { e.CreateUserId, e.CreateTime };
            await _assetRepo.UpdateWithIgnoreAsync(asset, ignoreExp);
        }
    }
}
