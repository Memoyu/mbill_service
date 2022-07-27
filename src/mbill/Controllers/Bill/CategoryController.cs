namespace mbill.Controllers.Bill;

/// <summary>
/// 账单分类管理
/// </summary>
[Authorize]
[Route("api/category")]
public class CategoryController : ApiControllerBase
{
    private readonly ICategorySvc _categorySvc;

    public CategoryController(ICategorySvc categorySvc)
    {
        _categorySvc = categorySvc;
    }

    /// <summary>
    /// 新增账单分组/分类
    /// </summary>
    /// <param name="dto">账单分类</param>
    [HttpPost]
    [Authorize]
    [LocalAuthorize("新增", "账单分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<CategoryDto>> CreateAsync([FromBody] CreateCategoryInput dto)
    {
        return await _categorySvc.InsertAsync(dto); ;
    }

    /// <summary> 
    /// 删除账单分组/分类
    /// </summary>
    /// <param name="id">账单分类id</param>
    [HttpDelete]
    [LocalAuthorize("删除", "账单分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult> DeleteAsync([FromBody] long id)
    {
        return await _categorySvc.DeleteAsync(id);
    }

    /// <summary>
    /// 更新账单分组/分类
    /// </summary>
    /// <param name="input">账单分类信息</param>
    [HttpPut]
    [LocalAuthorize("更新", "账单分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<CategoryDto>> EditAsync([FromBody] EditCategoryInput input)
    {
        return await _categorySvc.EditAsync(input);
    }

    /// <summary>
    /// 获取账单分组/分类
    /// </summary>
    /// <param name="id">分类id</param>
    [HttpGet]
    [LocalAuthorize("获取详情", "账单分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<CategoryDto>> GetAsync([FromQuery] long id)
    {
        return await _categorySvc.GetAsync(id);
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
        return await _categorySvc.GetsAsync(type);
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
        return await _categorySvc.GetParentAsync(id);
    }

    /// <summary>
    /// 获取账单分类父项集合
    /// </summary>
    [HttpGet("parents")]
    [LocalAuthorize("获取父项集合", "账单分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<IEnumerable<CategoryDto>>> GetParentsAsync()
    {
        return await _categorySvc.GetParentsAsync();
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
        return await _categorySvc.GetGroupsAsync(type);
    }

    /// <summary>
    /// 获取账单分类分页
    /// </summary>
    /// <param name="pagingDto">分页参数</param>
    [HttpGet("pages")]
    [LocalAuthorize("获取分页", "管理员")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
    public async Task<ServiceResult<PagedDto<CategoryPageDto>>> GetPageAsync([FromQuery] CategoryPagingInput pagingDto)
    {
        return await _categorySvc.GetPageAsync(pagingDto);
    }

    /// <summary>
    /// 排序账单分类
    /// </summary>
    /// <param name="input">排序信息</param>
    [HttpPost("sort")]
    [Authorize]
    [LocalAuthorize("分类排序", "账单分类")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult> SortAsync([FromBody] SortCategoryInput input)
    {
        if (input.Sorts == null || !input.Sorts.Any())
            return ServiceResult<string>.Failed(ServiceResultCode.ParameterError, "排序内容不能为空");
        return await _categorySvc.SortAsync(input); ;
    }
}
