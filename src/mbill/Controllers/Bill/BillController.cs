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
    /// 检索指定条件账单
    /// </summary>
    /// <param name="input">检索条件</param>
    [HttpPost("search")]
    [LocalAuthorize("检索指定条件账单", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<PagedDto<BillSimpleDto>>> GetSearchPagesAsync([FromBody] BillSearchPagingInput input)
    {
        return await _billSvc.SearchPagesAsync(input);
    }

    /// <summary>
    /// 获取账单检索记录
    /// </summary>
    /// <returns></returns>
    [HttpGet("search/records")]
    [LocalAuthorize("获取账单检索记录", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<List<BillSearchRecordOutput>>> GetSearchRecordsAsync()
    {
        return await _billSvc.GetSearchRecordsAsync();
    }

    /// <summary>
    /// 获取指定条件分页账单
    /// </summary>
    /// <param name="input">分页条件</param>
    [HttpGet("pages")]
    [LocalAuthorize("获取指定条件分页账单", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<PagedDto<BillSimpleDto>>> GetByMonthPagesAsync([FromQuery] BillPagingInput input)
    {
        return await _billSvc.GetPagesAsync(input);
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
    /// 获取指定日期账单
    /// </summary>
    /// <param name="input">查询条件</param>
    [HttpGet("day")]
    [LocalAuthorize("获取指定日期账单", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<BillsByDayWithStatDto>> GetByDayAsync([FromQuery] DayBillInput input)
    {
        return await _billSvc.GetByDayAsync(input);
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
    /// 获取指定年份账单金额统计
    /// </summary>
    /// <param name="input">入参</param>
    [HttpGet("stat/year-total")]
    [LocalAuthorize("获取指定年份账单总金额", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<YearTotalStatDto>> GetYearTotalStatAsync([FromQuery] YearTotalStatInput input)
    {
        return await _billSvc.GetYearTotalStatAsync(input);
    }


    /// <summary>
    /// 获取指定年份的收支结余统计
    /// </summary>
    /// <param name="year">年份</param>
    [HttpGet("stat/year-surplus")]
    [LocalAuthorize("获取指定年份收支结余统计", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<List<YearSurplusStatDto>>> GetYearSurplusStatAsync([FromQuery] int year)
    {
        return await _billSvc.GetYearSurplusStatAsync(year);
    }

    /// <summary>
    /// 获取指定月份账单金额趋势统计
    /// </summary>
    /// <param name="input">查询入参</param>
    [HttpGet("stat/trend/month-total")]
    [LocalAuthorize("获取指定月份账单金额趋势", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<MonthTotalTrendStatDto>> GetMonthTotalTrendStatAsync([FromQuery] MonthTotalTrendStatInput input)
    {
        return await _billSvc.GetMonthTotalTrendStatAsync(input);
    }

    /// <summary>
    /// 获取指定年份账单金额趋势统计
    /// </summary>
    /// <param name="input">查询入参</param>
    [HttpGet("stat/trend/year-total")]
    [LocalAuthorize("获取指定年份账单金额趋势", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<YearTotalTrendStatDto>> GetYearTotalTrendStatAsync([FromQuery] YearTotalTrendStatInput input)
    {
        return await _billSvc.GetYearTotalTrendStatAsync(input);
    }

    /// <summary>
    /// 获取指定日期的收入或支出分类占比统计
    /// </summary>
    /// <param name="input">查询入参</param>
    [HttpGet("stat/percent/category")]
    [LocalAuthorize("获取日期内分类占比", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<CategoryPercentStatDto>> GetCategoryPercentStatAsync([FromQuery] CategoryPercentStatInput input)
    {
        return await _billSvc.GetCategoryPercentStatAsync(input);
    }

    /// <summary>
    /// 获取指定日期的收入或支出分类占比分组列表
    /// </summary>
    /// <param name="input">查询入参</param>
    [HttpGet("stat/percent/category/group")]
    [LocalAuthorize("获取日期内分类占比分组", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<List<CategoryPercentGroupDto>>> GetCategoryPercentGroupAsync([FromQuery] CategoryPercentGroupInput input)
    {
        return await _billSvc.GetCategoryPercentGroupAsync(input);
    }

    /// <summary>
    /// 获取指定条件账单排行列表
    /// </summary>
    /// <param name="input">查询入参</param>
    [HttpGet("stat/ranking")]
    [LocalAuthorize("获取账单排行列表", "账单")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<PagedDto<BillSimpleDto>>> GetRankingAsync([FromQuery] RankingPagingInput input)
    {
        return await _billSvc.GetRankingAsync(input);
    }
}
