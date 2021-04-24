using mbill_service.Core.AOP.Attributes;
using mbill_service.Core.Domains.Common;
using mbill_service.Core.Domains.Common.Consts;
using mbill_service.Service.Core.Logger;
using mbill_service.Service.Core.Logger.Input;
using mbill_service.Service.Core.Logger.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace mbill_service.Controllers.Core
{
    /// <summary>
    /// 日志管理
    /// </summary>
    [Route("api/admin/log")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
    public class LogController : ApiControllerBase
    {
        private readonly ILogService _logService;
        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// 获取日志信息分页
        /// </summary>
        [HttpGet("pages")]
        [LocalAuthorize("获取分页数据", "管理员")]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
        public async Task<ServiceResult<PagedDto<LogDto>>> GetPagesAsync([FromQuery] LogPagingDto pagingDto)
        {
            return ServiceResult<PagedDto<LogDto>>.Successed(await _logService.GetPagesAsync(pagingDto));
        }
    }
}
