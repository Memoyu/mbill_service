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
using Memoyu.Mbill.Application.Contracts.Dtos.Bill.Statement;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Entities.Bill.Statement;
using Memoyu.Mbill.Domain.Shared.Extensions;
using Memoyu.Mbill.ToolKits.Base.Page;
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

        public StatementService(IAuditBaseRepository<StatementEntity, long> statementRepository)
        {
            _statementRepository = statementRepository;
        }

        public async Task<PagedDto<StatementDto>> GetPageAsync(StatementPagingDto pageDto)
        {

            int? year = pageDto.Date?.Year;
            int? month = pageDto.Date?.Month;
            int? day = pageDto.Date?.Day;

            List<StatementDto> statements = await _statementRepository
                .Select
                .Where(s => s.IsDeleted == false)
                .WhereIf(pageDto.UserId != null, s => s.CreateUserId == pageDto.UserId)
                .WhereIf(year != null, s => s.Year == year)
                .WhereIf(month != null, s => s.Month == month)
                .WhereIf(day != null, s => s.Day == day)
                .WhereIf(pageDto.Type.IsNotNullOrEmpty(), s => s.Type == pageDto.Type)
                .WhereIf(pageDto.CategoryId != null, s => s.CategoryId == pageDto.CategoryId)
                .WhereIf(pageDto.AssetId != null, s => s.AssetId == pageDto.AssetId)
                .OrderBy(pageDto.Sort.IsNotNullOrEmpty(), pageDto.Sort)
                .ToPageListAsync<StatementEntity, StatementDto>(pageDto, out long totalCount);

            return new PagedDto<StatementDto>(statements, totalCount);
        }

        public async Task InsertAsync(StatementEntity statement)
        {
            await _statementRepository.InsertAsync(statement);
        }
    }
}
