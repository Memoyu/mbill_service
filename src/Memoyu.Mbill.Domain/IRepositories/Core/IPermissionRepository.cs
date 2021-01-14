/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.IRepositories.Core
*   文件名称 ：IPermissionRepository.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-15 0:06:57
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Entities.Core;

namespace Memoyu.Mbill.Domain.IRepositories.Core
{
    public interface IPermissionRepository: IAuditBaseRepository<PermissionEntity>
    {
    }
}
