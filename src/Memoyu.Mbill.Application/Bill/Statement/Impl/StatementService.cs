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
using Memoyu.Mbill.Application.Contracts.Exceptions;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Entities.Bill.Asset;
using Memoyu.Mbill.Domain.Entities.Bill.Category;
using Memoyu.Mbill.Domain.Entities.Bill.Statement;
using Memoyu.Mbill.Domain.IRepositories.Bill.Asset;
using Memoyu.Mbill.Domain.IRepositories.Bill.Category;
using Memoyu.Mbill.Domain.IRepositories.Bill.Statement;
using Memoyu.Mbill.Domain.IRepositories.Core;
using Memoyu.Mbill.Domain.Shared.Enums;
using Memoyu.Mbill.Domain.Shared.Extensions;
using Memoyu.Mbill.ToolKits.Base.Enum.Base;
using Memoyu.Mbill.ToolKits.Base.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Memoyu.Mbill.Domain.Shared.Const.SystemConst;

namespace Memoyu.Mbill.Application.Bill.Statement.Impl
{
    public class StatementService : ApplicationService, IStatementService
    {
        private readonly IStatementRepository _statementRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IFileRepository _fileRepository;

        public StatementService(
            IStatementRepository statementRepository,
            ICategoryRepository categoryRepository,
                IAssetRepository assetRepository,
            IFileRepository fileRepository)
        {
            _statementRepository = statementRepository;
            _categoryRepository = categoryRepository;
            _assetRepository = assetRepository;
            _fileRepository = fileRepository;
        }

        public async Task<StatementDto> InsertAsync(StatementEntity statement)
        {
            var entity = await _statementRepository.InsertAsync(statement);
            if (entity == null) throw new KnownException("新增账单失败！", ServiceResultCode.Failed);
            var statementDto = MapToDto<StatementDto>(entity);
            return statementDto;
        }

        public async Task DeleteAsync(long id)
        {
            var exist = await _statementRepository.Select.AnyAsync(s => s.Id == id && !s.IsDeleted);
            if (!exist) throw new KnownException("没有找到该账单信息", ServiceResultCode.NotFound);
            await _statementRepository.DeleteAsync(id);
        }

        public async Task UpdateAsync(StatementEntity statement)
        {
            var exist = await _statementRepository.Select.AnyAsync(s => s.Id == statement.Id && !s.IsDeleted);
            if (!exist) throw new KnownException("没有找到该账单信息", ServiceResultCode.NotFound);
            await _statementRepository.UpdateAsync(statement);
        }

        public async Task<StatementDetailDto> GetDetailAsync(long id)
        {
            var statement = await _statementRepository.GetAsync(id);
            var dto = MapToDto<StatementDetailDto>(statement);
            var assetDto = await _assetRepository.GetAssetParentAsync(dto.AssetId);
            var categoryDto = await _categoryRepository.GetCategoryParentAsync(dto.CategoryId);
            dto.assetParentName = assetDto?.Name;
            dto.categoryParentName = categoryDto?.Name;
            return dto;
        }

        public async Task<PagedDto<StatementDto>> GetPagesAsync(StatementPagingDto pageDto)
        {
            pageDto.UserId = pageDto.UserId ?? CurrentUser.Id;
            var statements = await _statementRepository
                .Select
                .Where(s => s.IsDeleted == false)
                .Where(s => s.CreateUserId == pageDto.UserId)
                .WhereIf(pageDto.Year != null, s => s.Year == pageDto.Year)
                .WhereIf(pageDto.Month != null, s => s.Month == pageDto.Month)
                .WhereIf(pageDto.Day != null, s => s.Day == pageDto.Day)
                .WhereIf(pageDto.Type.IsNotNullOrEmpty(), s => s.Type == pageDto.Type)
                .WhereIf(pageDto.CategoryId != null, s => s.CategoryId == pageDto.CategoryId)
                .WhereIf(pageDto.AssetId != null, s => s.AssetId == pageDto.AssetId)
                .OrderBy(pageDto.Sort.IsNotNullOrEmpty(), pageDto.Sort)
                .OrderBy(pageDto.Sort.IsNullOrEmpty(), "Year DESC, Month DESC, Day DESC, Time DESC")
                .ToPageListAsync(pageDto, out long totalCount);
            List<StatementDto> statementDtos = statements
                .Select(s =>
                {
                    var dto = MapToDto<StatementDto>(s);
                    return dto;
                })
                .ToList();

            return new PagedDto<StatementDto>(statementDtos, totalCount);
        }
        public async Task<StatementTotalDto> GetMonthStatisticsAsync(StatementTotalInputDto input)
        {
            var userId = input.UserId ?? CurrentUser.Id;
            var statements =  await _statementRepository
               .Select
               .Where(s => s.IsDeleted == false)
               .Where(s => s.CreateUserId == userId)
               .WhereIf(input.Year != null, s => s.Year == input.Year)
               .WhereIf(input.Month != null, s => s.Month == input.Month)
               .ToListAsync();
            var dto = new StatementTotalDto();
            statements.ForEach(s =>
            {
                switch (s.Type)
                {
                    case "expend":
                        dto.MonthExpend += s.Amount;
                        break;
                    case "income":
                        dto.MonthIcome += s.Amount;
                        break;
                    case "repayment":
                        dto.MonthRepayment += s.Amount;
                        break;
                    case "transfer":
                        dto.MonthTransfer += s.Amount;
                        break;
                }
                if (input.Day != null && input.Day == s.Day)
                {
                    switch (s.Type)
                    {
                        case "expend":
                            dto.DayExpend += s.Amount;
                            break;
                        case "income":
                            dto.DayIcome += s.Amount;
                            break;
                        case "repayment":
                            dto.DayRepayment += s.Amount;
                            break;
                        case "transfer":
                            dto.DayTransfer += s.Amount;
                            break;
                    }
                }
            });
            return dto;
        }

        /// <summary>
        /// 映射Dto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statement"></param>
        /// <returns></returns>
        private T MapToDto<T>(StatementEntity statement) where T : MapDto
        {
            T dto = Mapper.Map<T>(statement);
            if (statement.CategoryId != null)
            {
                var category = _categoryRepository.Get(statement.CategoryId.Value);
                dto.CategoryName = category.Name;
                dto.CategoryIconPath = _fileRepository.GetFileUrl(category.IconUrl);
            }
            else
            {
                if (statement.Type.Equals(StatementTypeEnum.transfer.ToString()))
                {
                    dto.CategoryIconPath = _fileRepository.GetFileUrl("core/images/category/icon_transfer_64.png");
                }
                else if (statement.Type.Equals(StatementTypeEnum.repayment.ToString()))
                {
                    dto.CategoryIconPath = _fileRepository.GetFileUrl("core/images/category/icon_repayment_64.png");
                }
            }
            var asset = _assetRepository.Get(statement.AssetId);
            if (statement.TargetAssetId != null)
            {
                var targetAsset = _assetRepository.Get(statement.TargetAssetId.Value);
                dto.TargetAssetName = targetAsset.Name;
            }
            dto.AssetName = asset.Name;
            dto.TypeName = Switcher.StatementType(statement.Type);

            return dto;
        }

     
    }
}
