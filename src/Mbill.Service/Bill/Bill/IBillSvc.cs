namespace Mbill.Service.Bill.Bill;

public interface IBillSvc
{
    /// <summary>
    /// 新增账单
    /// </summary>
    /// <param name="input">数据源</param>
    /// <returns></returns>
    Task<ServiceResult<BillSimpleDto>> CreateAsync(ModifyBillInput input);

    /// <summary>
    /// 获取账单详情
    /// </summary>
    /// <param name="id">账单id</param>
    /// <returns></returns>
    Task<ServiceResult<BillDetailDto>> GetDetailAsync(long id);


    /// <summary>
    /// 删除账单信息
    /// </summary>
    /// <param name="id">账单id</param>
    /// <returns></returns>
    Task<ServiceResult> DeleteAsync(long id);

    /// <summary>
    /// 更新账单信息
    /// </summary>
    /// <param name="input">账单信息</param>
    /// <returns></returns>
    Task<ServiceResult<BillSimpleDto>> UpdateAsync(ModifyBillInput input);

    /// <summary>
    /// 获取指定日期账单
    /// </summary>
    /// <param name="input">查询条件</param>
    /// <returns></returns>
    Task<ServiceResult<BillsByDayWithStatDto>> GetByDayAsync(DayBillInput input);

    /// <summary>
    /// 获取账单检索记录
    /// </summary>
    /// <returns></returns>
    Task<ServiceResult<List<BillSearchRecordOutput>>> GetSearchRecordsAsync();

    /// <summary>
    /// 检索账单数据
    /// </summary>
    /// <param name="input">检索条件</param>
    /// <returns></returns>
    Task<ServiceResult<PagedDto<BillDetailDto>>> SearchPagesAsync(BillSearchPagingInput input);

    /// <summary>
    /// 获取指定条件分页账单
    /// </summary>
    /// <param name="input">分页查询</param>
    /// <returns></returns>
    Task<ServiceResult<PagedDto<BillSimpleDto>>> GetPagesAsync(BillPagingInput input);

    /// <summary>
    /// 获取分页账单数据
    /// </summary>
    /// <param name="input">分页查询</param>
    /// <returns></returns>
    Task<ServiceResult<PagedDto<BillsByDayDto>>> GetByMonthPagesAsync(MonthBillPagingInput input);

    /// <summary>
    /// 获取日期范围内存在账单的日期
    /// </summary>
    /// <param name="input">查询入参</param>
    /// <returns></returns>
    Task<ServiceResult<List<BillDateWithTotalDto>>> RangeHasBillDaysAsync(RangeHasBillDaysInput input);

    /// <summary>
    /// 获取指定月份账单总金额
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<MonthTotalStatDto>> GetMonthTotalStatAsync(MonthTotalStatInput input);

    /// <summary>
    /// 获取指定日期各类型账单总额统计
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<YearTotalStatDto>> GetYearTotalStatAsync(YearTotalStatInput input);

    /// <summary>
    /// 获取指定年份的收支结余统计
    /// </summary>
    /// <param name="year">年份</param>
    /// <returns></returns>
    Task<ServiceResult<List<YearSurplusStatDto>>> GetYearSurplusStatAsync(int year);

    /// <summary>
    /// 获取指定年份账单金额趋势统计
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<MonthTotalTrendStatDto>> GetMonthTotalTrendStatAsync(MonthTotalTrendStatInput input);

    /// <summary>
    /// 获取指定年份账单金额趋势统计
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<YearTotalTrendStatDto>> GetYearTotalTrendStatAsync(YearTotalTrendStatInput input);

    /// <summary>
    /// 获取指定日期的收入或支出分类占比统计
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<CategoryPercentStatDto>> GetCategoryPercentStatAsync(CategoryPercentStatInput input);

    /// <summary>
    /// 获取指定日期的收入或支出分类占比分组列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<List<CategoryPercentGroupDto>>> GetCategoryPercentGroupAsync(CategoryPercentGroupInput input);

    /// <summary>
    /// 获取指定条件账单排行列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<PagedDto<BillSimpleDto>>> GetRankingAsync(RankingPagingInput input);
}
