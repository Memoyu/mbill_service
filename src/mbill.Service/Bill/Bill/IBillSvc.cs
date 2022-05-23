namespace mbill.Service.Bill.Statement;

public interface IBillSvc
{
    /// <summary>
    /// 新增账单
    /// </summary>
    /// <param name="statement">数据源</param>
    /// <returns></returns>
    Task<BillDto> InsertAsync(BillEntity statement);

    /// <summary>
    /// 获取账单详情
    /// </summary>
    /// <param name="id">账单id</param>
    /// <returns></returns>
    Task<BillDetailDto> GetDetailAsync(long id);


    /// <summary>
    /// 删除账单信息
    /// </summary>
    /// <param name="id">账单id</param>
    /// <returns></returns>
    Task DeleteAsync(long id);

    /// <summary>
    /// 更新账单信息
    /// </summary>
    /// <param name="statement">账单信息</param>
    /// <returns></returns>
    Task UpdateAsync(BillEntity statement);

    /// <summary>
    /// 获取分页账单数据
    /// </summary>
    /// <param name="pageDto">分页查询</param>
    /// <returns></returns>
    Task<PagedDto<BillDto>> GetPagesAsync(BillPagingDto pageDto);

    /// <summary>
    /// 获取指定日期各类型账单总额统计
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<BillTotalDto> GetStatisticsTotalAsync(BillDateInputDto input);

    /// <summary>
    /// 获取指定日期支出分类统计
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<BillExpendCategoryDto> GetExpendCategoryStatisticsAsync(BillDateInputDto input);

    /// <summary>
    /// 获取当前月份所有周的支出趋势统计
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<IEnumerable<BillExpendTrendDto>> GetWeekExpendTrendStatisticsAsync(BillDateInputDto input);

    /// <summary>
    /// 获取当前月往前5个月的支出趋势统计
    /// </summary>
    /// <param name="input"></param>
    /// <param name="count">月数</param>
    /// <returns></returns>
    Task<IEnumerable<BillExpendTrendDto>> GetMonthExpendTrendStatisticsAsync(BillDateInputDto input, int count);

}
