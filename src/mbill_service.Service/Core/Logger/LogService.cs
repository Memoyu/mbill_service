using mbill_service.Core.Domains.Common;
using mbill_service.Core.Extensions;
using mbill_service.Core.Interface.IRepositories.Core;
using mbill_service.Service.Base;
using mbill_service.Service.Core.Logger.Input;
using mbill_service.Service.Core.Logger.Output;
using System.Linq;
using System.Threading.Tasks;

namespace mbill_service.Service.Core.Logger
{
    public class LogService : ApplicationService, ILogService
    {
        private readonly ILogRepo _logRepo;
        public LogService(ILogRepo logRepo)
        {
            _logRepo = logRepo;
        }
        public async Task<PagedDto<LogDto>> GetPagesAsync(LogPagingDto pagingDto)
        {
            pagingDto.Sort = pagingDto.Sort.IsNullOrEmpty() ? "create_time ASC" : pagingDto.Sort.Replace("-", " ");
            var logs = await _logRepo
                .Select
                .WhereIf(pagingDto.Method.IsNotNullOrWhiteSpace(), l => l.Method.Equals(pagingDto.Method))
                .WhereIf(pagingDto.Username.IsNotNullOrWhiteSpace(), l => l.Username.Contains(pagingDto.Username))
                .WhereIf(pagingDto.UserId > 0, l => l.UserId == pagingDto.UserId)
                .WhereIf(pagingDto.StatusCode > 0, l => l.StatusCode == pagingDto.StatusCode)
                .WhereIf(pagingDto.CreateTime != null, u => u.CreateTime == pagingDto.CreateTime)
                .OrderBy(pagingDto.Sort)
                .ToPageListAsync(pagingDto, out long totalCount);
            var dtos = logs.Select(l => Mapper.Map<LogDto>(l)).ToList();

            return new PagedDto<LogDto>(dtos, totalCount);
        }
    }
}
