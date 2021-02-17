/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Bill.Asset.Impl
*   文件名称 ：AssetService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 21:06:06
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using AutoMapper;
using Memoyu.Mbill.Application.Base.Impl;
using Memoyu.Mbill.Application.Contracts.Dtos.Bill.Asset;
using Memoyu.Mbill.Application.Contracts.Exceptions;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Entities.Bill.Asset;
using Memoyu.Mbill.Domain.IRepositories.Bill.Asset;
using Memoyu.Mbill.Domain.IRepositories.Core;
using Memoyu.Mbill.ToolKits.Base.Enum.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Bill.Asset.Impl
{
    public class AssetService : ApplicationService, IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapeer;

        public AssetService(IAssetRepository assetRepository, IFileRepository fileRepository, IMapper mapper)
        {
            _assetRepository = assetRepository;
            _fileRepository = fileRepository;
            _mapeer = mapper;
        }


        public async Task<IEnumerable<AssetGroupDto>> GetGroupsAsync(string type)
        {
            List<AssetEntity> entities = await _assetRepository
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
                             s.IconUrl = _fileRepository.GetFileUrl(s.IconUrl);
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
            var asset = await _assetRepository.GetAssetAsync(id) ?? throw new KnownException("资产信息不存在或已删除！", ServiceResultCode.NotFound);
            var parentAsset = await _assetRepository.GetAssetAsync(asset.ParentId) ?? throw new KnownException("资产父项信息不存在或已删除！", ServiceResultCode.NotFound);
            return _mapeer.Map<AssetDto>(parentAsset);

        }

        public async Task<AssetDto> GetAsync(long id)
        {
            var asset = await _assetRepository.GetAssetParentAsync(id) ?? throw new KnownException("资产信息不存在或已删除！", ServiceResultCode.NotFound);
            return _mapeer.Map<AssetDto>(asset);
        }


        public async Task InsertAsync(AssetEntity asset)
        {
            if (!string.IsNullOrEmpty(asset.Name))
            {
                bool isRepeatName = await _assetRepository.Select.AnyAsync(r => r.Name == asset.Name);
                if (isRepeatName)//资产名重复
                {
                    throw new KnownException("资产名称重复，请重新输入", ServiceResultCode.RepeatField);
                }
            }
            await _assetRepository.InsertAsync(asset);
        }
    }
}
