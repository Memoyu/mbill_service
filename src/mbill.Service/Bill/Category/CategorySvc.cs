using Mbill.Core.Common;
using System.Collections.Generic;

namespace Mbill.Service.Bill.Category;

public class CategorySvc : ApplicationSvc, ICategorySvc
{
    private readonly ICategoryRepo _categoryRepo;
    private readonly IFileRepo _fileRepo;

    public CategorySvc(ICategoryRepo categoryRepo, IFileRepo fileRepo)
    {
        _categoryRepo = categoryRepo;
        _fileRepo = fileRepo;
    }

    public async Task<ServiceResult<CategoryDto>> InsertAsync(CreateCategoryInput input)
    {
        var categroy = Mapper.Map<CategoryEntity>(input);
        bool isRepeatName = await _categoryRepo.Select.AnyAsync(r => r.Name == categroy.Name && CurrentUser.BId == r.CreateUserBId);
        if (isRepeatName)//分类名重复
            return ServiceResult<CategoryDto>.Failed("分类名称重复，请重新输入");

        categroy.BId = SnowFlake.NextId();
        var entity = await _categoryRepo.InsertAsync(categroy);
        return ServiceResult<CategoryDto>.Successed(Mapper.Map<CategoryDto>(entity));
    }

    public async Task<ServiceResult> DeleteAsync(long bId)
    {
        var category = await _categoryRepo.Select.Where(s => s.Id == bId && s.CreateUserBId == CurrentUser.BId).ToOneAsync();
        if (category == null) return ServiceResult.Failed(ServiceResultCode.NotFound, "没有找到该账单分类信息");
        var cnt = await _categoryRepo.DeleteAsync(category);
        // 如果是分组，则删除分类
        if (cnt > 0 && category.ParentBId == 0)
            await _categoryRepo.DeleteAsync(c => c.ParentBId == bId);
        return ServiceResult.Successed();
    }

    public async Task<ServiceResult<CategoryDto>> EditAsync(EditCategoryInput input)
    {
        var category = await _categoryRepo.Select.Where(s => s.BId == input.BId && !s.IsDeleted).ToOneAsync();
        if (category == null) return ServiceResult<CategoryDto>.Failed(ServiceResultCode.NotFound, "没有找到该账单分类信息");
        category.Name = input.Name;
        category.Budget = input.Budget;
        category.Icon = input.Icon;
        await _categoryRepo.UpdateAsync(category);
        return ServiceResult<CategoryDto>.Successed(Mapper.Map<CategoryDto>(category));
    }

    public async Task<ServiceResult<CategoryDto>> GetAsync(long bId)
    {
        var category = await _categoryRepo.GetCategoryAsync(bId);
        if (category == null)
            return ServiceResult<CategoryDto>.Failed(ServiceResultCode.NotFound, "分类信息不存在或已删除！");
        return ServiceResult<CategoryDto>.Successed(Mapper.Map<CategoryDto>(category));
    }

    public async Task<ServiceResult<List<CategoryDto>>> GetsAsync(int type)
    {
        var categories = await _categoryRepo.Select.Where(c => c.ParentBId != 0 && c.Type == type && c.CreateUserBId == CurrentUser.BId).ToListAsync();
        var dtos = categories.Select(Mapper.Map<CategoryDto>).ToList();
        return ServiceResult<List<CategoryDto>>.Successed(dtos);
    }

    public async Task<ServiceResult<CategoryDto>> GetParentAsync(long bId)
    {
        var category = await _categoryRepo.GetCategoryAsync(bId);
        if (category == null)
            return ServiceResult<CategoryDto>.Failed(ServiceResultCode.NotFound, "分类信息不存在或已删除！");
        var categoryParent = await _categoryRepo.GetCategoryAsync(category.ParentBId);
        if (categoryParent == null)
            return ServiceResult<CategoryDto>.Failed(ServiceResultCode.NotFound, "分类父项信息不存在或已删除！");
        return ServiceResult<CategoryDto>.Successed(Mapper.Map<CategoryDto>(categoryParent));
    }

