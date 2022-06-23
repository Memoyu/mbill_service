namespace mbill.Controllers.Bill;

/// <summary>
/// 账单分类管理
/// </summary>
[Authorize]
[Route("api/category")]
public class CategoryController : ApiControllerBase
{
    private readonly ICategorySvc _categoryService;
    private readonly IMapper _mapper;

    public CategoryController(ICategorySvc categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }

    /// <summary>
    /// 新增账单分类
    /// </summary>
    /// <param name="dto">账单分类</param>
    [Logger("用户新建了一个账单分类")]
    [HttpPost]
    [Authorize]
    [LocalAuthorize("新增", "账单分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
    public async Task<ServiceResult> CreateAsync([FromBody] ModifyCategoryDto dto)
    {
        await _categoryService.InsertAsync(_mapper.Map<CategoryEntity>(dto));
        return ServiceResult.Successed("账单分类创建成功");
    }

    /// <summary> 
    /// 删除账单分类
    /// </summary>
    /// <param name="id">账单分类id</param>
    [HttpDelete]
    [LocalAuthorize("删除", "账单分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
    public async Task<ServiceResult> DeleteAsync([FromQuery] long id)
    {
        await _categoryService.DeleteAsync(id);
        return ServiceResult.Successed("账单分类删除成功！");
    }

    /// <summary>
    /// 更新账单分类
    /// </summary>
    /// <param name="dto">账单分类信息</param>
    [HttpPut]
    [LocalAuthorize("更新", "账单分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
    public async Task<ServiceResult> UpdateAsync([FromBody] ModifyCategoryDto dto)
    {
        await _categoryService.UpdateAsync(_mapper.Map<CategoryEntity>(dto));
        return ServiceResult.Successed("账单分类更新成功！");
    }

    /// <summary>
    /// 获取分类
    /// </summary>
    /// <param name="id">分类id</param>
    [HttpGet]
    [LocalAuthorize("获取详情", "账单分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<CategoryDto>> GetAsync([FromQuery] long id)
    {
        return ServiceResult<CategoryDto>.Successed(await _categoryService.GetAsync(id));
    }

    /// <summary>
    /// 获取全部分类()
    /// </summary>
    /// <param name="type">分类类型 0 支出， 1 收入</param>
    [HttpGet("list")]
    [LocalAuthorize("获取全部", "账单分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<List<CategoryDto>>> GetsAsync([FromQuery] int type)
    {
        return await _categoryService.GetsAsync(type);
    }


    /// <summary>
    /// 获取分类父项
    /// </summary>
    /// <param name="id">分类id</param>
    [HttpGet("parent")]
    [LocalAuthorize("获取父项详情", "账单分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<CategoryDto>> GetParentAsync([FromQuery] long id)
    {
        return ServiceResult<CategoryDto>.Successed(await _categoryService.GetParentAsync(id));
    }

    /// <summary>
    /// 获取账单分类父项集合
    /// </summary>
    [HttpGet("parents")]
    [LocalAuthorize("获取父项集合", "资产分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<IEnumerable<CategoryDto>>> GetParentsAsync()
    {
        return ServiceResult<IEnumerable<CategoryDto>>.Successed(await _categoryService.GetParentsAsync());
    }

    /// <summary>
    /// 获取分组后的账单分类
    /// </summary>
    /// <param name="type">账单类型</param>
    [HttpGet("groups")]
    [LocalAuthorize("获取分组", "账单分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<IEnumerable<CategoryGroupDto>>> GetGroupAsync([FromQuery] int? type)
    {
        return ServiceResult<IEnumerable<CategoryGroupDto>>.Successed(await _categoryService.GetGroupsAsync(type));
    }

    /// <summary>
    /// 获取账单分类分页
    /// </summary>
    /// <param name="pagingDto">分页参数</param>
    [HttpGet("pages")]
    [LocalAuthorize("获取分页", "管理员")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
    public async Task<ServiceResult<PagedDto<CategoryPageDto>>> GetPageAsync([FromQuery] CategoryPagingDto pagingDto)
    {
        return ServiceResult<PagedDto<CategoryPageDto>>.Successed(await _categoryService.GetPageAsync(pagingDto));
    }
}
