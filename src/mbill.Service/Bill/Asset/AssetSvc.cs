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
        bool isRepeatName = await _assetRepo.Select.AnyAsync(r => r.Name == asset.Name && CurrentUser.Id == r.CreateUserId);
        if (isRepeatName)//资产名重复
            return ServiceResult<AssetDto>.Failed("资产名称重复，请重新输入");
        var entity = await _assetRepo.InsertAsync(asset);
        return ServiceResult<AssetDto>.Successed(Mapper.Map<AssetDto>(entity));
    }

    public async Task<ServiceResult> DeleteAsync(long id)
    {
        var category = await _assetRepo.Select.Where(s => s.Id == id && s.CreateUserId == CurrentUser.Id).ToOneAsync();
        if (category == null) return ServiceResult.Failed(ServiceResultCode.NotFound, "没有找到该资产分类信息");
        var cnt = await _assetRepo.DeleteAsync(id);
        // 如果是分组，则删除分类
        if (cnt > 0 && category.ParentId == 0)
            await _assetRepo.DeleteAsync(c => c.ParentId == id);
        return ServiceResult.Successed();
    }

    public async Task<ServiceResult<AssetDto>> EditAsync(EditAssetInput input)
    {
        var asset = await _assetRepo.Select.Where(s => s.Id == input.Id && !s.IsDeleted).ToOneAsync();
        if (asset == null) return ServiceResult<AssetDto>.Failed(ServiceResultCode.NotFound, "没有找到该资产分类信息");
        asset.Name = input.Name;
        asset.Amount = input.Amount;
        asset.Icon = input.Icon;
        await _assetRepo.UpdateAsync(asset);
        return ServiceResult<AssetDto>.Successed(Mapper.Map<AssetDto>(asset));
    }

    public async Task<ServiceResult<AssetDto>> GetAsync(long id)
    {
        var asset = await _assetRepo.GetAssetAsync(id);
        if (asset == null)
            return ServiceResult<AssetDto>.Failed(ServiceResultCode.ParameterError, "资产信息不存在或已删除！");
        return ServiceResult<AssetDto>.Successed(Mapper.Map<AssetDto>(asset));
    }

    public async Task<ServiceResult<AssetDto>> GetParentAsync(long id)
    {
        var asset = await _assetRepo.GetAssetAsync(id);
        if (asset == null)
            return ServiceResult<AssetDto>.Failed(ServiceResultCode.ParameterError, "资产信息不存在或已删除！");
        var parentAsset = await _assetRepo.GetAssetAsync(asset.ParentId);
        if (parentAsset == null)
            return ServiceResult<AssetDto>.Failed(ServiceResultCode.ParameterError, "资产父项信息不存在或已删除！");
        return ServiceResult<AssetDto>.Successed(Mapper.Map<AssetDto>(parentAsset));

    }

    public async Task<ServiceResult<IEnumerable<AssetDto>>> GetParentsAsync()
    {
        var assets = await _assetRepo
            .Select
            .Where(a => a.ParentId == 0)
            .OrderBy(a => a.CreateTime)
            .ToListAsync();
        var assetDtos = assets.Select(a => Mapper.Map<AssetDto>(a)).ToList();
        return ServiceResult<IEnumerable<AssetDto>>.Successed(assetDtos);
    }

    public async Task<ServiceResult<IEnumerable<AssetGroupDto>>> GetGroupsAsync(int? type)
    {
        List<AssetEntity> entities = await _assetRepo
            .Select
            .Where(c => c.CreateUserId == CurrentUser.Id)
            .WhereIf(type.HasValue, c => c.Type == type)
            .ToListAsync();
        List<AssetEntity> parents = entities.FindAll(c => c.ParentId == 0).OrderByDescending(d => d.Sort).ToList();
        List<AssetGroupDto> dtos = parents
            .Select(c =>
            {
                var dto = new AssetGroupDto();
                dto.Id = c.Id;
                dto.Name = c.Name;
                dto.Childs = entities
                    .FindAll(d => d.ParentId == c.Id)
                     .Select(d =>
                     {
                         return Mapper.Map<AssetDto>(d);
                     }).OrderByDescending(d => d.Sort)
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
        var parentIds = new List<string>();
        if (!string.IsNullOrWhiteSpace(pagingDto.ParentIds))
            parentIds = pagingDto.ParentIds.Split(",").ToList();
        var assets = await _assetRepo
            .Select
            .WhereIf(!string.IsNullOrWhiteSpace(pagingDto.AssetName), a => a.Name.Contains(pagingDto.AssetName))
            .WhereIf(parentIds != null && parentIds.Any(), a => parentIds.Contains(a.ParentId.ToString()))
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
            dto.IconUrl = _fileRepo.GetFileUrl(a.Icon);
            return dto;
        }).ToList();

        return ServiceResult<PagedDto<AssetPageDto>>.Successed(new PagedDto<AssetPageDto>(assetDtos, totalCount));
    }

    public async Task<ServiceResult> SortAsync(SortAssetInput input)
    {
        var edits = input.Sorts.Select(s => new AssetEntity { Id = s.Id, Sort = s.Sort }).ToList();
        var cnt = await _assetRepo.Orm.Update<AssetEntity>().SetSource(edits).UpdateColumns(e => new { e.Sort }).ExecuteAffrowsAsync();
        if (cnt <= 0) return ServiceResult.Failed("排序失败");
        return ServiceResult.Successed();
    }
}
