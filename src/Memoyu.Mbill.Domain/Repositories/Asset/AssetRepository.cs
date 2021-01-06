/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Repositories.Asset
*   文件名称 ：AssetRepository.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 21:06:06
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql;
using Memoyu.Mbill.Domain.Entities.Asset;
using Memoyu.Mbill.Domain.IRepositories.Asset;
using Memoyu.Mbill.Domain.Base.Impl;
using Memoyu.Mbill.Domain.Shared.Security;

namespace Memoyu.Mbill.Domain.Repositories.Asset
{
    public class AssetRepository : AuditBaseRepository<AssetEntity>, IAssetRepository
    {
        private readonly ICurrentUser _currentUser;
        public AssetRepository(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
            _currentUser = currentUser;
        }
    }
}
