/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.IRepositories.Statement
*   文件名称 ：IStatementRepository.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 21:06:06
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Domain.Entities.Statement;
using Memoyu.Mbill.Domain.Base;

namespace Memoyu.Mbill.Domain.IRepositories.Statement
{
    public interface IStatementRepository : IAuditBaseRepository<StatementEntity>
    {
    }
}
