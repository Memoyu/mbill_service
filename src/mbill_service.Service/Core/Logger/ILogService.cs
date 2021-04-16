using mbill_service.Core.Domains.Common;
using mbill_service.Service.Core.Logger.Input;
using mbill_service.Service.Core.Logger.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbill_service.Service.Core.Logger
{
    public interface ILogService
    {
       /// <summary>
       /// 获取日志分页
       /// </summary>
       /// <param name="pagingDto"></param>
       /// <returns></returns>
        Task<PagedDto<LogDto>> GetPagesAsync(LogPagingDto pagingDto);
    }
}
