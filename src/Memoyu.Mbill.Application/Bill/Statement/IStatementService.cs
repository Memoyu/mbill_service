/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Bill.Statement
*   文件名称 ：IStatementService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 21:15:52
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/

using Memoyu.Mbill.Application.Contracts.Dtos.Bill.Statement;
using Memoyu.Mbill.Domain.Entities.Bill.Statement;
using Memoyu.Mbill.ToolKits.Base.Page;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Bill.Statement
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
