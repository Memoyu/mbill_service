using mbill_service.Core.Domains.Common;
using mbill_service.Core.Domains.Common.Enums.Base;
using mbill_service.Core.Exceptions;
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
            if (pagingDto.CreateStartTime != null && pagingDto.CreateEndTime == null) throw new KnownException("创建时间参数有误", ServiceResultCode.ParameterError);
            pagingDto.Sort = pagingDto.Sort.IsNullOrEmpty() ? "create_time ASC" : pagingDto.Sort.Replace("-", " ");
            var logs = await _logRepo
                .Select
                .WhereIf(pagingDto.Method.IsNotNullOrWhiteSpace(), l => l.Method.Equals(pagingDto.Method.ToUpper()))
                .WhereIf(pagingDto.Username.IsNotNullOrWhiteSpace(), l => l.Username.Contains(pagingDto.Username))
                .WhereIf(!string.IsNullOrWhiteSpace(pagingDto.UserId) && long.TryParse(pagingDto.UserId, out _), l => l.UserId == long.Parse(pagingDto.UserId))
                .WhereIf(!string.IsNullOrWhiteSpace(pagingDto.StatusCode) && int.TryParse(pagingDto.StatusCode, out _), l => l.StatusCode == int.Parse(pagingDto.StatusCode))
                .WhereIf(pagingDto.CreateStartTime != null, a => a.CreateTime >= pagingDto.CreateStartTime && a.CreateTime <= pagingDto.CreateEndTime)
                .OrderBy(pagingDto.Sort)
                .ToPageListAsync(pagingDto, out long totalCount);
            var dtos = logs.Select(l => Mapper.Map<LogDto>(l)).ToList();

            return new PagedDto<LogDto>(dtos, totalCount);
        }
    }
}
