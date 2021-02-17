/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Asset
*   文件名称 ：AssetService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 21:05:24
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Application.Contracts.Dtos.Bill.Asset;
using Memoyu.Mbill.Domain.Entities.Bill.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Bill.Asset
{
    public interface IAssetService
    {
        /// <summary>
        /// 新增资产分类
        /// </summary>
        /// <param name="input">数据源</param>
        /// <returns></returns>
        Task InsertAsync(AssetEntity asset);

        /// <summary>
        /// 获取分级后的组合类别数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<IEnumerable<AssetGroupDto>> GetGroupsAsync(string type);

        /// <summary>
        /// 获取父项 by id
        /// </summary>
        /// <param name="id">资产Id</param>
        /// <returns></returns>
        Task<AssetDto> GetAsync(long id);

        /// <summary>
        /// 获取父项 by 子项 id
        /// </summary>
        /// <param name="id">资产子项Id</param>
        /// <returns></returns>
        Task<AssetDto> GetParentAsync(long id);
    }
}
