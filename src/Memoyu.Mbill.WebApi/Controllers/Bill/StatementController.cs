using AutoMapper;
using Memoyu.Mbill.Application.Bill.Statement;
using Memoyu.Mbill.Application.Contracts.Attributes;
using Memoyu.Mbill.Application.Contracts.Dtos.Bill.Statement;
using Memoyu.Mbill.Domain.Entities.Bill.Statement;
using Memoyu.Mbill.Domain.Shared.Const;
using Memoyu.Mbill.ToolKits.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Memoyu.Mbill.WebApi.Controllers.Bill
{
    /// <summary>
    /// 账单管理
    /// </summary>
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public class StatementController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStatementService _statementService;

        public StatementController(IStatementService statementService,IMapper mapper)
        {
            _mapper = mapper;
            _statementService = statementService;
        }

        /// <summary>
        /// 新增账单
        /// </summary>
        /// <param name="dto">账单</param>
        [Logger("用户新建了一个账单分类")]
        [HttpPost("create")]
        [Authorize]
        public async Task<ServiceResult> CreateAsync([FromBody] ModifyStatementDto dto)
        {
            await _statementService.InsertAsync(_mapper.Map<StatementEntity>(dto));
            return ServiceResult.Successed("账单分类创建成功");
        }
    }
}
