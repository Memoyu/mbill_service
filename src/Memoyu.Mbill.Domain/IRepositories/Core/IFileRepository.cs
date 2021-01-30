/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.IRepositories.Core
*   文件名称 ：IFileRepository.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-23 19:20:59
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/

using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Entities.Core;
using System;

namespace Memoyu.Mbill.Domain.IRepositories.Core
{
    public interface IFileRepository : IAuditBaseRepository<FileEntity>
    {
        string GetFileUrl(string path);
    }
}
