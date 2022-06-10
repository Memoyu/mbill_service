namespace mbill.Controllers.Bill;

/// <summary>
/// 账单管理
/// </summary>
[Authorize]
[Route("api/bill")]
[ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
public class BillController : ApiControllerBase
{
    private readonly IBillSvc _billSvc;

    public BillController(IBillSvc billService)
    {
        _billSvc = billService;
    }

    /// <summary>
    /// 新增账单
    /// </summary>
    /// <param name="input">账单</param>
    [HttpPost]
    [LocalAuthorize("新增", "账单")]
    public async Task<ServiceResult<BillSimpleDto>> CreateAsync([FromBody] ModifyBillInput input)
    {
        return await _billSvc.CreateAsync(input);
    }

    /// <summary>
    /// 获取账单详情
    /// </summary>
    /// <param name="id">账单id</param>
    [HttpGet]
    [LocalAuthorize("详情", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<BillDetailDto>> GetAsync([FromQuery] long id)
    {
        return await _billSvc.GetDetailAsync(id);
    }

    /// <summary> 
    /// 删除账单信息
    /// </summary>
    /// <param name="id">账单id</param>
    [HttpDelete]
    [LocalAuthorize("删除", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult> DeleteAsync([FromBody] long id)
    {
        return await _billSvc.DeleteAsync(id);
    }

    /// <summary>
    /// 更新账单信息
    /// </summary>
    /// <param name="input">账单信息</param>
    [HttpPut]
    [LocalAuthorize("更新", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<BillSimpleDto>> UpdateAsync([FromBody] ModifyBillInput input)
    {
        return await _billSvc.UpdateAsync(input);
    }

    /// <summary>
    /// 获取指定日期账单
    /// </summary>
    /// <param name="input">查询条件</param>
    [HttpGet("day")]
    [LocalAuthorize("获取指定日期账单", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<BillsByDayDto>> GetByDayAsync([FromQuery] DayBillInput input)
    {
        return await _billSvc.GetByDayAsync(input);
    }

    /// <summary>
    /// 获取指定月份分组分页账单
    /// </summary>
    /// <param name="input">分页条件</param>
    [HttpGet("month/pages")]
    [LocalAuthorize("获取指定月份分组分页账单", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<PagedDto<BillsByDayDto>>> GetByMonthPagesAsync([FromQuery] MonthBillPagingInput input)
    {
        return await _billSvc.GetByMonthPagesAsync(input);
    }


    /// <summary>
    /// 获取日期范围内存在账单的日期
    /// </summary>
    /// <param name="input">查询入参</param>
    [HttpGet("date/has-bill-days")]
    [LocalAuthorize("获取日期范围内存在账单的日期", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<List<BillDateWithTotalDto>>> RangeHasBillDaysAsync([FromQuery] RangeHasBillDaysInput input)
    {
        return await _billSvc.RangeHasBillDaysAsync(input);
    }

    /// <summary>
    /// 获取指定月份账单总金额
    /// </summary>
    /// <param name="input">入参</param>
    [HttpGet("stat/month-total")]
    [LocalAuthorize("获取指定月份账单总金额", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<MonthTotalStatDto>> GetMonthTotalStatAsync([FromQuery] MonthTotalStatInput input)
    {
        return await _billSvc.GetMonthTotalStatAsync(input);
    }

    /// <summary>
    /// 获取指定日期各类型账单金额统计
    /// </summary>
    /// <param name="input">入参</param>
    [HttpGet("stat/year-total")]
    [LocalAuthorize("获取用户年金额统计", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<YearTotalStatDto>> GetMonthStatisticsAsync([FromQuery] YearTotalStatInput input)
    {
        return await _billSvc.GetYearTotalStatAsync(input);
    }

    /*/// <summary>
    /// 获取指定日期各类型账单金额统计
    /// </summary>
    /// <param name="input">入参</param>
    [HttpGet("stat/total")]
    [LocalAuthorize("获取各类型金额统计", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<BillTotalDto>> GetMonthStatAsync([FromQuery] BillDateInput input)
    {
        return ServiceResult<BillTotalDto>.Successed(await _billService.GetStatisticsTotalAsync(input));
    }

    /// <summary>
    /// 获取指定日期支出分类统计
    /// </summary>
    /// <param name="input">查询入参</param>
    [HttpGet("stat/expend/category")]
    [LocalAuthorize("获取日期内指定分类的数据", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<BillExpendCategoryDto>> GetExpendCategoryStatAsync([FromQuery] BillDateInput input)
    {
        return ServiceResult<BillExpendCategoryDto>.Successed(await _billService.GetExpendCategoryStatisticsAsync(input));
    }

    /// <summary>
    /// 获取当前月份所有周的支出趋势统计
    /// </summary>
    /// <param name="input">查询入参</param>
    [HttpGet("stat/expend/trend/week")]
    [LocalAuthorize("获取周指定分类的趋势统计", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<IEnumerable<BillExpendTrendDto>>> GetWeekExpendTrendStatAsync([FromQuery] BillDateInput input)
    {
        return ServiceResult<IEnumerable<BillExpendTrendDto>>.Successed(await _billService.GetWeekExpendTrendStatisticsAsync(input));
    }

    /// <summary>
    /// 获取当前月往前4个月的支出趋势统计(共5个月)
    /// </summary>
    /// <param name="input">查询入参</param>
    [HttpGet("stat/expend/trend/5month")]
    [LocalAuthorize("获取五个月指定分类的趋势统计", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<IEnumerable<BillExpendTrendDto>>> GetMonthExpendTrendStatAsync([FromQuery] BillDateInput input)
    {
        return ServiceResult<IEnumerable<BillExpendTrendDto>>.Successed(await _billService.GetMonthExpendTrendStatisticsAsync(input, 5));
    }*/

}