    public async Task<ServiceResult<IEnumerable<CategoryDto>>> GetParentsAsync()
    {
        var assets = await _categoryRepo
            .Select
            .Where(a => a.ParentBId == 0)
            .OrderBy(a => a.CreateTime)
            .ToListAsync();
        var categoryDtos = assets.Select(Mapper.Map<CategoryDto>).ToList();
        return ServiceResult<IEnumerable<CategoryDto>>.Successed(categoryDtos);
    }

    public async Task<ServiceResult<IEnumerable<CategoryGroupDto>>> GetGroupsAsync(int? type)
    {
        List<CategoryEntity> entities = await _categoryRepo
            .Select
            .Where(c => c.CreateUserBId == CurrentUser.BId)
            .WhereIf(type.HasValue, c => c.Type == type)
            .ToListAsync();
        List<CategoryEntity> parents = entities.FindAll(c => c.ParentBId == 0).OrderByDescending(d => d.Sort).ToList();
        List<CategoryGroupDto> dtos = parents
            .Select(c =>
            {
                var dto = new CategoryGroupDto();
                dto.BId = c.BId;
                dto.Name = c.Name;
                dto.Childs = entities
                    .FindAll(d => d.ParentBId == c.BId)
                    .Select(Mapper.Map<CategoryDto>).OrderByDescending(d => d.Sort)
                    .ToList();
                return dto;
            })
            .ToList();
        return ServiceResult<IEnumerable<CategoryGroupDto>>.Successed(dtos);
    }

    public async Task<ServiceResult<PagedDto<CategoryPageDto>>> GetPageAsync(CategoryPagingInput pagingDto)
    {
        if (pagingDto.CreateStartTime != null && pagingDto.CreateEndTime == null) throw new KnownException("创建时间参数有误", ServiceResultCode.ParameterError);
        var parentBIds = new List<string>();
        if (!string.IsNullOrWhiteSpace(pagingDto.ParentBIds))
            parentBIds = pagingDto.ParentBIds.Split(",").ToList();
        pagingDto.Sort = pagingDto.Sort.IsNullOrEmpty() ? "id ASC" : pagingDto.Sort.Replace("-", " ");
        var categories = await _categoryRepo
            .Select
            .WhereIf(!string.IsNullOrWhiteSpace(pagingDto.CategoryName), c => c.Name.Contains(pagingDto.CategoryName))
            .WhereIf(parentBIds != null && parentBIds.Any(), c => parentBIds.Contains(c.ParentBId.ToString()))
            .WhereIf(!string.IsNullOrWhiteSpace(pagingDto.Type), c => c.Type.Equals(pagingDto.Type))
            .WhereIf(pagingDto.CreateStartTime != null, c => c.CreateTime >= pagingDto.CreateStartTime && c.CreateTime <= pagingDto.CreateEndTime)
            .OrderBy(pagingDto.Sort)
            .ToPageListAsync(pagingDto, out long totalCount);
        var categoryDtos = categories.Select(async c =>
        {
            var dto = Mapper.Map<CategoryPageDto>(c);
            CategoryEntity category = null;
            if (c.ParentBId != 0)
                category = await _categoryRepo.GetCategoryAsync(c.ParentBId);
            dto.ParentName = category?.Name;
            dto.TypeName = Switcher.CategoryType(c.Type);
            dto.IconUrl = _fileRepo.GetFileUrl(c.Icon);
            return dto;
        }).Select(t => t.Result).ToList();

        return ServiceResult<PagedDto<CategoryPageDto>>.Successed(new PagedDto<CategoryPageDto>(categoryDtos, totalCount));
    }

    public async Task<ServiceResult> SortAsync(SortCategoryInput input)
    {
        var edits = input.Sorts.Select(s => new CategoryEntity { BId = s.BId, Sort = s.Sort }).ToList();
        var cnt = await _categoryRepo.Orm.Update<CategoryEntity>().SetSource(edits, e => e.BId).UpdateColumns(e => new { e.Sort }).ExecuteAffrowsAsync();
        if (cnt <= 0) return ServiceResult.Failed("排序失败");
        return ServiceResult.Successed();
    }
}
