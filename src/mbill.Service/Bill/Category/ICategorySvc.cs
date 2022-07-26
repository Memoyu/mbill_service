namespace mbill.Service.Bill.Category;

public interface ICategorySvc
{
    /// <summary>
    /// 新增账单分类
    /// </summary>
    /// <param name="dto">数据源</param>
    /// <returns></returns>
    Task<ServiceResult<CategoryDto>> InsertAsync(CreateCategoryInput dto);

    /// <summary>
    /// 删除账单分类信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ServiceResult> DeleteAsync(long id);

    /// <summary>
    /// 更新账单分类
    /// </summary>
    /// <param name="input">账单分类信息</param>
    /// <returns></returns>
    Task<ServiceResult> EditAsync(EditCategoryInput input);

    /// <summary>
    /// 获取分级后的组合类别数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    Task<ServiceResult<IEnumerable<CategoryGroupDto>>> GetGroupsAsync(int? type);

    /// <summary>
    /// 获取账单分类分页
    /// </summary>
    /// <param name="pagingDto">分页参数</param>
    /// <returns></returns>
    Task<ServiceResult<PagedDto<CategoryPageDto>>> GetPageAsync(CategoryPagingInput pagingDto);


    Task<ServiceResult<IEnumerable<CategoryDto>>> GetListAsync();

    /// <summary>
    /// 获取分类
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ServiceResult<CategoryDto>> GetAsync(long id);

    /// <summary>
    /// 获取群不分类
    /// </summary>
    /// <param name="type">分类类型 0 支出， 1 收入</param>
    /// <returns></returns>
    Task<ServiceResult<List<CategoryDto>>> GetsAsync(int type);

    /// <summary>
    /// 获取父项 by 子项 id
    /// </summary>
    /// <param name="id">分类子项Id</param>
    /// <returns></returns>
    Task<ServiceResult<CategoryDto>> GetParentAsync(long id);

    /// <summary>
    /// 获取账单分类父项集合
    /// </summary>
    /// <returns></returns>
    Task<ServiceResult<IEnumerable<CategoryDto>>> GetParentsAsync();

    /// <summary>
    /// 排序账单分类
    /// </summary>
    /// <param name="input">排序内容</param>
    /// <returns></returns>
    Task<ServiceResult> SortAsync(SortCategoryInput input);
}
