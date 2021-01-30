/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Repositories.Core
*   文件名称 ：FileRepository.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-23 19:21:33
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql;
using Memoyu.Mbill.Domain.Base.Impl;
using Memoyu.Mbill.Domain.Entities.Core;
using Memoyu.Mbill.Domain.IRepositories.Core;
using Memoyu.Mbill.Domain.Shared.Configurations;
using Memoyu.Mbill.Domain.Shared.Security;
using Microsoft.Extensions.Options;
using System;

namespace Memoyu.Mbill.Domain.Repositories.Core
{
    public class FileRepository : AuditBaseRepository<FileEntity>, IFileRepository
    {
        private readonly FileStorageOption _fileStorageOption;
        public FileRepository(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser, IOptions<FileStorageOption> fileStorageOption) : base(unitOfWorkManager, currentUser)
        {
            _fileStorageOption = fileStorageOption.Value;
        }

        public string GetFileUrl(string path)
        {
            if (string.IsNullOrEmpty(path)) return "";
            if (path.StartsWith("http") || path.StartsWith("https"))//如果是完整地址
            {
                return path;
            }

            if (path.StartsWith("core"))//如果是本地初始资源
            {
                return _fileStorageOption.LocalFile.Host + path;
            }


            FileEntity file = base.Where(r => r.Path == path).First();
            if (file == null) return path;
            switch (file.Type)
            {
                case 1:
                    return _fileStorageOption.LocalFile.Host + path;
                default:
                    return _fileStorageOption.LocalFile.Host + path;
            }
        }
    }
}
