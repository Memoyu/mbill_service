using AutoMapper;
using mbill_service.Controllers.Core;
using mbill_service.Core.AOP.Attributes;
using mbill_service.Core.Domains.Common;
using mbill_service.Core.Domains.Common.Consts;
using mbill_service.Core.Domains.Entities.Bill;
using mbill_service.Service.Bill.Statement;
using mbill_service.Service.Bill.Statement.Input;
using mbill_service.Service.Bill.Statement.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mbill_service.Controllers.Bill
{
    /// <summary>
    /// 账单管理
    /// </summary>
    [Authorize]
    [Route("api/statement")]
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
        [Logger("用户新建了一条账单记录")]
        [HttpPost]
        [LocalAuthorize("新增", "账单")]
        public async Task<ServiceResult<StatementDto>> CreateAsync([FromBody] ModifyStatementDto dto)
        {
            var result = await _statementService.InsertAsync(_mapper.Map<StatementEntity>(dto));
            return ServiceResult<StatementDto>.Successed(result, "账单分类创建成功！");
        }

        /// <summary>
        /// 获取账单详情
        /// </summary>
        /// <param name="id">账单id</param>
        [HttpGet("detail")]
        [LocalAuthorize("获取详情", "账单")]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
        public async Task<ServiceResult<StatementDetailDto>> GetAsync([FromQuery]long id)
        {
            return ServiceResult<StatementDetailDto>.Successed(await _statementService.GetDetailAsync(id));
        }

        /// <summary> 
        /// 删除账单信息
        /// </summary>
        /// <param name="id">账单id</param>
        [HttpDelete]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
        public async Task<ServiceResult> DeleteAsync([FromQuery] long id)
        {
            await _statementService.DeleteAsync(id);
            return ServiceResult.Successed("账单删除成功！");
        }

        /// <summary>
        /// 更新账单信息
        /// </summary>
        /// <param name="dto">账单信息</param>
        [HttpPut]
        [LocalAuthorize("更新", "账单")]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
        public async Task<ServiceResult> UpdateAsync([FromBody] ModifyStatementDto dto)
        {
            await _statementService.UpdateAsync(_mapper.Map<StatementEntity>(dto));
            return ServiceResult.Successed("账单更新成功！");
        }

        /// <summary>
        /// 获取账单分页信息
        /// </summary>
        /// <param name="pagingDto">分页条件</param>
        [HttpGet("pages")]
        [LocalAuthorize("获取分页数据", "账单")]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
        public async Task<ServiceResult<PagedDto<StatementDto>>> GetStatementPagesAsync([FromQuery] StatementPagingDto pagingDto)
        {
            return ServiceResult<PagedDto<StatementDto>>.Successed(await _statementService.GetPagesAsync(pagingDto));
        }

        /// <summary>
        /// 获取指定日期各类型账单金额统计
        /// </summary>
        /// <param name="input">入参</param>
        [HttpGet("statistics/total")]
        [LocalAuthorize("获取各类型金额统计", "账单")]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
        public async Task<ServiceResult<StatementTotalDto>> GetMonthStatisticsAsync([FromQuery] StatementDateInputDto input)
        {
            return ServiceResult<StatementTotalDto>.Successed(await _statementService.GetStatisticsTotalAsync(input));
        }

        /// <summary>
        /// 获取指定日期支出分类统计
        /// </summary>
        /// <param name="input">查询入参</param>
        [HttpGet("statistics/expend/category")]
        [LocalAuthorize("获取日期内指定分类的数据", "账单")]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
        public async Task<ServiceResult<StatementExpendCategoryDto>> GetExpendCategoryStatisticsAsync([FromQuery] StatementDateInputDto input)
        {
            return ServiceResult<StatementExpendCategoryDto>.Successed(await _statementService.GetExpendCategoryStatisticsAsync(input));
        }

        /// <summary>
        /// 获取当前月份所有周的支出趋势统计
        /// </summary>
        /// <param name="input">查询入参</param>
        [HttpGet("statistics/expend/trend/week")]
        [LocalAuthorize("获取周指定分类的趋势统计", "账单")]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
        public async Task<ServiceResult<IEnumerable<StatementExpendTrendDto>>> GetWeekExpendTrendStatisticsAsync([FromQuery] StatementDateInputDto input)
        {
            return ServiceResult<IEnumerable<StatementExpendTrendDto>>.Successed(await _statementService.GetWeekExpendTrendStatisticsAsync(input));
        }

        /// <summary>
        /// 获取当前月往前4个月的支出趋势统计(共5个月)
        /// </summary>
        /// <param name="input">查询入参</param>
        [HttpGet("statistics/expend/trend/5month")]
        [LocalAuthorize("获取五个月指定分类的趋势统计", "账单")]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
        public async Task<ServiceResult<IEnumerable<StatementExpendTrendDto>>> GetMonthExpendTrendStatisticsAsync([FromQuery] StatementDateInputDto input)
        {
            return ServiceResult<IEnumerable<StatementExpendTrendDto>>.Successed(await _statementService.GetMonthExpendTrendStatisticsAsync(input, 5));
        }

    }
}
