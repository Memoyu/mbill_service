using Mbill.Core.Common.Configs;
using Mbill.Core.Domains.Entities.Core;
using Mbill.Core.Interface.IRepositories.Core;
using Microsoft.Extensions.Options;

namespace Mbill.Infrastructure.Repository.Core
{
    public class FileRepo : AuditBaseRepo<FileEntity>, IFileRepo
    {
        private readonly FileStorageOptions _fileStorageOption;

        public FileRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser, IOptionsMonitor<FileStorageOptions> option) : base(unitOfWorkManager, currentUser)
        {
            _fileStorageOption = option.CurrentValue ?? throw new ArgumentNullException("FileStorage 配置为空");
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
                1 => _fileStorageOption?.Qiniu?.Host + file.Path,
                99 => _fileStorageOption?.Local?.Host,
                _ => file.Path,
            };

        }
    }
}
