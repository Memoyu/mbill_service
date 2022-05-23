namespace mbill.Core.Interface.IRepositories.Bill;

public interface ICategoryRepo : IAuditBaseRepo<CategoryEntity>
{
    /// <summary>
    /// 获取分类信息 By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<CategoryEntity> GetCategoryAsync(long id);

    /// <summary>
    /// 获取分类父项 By 子项 Id
    /// </summary>
    /// <param name="id">分类子项Id</param>
    /// <returns></returns>
    Task<CategoryEntity> GetCategoryParentAsync(long id);

}
