﻿using Mbill.Core.Common;

namespace Mbill.Service.Bill.Asset;

public class AssetSvc : ApplicationSvc, IAssetSvc
{
    private readonly IAssetRepo _assetRepo;
    private readonly IFileRepo _fileRepo;

    public AssetSvc(IAssetRepo assetRepo, IFileRepo fileRepo)
    {
        _assetRepo = assetRepo;
        _fileRepo = fileRepo;
    }

    public async Task<ServiceResult<AssetDto>> InsertAsync(CreateAssetInput input)
    {
        var asset = Mapper.Map<AssetEntity>(input);
        bool isRepeatName = await _assetRepo.Select.AnyAsync(r => r.Name == asset.Name && CurrentUser.BId == r.CreateUserBId);
        if (isRepeatName)//资产名重复
            return ServiceResult<AssetDto>.Failed("资产名称重复，请重新输入");
        asset.BId = SnowFlake.NextId();
        var entity = await _assetRepo.InsertAsync(asset);
        return ServiceResult<AssetDto>.Successed(Mapper.Map<AssetDto>(entity));
    }

    public async Task<ServiceResult> DeleteAsync(long bId)
    {
        var category = await _assetRepo.Select.Where(s => s.BId == bId && s.CreateUserBId == CurrentUser.BId).ToOneAsync();
        if (category == null) return ServiceResult.Failed(ServiceResultCode.NotFound, "没有找到该资产分类信息");
        var cnt = await _assetRepo.DeleteAsync(category);
        // 如果是分组，则删除分类
        if (cnt > 0 && category.ParentBId == 0)
            await _assetRepo.DeleteAsync(c => c.ParentBId == bId);
        return ServiceResult.Successed();
    }

    public async Task<ServiceResult<AssetDto>> EditAsync(EditAssetInput input)
    {
        var asset = await _assetRepo.Select.Where(s => s.BId == input.BId && !s.IsDeleted).ToOneAsync();
        if (asset == null) return ServiceResult<AssetDto>.Failed(ServiceResultCode.NotFound, "没有找到该资产分类信息");
        asset.Name = input.Name;
        asset.Amount = input.Amount;
        asset.Icon = input.Icon;
        await _assetRepo.UpdateAsync(asset);
        return ServiceResult<AssetDto>.Successed(Mapper.Map<AssetDto>(asset));
    }

    public async Task<ServiceResult<AssetDto>> GetAsync(long bId)
    {
        var asset = await _assetRepo.GetAssetAsync(bId);
        if (asset == null)
            return ServiceResult<AssetDto>.Failed(ServiceResultCode.ParameterError, "资产信息不存在或已删除！");
        return ServiceResult<AssetDto>.Successed(Mapper.Map<AssetDto>(asset));
    }

    public async Task<ServiceResult<AssetDto>> GetParentAsync(long bId)
    {
        var asset = await _assetRepo.GetAssetAsync(bId);
        if (asset == null)
            return ServiceResult<AssetDto>.Failed(ServiceResultCode.ParameterError, "资产信息不存在或已删除！");
        var parentAsset = await _assetRepo.GetAssetAsync(asset.ParentBId);
        if (parentAsset == null)
            return ServiceResult<AssetDto>.Failed(ServiceResultCode.ParameterError, "资产父项信息不存在或已删除！");
        return ServiceResult<AssetDto>.Successed(Mapper.Map<AssetDto>(parentAsset));

    }

    public async Task<ServiceResult<IEnumerable<AssetDto>>> GetParentsAsync()
    {
        var assets = await _assetRepo
            .Select
            .Where(a => a.ParentBId == 0)
            .OrderBy(a => a.CreateTime)
            .ToListAsync();
        var assetDtos = assets.Select(Mapper.Map<AssetDto>).ToList();
        return ServiceResult<IEnumerable<AssetDto>>.Successed(assetDtos);
    }

    public async Task<ServiceResult<IEnumerable<AssetGroupDto>>> GetGroupsAsync(int? type)
    {
        List<AssetEntity> entities = await _assetRepo
            .Select
            .Where(c => c.CreateUserBId == CurrentUser.BId)
            .WhereIf(type.HasValue, c => c.Type == type)
            .ToListAsync();
        List<AssetEntity> parents = entities.FindAll(c => c.ParentBId == 0).OrderByDescending(d => d.Sort).ToList();
        List<AssetGroupDto> dtos = parents
            .Select(c =>
            {
                var dto = new AssetGroupDto();
                dto.BId = c.BId;
                dto.Name = c.Name;
                dto.Childs = entities
                    .FindAll(d => d.ParentBId == c.BId)
                     .Select(Mapper.Map<AssetDto>).OrderByDescending(d => d.Sort)
                    .ToList();
                return dto;
            })
            .ToList();
        return ServiceResult<IEnumerable<AssetGroupDto>>.Successed(dtos);
    }

    public async Task<ServiceResult<PagedDto<AssetPageDto>>> GetPageAsync(AssetPagingDto pagingDto)
    {
        if (pagingDto.CreateStartTime != null && pagingDto.CreateEndTime == null)
            return ServiceResult<PagedDto<AssetPageDto>>.Failed(ServiceResultCode.ParameterError, "创建时间参数有误");
        pagingDto.Sort = pagingDto.Sort.IsNullOrEmpty() ? "id ASC" : pagingDto.Sort.Replace("-", " ");
        var parentBIds = new List<string>();
        if (!string.IsNullOrWhiteSpace(pagingDto.ParentBIds))
            parentBIds = pagingDto.ParentBIds.Split(",").ToList();
        var assets = await _assetRepo
            .Select
            .Include(a => a.Parent)
            .WhereIf(!string.IsNullOrWhiteSpace(pagingDto.AssetName), a => a.Name.Contains(pagingDto.AssetName))
            .WhereIf(parentBIds != null && parentBIds.Any(), a => parentBIds.Contains(a.ParentBId.ToString()))
            .WhereIf(!string.IsNullOrWhiteSpace(pagingDto.Type), c => c.Type.Equals(pagingDto.Type))
            .WhereIf(pagingDto.CreateStartTime != null, a => a.CreateTime >= pagingDto.CreateStartTime && a.CreateTime <= pagingDto.CreateEndTime)
            .OrderBy(pagingDto.Sort)
            .ToPageListAsync(pagingDto, out long totalCount);

        var assetDtos = Mapper.Map<List<AssetPageDto>>(assets).ToList();
        return ServiceResult<PagedDto<AssetPageDto>>.Successed(new PagedDto<AssetPageDto>(assetDtos, totalCount));
    }

    public async Task<ServiceResult> SortAsync(SortAssetInput input)
    {
        var edits = input.Sorts.Select(s => new AssetEntity { BId = s.BId, Sort = s.Sort }).ToList();
        var cnt = await _assetRepo.Orm.Update<AssetEntity>().SetSource(edits, e => e.BId).UpdateColumns(e => new { e.Sort }).ExecuteAffrowsAsync();
        if (cnt <= 0) return ServiceResult.Failed("排序失败");
        return ServiceResult.Successed();
    }
}
