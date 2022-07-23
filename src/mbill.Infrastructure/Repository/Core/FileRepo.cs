using mbill.Core.Common.Configs;
using mbill.Core.Domains.Entities.Core;
using mbill.Core.Interface.IRepositories.Core;
using mbill.ToolKits.Qiniu;
using Microsoft.Extensions.Options;
using System.IO;

namespace mbill.Infrastructure.Repository.Core
{
    public class FileRepo : AuditBaseRepo<FileEntity>, IFileRepo
    {
        private readonly QiniuClientOption _qiniuOption;

        public FileRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser, IOptionsMonitor<QiniuClientOption> option) : base(unitOfWorkManager, currentUser)
        {
            _qiniuOption = option.CurrentValue;
        }

        public string GetFileUrl(string path)
        {
            if (string.IsNullOrEmpty(path)) return "";
            if (path.StartsWith("http") || path.StartsWith("https"))//如果是完整地址
            {
                return path;
            }

            var file = Where(r => r.Path == path).First();
            if (file == null) return path;
            return file.Type switch
            {
                1 => _qiniuOption.Host + file.Path,
                _ => file.Path,
            };

        }
    }
}
