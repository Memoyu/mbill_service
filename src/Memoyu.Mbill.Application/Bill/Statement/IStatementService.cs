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

using Memoyu.Mbill.Domain.Entities.Bill.Statement;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Bill.Statement
{
    public interface IStatementService
    {
        /// <summary>
        /// 新增账单
        /// </summary>
        /// <param name="input">数据源</param>
        /// <returns></returns>
        Task InsertAsync(StatementEntity statement);
    }
}
