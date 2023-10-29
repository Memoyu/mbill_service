namespace Mbill.Core.Interface.IRepositories.Bill;

public interface ICategoryRepo : IAuditBaseRepo<CategoryEntity>
{
    /// <summary>
    /// 获取分类信息 By bId
    /// </summary>
    /// <param name="bId"></param>
    /// <returns></returns>
    Task<CategoryEntity> GetCategoryAsync(long bId);

    /// <summary>
    /// 获取分类父项 By 子项 bId
    /// </summary>
    /// <param name="bId">分类子项bId</param>
    /// <returns></returns>
    Task<CategoryEntity> GetCategoryParentAsync(long bId);

}
