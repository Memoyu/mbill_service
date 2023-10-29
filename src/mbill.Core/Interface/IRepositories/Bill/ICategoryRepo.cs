namespace Mbill.Core.Interface.IRepositories.Bill;

public interface ICategoryRepo : IAuditBaseRepo<CategoryEntity>
{
    /// <summary>
    /// 获取分类信息 By bId
    /// </summary>
    /// <param name="bId"></param>
    /// <returns></returns>
    Task<CategoryEntity> GetCategoryAsync(long bId);
}
