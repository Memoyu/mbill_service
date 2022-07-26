namespace mbill.Service.Bill.Asset;

public class AssetSvc : ApplicationSvc, IAssetSvc
{
    private readonly IAssetRepo _assetRepo;
    private readonly IFileRepo _fileRepo;
    private readonly IMapper _mapeer;

    public AssetSvc(IAssetRepo assetRepo, IFileRepo fileRepo, IMapper mapper)
    {
        _assetRepo = assetRepo;
        _fileRepo = fileRepo;
        _mapeer = mapper;
    }


    public async Task<ServiceResult<IEnumerable<AssetGroupDto>>> GetGroupsAsync(int? type)
    {
        List<AssetEntity> entities = await _assetRepo
            .Select
            .Where(c => c.CreateUserId == CurrentUser.Id)
            .WhereIf(type.HasValue, c => c.Type == type)
            .ToListAsync();
        List<AssetEntity> parents = entities.FindAll(c => c.ParentId == 0).OrderBy(d => d.Sort).ToList();
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
                         var s = Mapper.Map<AssetDto>(d);
                         s.IconUrl = _fileRepo.GetFileUrl(d.Icon);
                         return s;
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


    public async Task<ServiceResult<AssetDto>> GetParentAsync(long id)
    {
        var asset = await _assetRepo.GetAssetAsync(id);
        if (asset == null)
            return ServiceResult<AssetDto>.Failed(ServiceResultCode.ParameterError, "资产信息不存在或已删除！");
        var parentAsset = await _assetRepo.GetAssetAsync(asset.ParentId) ?? throw new KnownException("资产父项信息不存在或已删除！", ServiceResultCode.NotFound);
        return ServiceResult<AssetDto>.Successed(_mapeer.Map<AssetDto>(parentAsset));

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

    public async Task<ServiceResult<AssetDto>> GetAsync(long id)
    {
        var asset = await _assetRepo.GetAssetParentAsync(id);
        if (asset == null)
            return ServiceResult<AssetDto>.Failed(ServiceResultCode.ParameterError, "资产信息不存在或已删除！");
        return ServiceResult<AssetDto>.Successed(_mapeer.Map<AssetDto>(asset));
    }


    public async Task<ServiceResult> InsertAsync(AssetEntity asset)
    {
        bool isRepeatName = await _assetRepo.Select.AnyAsync(r => r.Name == asset.Name);
        if (isRepeatName)//资产名重复
            return ServiceResult.Failed(ServiceResultCode.ParameterError, "资产名称重复，请重新输入");
        await _assetRepo.InsertAsync(asset);
        return ServiceResult.Successed();
    }

    public async Task<ServiceResult> DeleteAsync(long id)
    {
        var exist = await _assetRepo.Select.AnyAsync(s => s.Id == id && !s.IsDeleted);
        if (!exist) throw new KnownException("没有找到该账单分类信息", ServiceResultCode.NotFound);
        await _assetRepo.DeleteAsync(id);
        return ServiceResult.Successed();
    }

    public async Task<ServiceResult> EditAsync(AssetEntity asset)
    {
        var exist = await _assetRepo.Select.AnyAsync(s => s.Id == asset.Id && !s.IsDeleted);
        if (!exist) return ServiceResult.Failed(ServiceResultCode.ParameterError, "没有找到该资产分类信息");
        Expression<Func<AssetEntity, object>> ignoreExp = e => new { e.CreateUserId, e.CreateTime };
        await _assetRepo.UpdateWithIgnoreAsync(asset, ignoreExp);
        return ServiceResult.Successed();
    }
}
