/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.IRepositories.Bill.Asset
*   文件名称 ：IAssetRepository.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 21:06:06
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Entities.Bill.Asset;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Domain.IRepositories.Bill.Asset
{
    public interface IAssetRepository : IAuditBaseRepository<AssetEntity , long>
    {
        /// <summary>
        /// 获取资产信息 By Id
        /// </summary>
        /// <param name="id">资产Id</param>
        /// <returns></returns>
        Task<AssetEntity> GetAssetAsync(long id);

        /// <summary>
        /// 获取资产父项信息 By 子项 Id
        /// </summary>
        /// <param name="id">资产子项Id</param>
        /// <returns></returns>
        Task<AssetEntity> GetAssetParentAsync(long id);
    }
}
