using mbill.Core.Common.Configs;
using mbill.Core.Domains.Entities.Core;
using mbill.Core.Interface.IRepositories.Core;
using mbill.ToolKits.Qiniu;
using Microsoft.Extensions.Options;

namespace mbill.Infrastructure.Repository.Core
{
    public class FileRepo : AuditBaseRepo<FileEntity>, IFileRepo
    {
        private readonly QiniuClientOption _qiniuOption;

        public FileRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser, IOptionsMonitor<QiniuClientOption> option) : base(unitOfWorkManager, currentUser)
        {
            _qiniuOption = option.CurrentValue;
        }

        public string GetFileUrl(string md5)
        {
            if (string.IsNullOrEmpty(md5)) return "";
            if (md5.StartsWith("http") || md5.StartsWith("https"))//如果是完整地址
            {
                return md5;
            }

            var file = Where(r => r.Md5 == md5).First();
            if (file == null) return md5;
            return file.Type switch
            {
                1 => _qiniuOption.Host + file.Path,
                _ => file.Path,
            };

        }
    }
}
