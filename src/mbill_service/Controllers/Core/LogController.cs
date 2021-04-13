using mbill_service.Core.Domains.Common.Consts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
