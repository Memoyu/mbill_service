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
using Memoyu.Mbill.Domain.Entities.Bill.Asset;
using Memoyu.Mbill.Domain.Entities.Bill.Category;
using Memoyu.Mbill.Domain.Entities.Bill.Statement;
using Memoyu.Mbill.Domain.IRepositories.Core;
using Memoyu.Mbill.Domain.Shared.Enums;
using Memoyu.Mbill.Domain.Shared.Extensions;
using Memoyu.Mbill.ToolKits.Base.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Bill.Statement.Impl
{
    public class StatementService : ApplicationService, IStatementService
    {
        private readonly IAuditBaseRepository<StatementEntity, long> _statementRepository;
        private readonly IAuditBaseRepository<CategoryEntity, long> _categoryRepository;
        private readonly IAuditBaseRepository<AssetEntity, long> _assetRepository;
        private readonly IFileRepository _fileRepository;

        public StatementService(
            IAuditBaseRepository<StatementEntity, long> statementRepository,
            IAuditBaseRepository<CategoryEntity, long> categoryRepository,
            IAuditBaseRepository<AssetEntity, long> assetRepository,
            IFileRepository fileRepository)
        {
            _statementRepository = statementRepository;
            _categoryRepository = categoryRepository;
            _assetRepository = assetRepository;
            _fileRepository = fileRepository;
        }

        public async Task InsertAsync(StatementEntity statement)
        {
            await _statementRepository.InsertAsync(statement);
        }

        public async Task<StatementDetailDto> GetDetailAsync(int id)
        {
            var statement = await _statementRepository.GetAsync(id);
            throw new NotImplementedException();
        }

        public async Task<PagedDto<StatementDto>> GetPagesAsync(StatementPagingDto pageDto)
        {

            int? year = pageDto.Date?.Year;
            int? month = pageDto.Date?.Month;
            int? day = pageDto.Date?.Day;

            var statements = await _statementRepository
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
                .OrderBy(pageDto.Sort.IsNullOrEmpty(), "Year DESC, Month DESC, Day DESC, Time DESC")
                .ToPageListAsync(pageDto, out long totalCount);
            List<StatementDto> statementDtos = statements
                .Select(s =>
                {
                    StatementDto dto = Mapper.Map<StatementDto>(s);
                    if (s.CategoryId != null)
                    {
                        var category = _categoryRepository.Get(s.CategoryId.Value);
                        dto.CategoryName = category.Name;
                        dto.CategoryIconPath =_fileRepository.GetFileUrl(category.IconUrl);
                    }
                    else
                    {
                        if (s.Type.Equals(StatementTypeEnum.transfer.ToString()))
                        {
                            dto.CategoryIconPath = _fileRepository.GetFileUrl("core/images/category/icon_transfer_64.png");
                        }
                        else if (s.Type.Equals(StatementTypeEnum.repayment.ToString()))
                        {
                            dto.CategoryIconPath = _fileRepository.GetFileUrl("core/images/category/icon_repayment_64.png");
                        }
                    }
                    var asset = _assetRepository.Get(s.AssetId);
                    if (s.TargetAssetId != null)
                    {
                        var targetAsset = _assetRepository.Get(s.TargetAssetId.Value);
                        dto.TargetAssetName = targetAsset.Name;
                    }
                    dto.AssetName = asset.Name;
                    return dto;
                })
                .ToList();

            return new PagedDto<StatementDto>(statementDtos, totalCount);
        }

       
    }
}
