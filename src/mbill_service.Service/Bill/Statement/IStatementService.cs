using mbill_service.Core.Domains.Common;
using mbill_service.Core.Domains.Entities.Bill;
using mbill_service.Service.Bill.Statement.Output;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mbill_service.Service.Bill.Statement
{
    public interface IStatementService
    {
        /// <summary>
        /// 新增账单
        /// </summary>
        /// <param name="statement">数据源</param>
        /// <returns></returns>
        Task<StatementDto> InsertAsync(StatementEntity statement);

        /// <summary>
        /// 获取账单详情
        /// </summary>
        /// <param name="id">账单id</param>
        /// <returns></returns>
        Task<StatementDetailDto> GetDetailAsync(long id);


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
        Task UpdateAsync(StatementEntity statement);

        /// <summary>
        /// 获取分页账单数据
        /// </summary>
        /// <param name="pageDto">分页查询</param>
        /// <returns></returns>
        Task<PagedDto<StatementDto>> GetPagesAsync(StatementPagingDto pageDto);

        /// <summary>
        /// 获取指定日期各类型账单总额统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<StatementTotalDto> GetStatisticsTotalAsync(StatementDateInputDto input);

        /// <summary>
        /// 获取指定日期支出分类统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<StatementExpendCategoryDto> GetExpendCategoryStatisticsAsync(StatementDateInputDto input);

        /// <summary>
        /// 获取当前月份所有周的支出趋势统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<StatementExpendTrendDto>> GetWeekExpendTrendStatisticsAsync(StatementDateInputDto input);

        /// <summary>
        /// 获取当前月往前5个月的支出趋势统计
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count">月数</param>
        /// <returns></returns>
        Task<IEnumerable<StatementExpendTrendDto>> GetMonthExpendTrendStatisticsAsync(StatementDateInputDto input, int count);

    }
}
