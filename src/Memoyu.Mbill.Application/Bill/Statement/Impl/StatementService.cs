/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Bill.Statement.Impl
*   文件名称 ：StatementService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 21:15:39
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Application.Base.Impl;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Entities.Bill.Statement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Bill.Statement.Impl
{
    public class StatementService : ApplicationService, IStatementService
    {
        private readonly IAuditBaseRepository<StatementEntity, long> _statementRepository;

        public StatementService(IAuditBaseRepository<StatementEntity , long> statementRepository)
        {
            _statementRepository = statementRepository;
        }

        public async Task InsertAsync(StatementEntity statement)
        {
            await _statementRepository.InsertAsync(statement);
        }
    }
}
