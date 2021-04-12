using mbill_service.Core.Common.Configs;
using mbill_service.Core.Domains.Entities.Core;
using mbill_service.Core.Interface.IRepositories.Core;
using mbill_service.Core.Security;
using mbill_service.Infrastructure.Repository.Base;
using FreeSql;
using Microsoft.Extensions.Options;

namespace mbill_service.Infrastructure.Repository.Core
{
    public class FileRepo : AuditBaseRepo<FileEntity>, IFileRepo
    {
        public FileRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
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
                return Appsettings.FileStorage.LocalFileHost + path;
            }


            FileEntity file = base.Where(r => r.Path == path).First();
            if (file == null) return path;
            switch (file.Type)
            {
                case 1:
                    return Appsettings.FileStorage.LocalFileHost + path;
                default:
                    return Appsettings.FileStorage.LocalFileHost + path;
            }
        }
    }
}
