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
using Memoyu.Mbill.ToolKits.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            Expression<Func<StatementEntity, object>> ignoreExp = e => new { e.CreateUserId, e.CreateTime };
            await _statementRepository.UpdateWithIgnoreAsync(statement, ignoreExp);
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
            // pageDto.UserId = pageDto.UserId ?? CurrentUser.Id;
            pageDto.Sort = pageDto.Sort.IsNullOrEmpty() ? "Time DESC" : pageDto.Sort.Replace("-", " ");
            var statements = await _statementRepository
                .Select
                .Where(s => s.IsDeleted == false)
                .WhereIf(pageDto.UserId != null, s => s.CreateUserId == pageDto.UserId)
                .WhereIf(pageDto.Year != null, s => s.Year == pageDto.Year)
                .WhereIf(pageDto.Month != null, s => s.Month == pageDto.Month)
                .WhereIf(pageDto.Day != null, s => s.Day == pageDto.Day)
                .WhereIf(pageDto.Type.IsNotNullOrEmpty(), s => s.Type == pageDto.Type)
                .WhereIf(pageDto.CategoryId != null, s => s.CategoryId == pageDto.CategoryId)
                .WhereIf(pageDto.AssetId != null, s => s.AssetId == pageDto.AssetId)
                .OrderBy(pageDto.Sort)
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

        public async Task<StatementTotalDto> GetStatisticsTotalAsync(StatementDateInputDto input)
        {
            // var userId = input.UserId ?? CurrentUser.Id;
            var statements = await _statementRepository
               .Select
               .Where(s => s.IsDeleted == false)
               .WhereIf(input.UserId != null, s => s.CreateUserId == input.UserId)
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

        public async Task<StatementExpendCategoryDto> GetExpendCategoryStatisticsAsync(StatementDateInputDto input)
        {
            var dto = new StatementExpendCategoryDto();
            var statements = await _statementRepository
               .Select
               .Where(s => s.IsDeleted == false)
               .Where(s => s.Type.Equals("expend"))
               .WhereIf(input.UserId != null, s => s.CreateUserId == input.UserId)
               .WhereIf(input.Year != null, s => s.Year == input.Year)
               .WhereIf(input.Month != null, s => s.Month == input.Month)
               .WhereIf(input.Day != null, s => s.Day == input.Day)
               .ToListAsync();
            decimal total = 0;
            // 根据CategoryId分组，并统计总额
            var childDetails = statements.GroupBy(s => s.CategoryId).Select(g =>
            {
                var info = _categoryRepository.GetAsync(g.Key.Value).Result;
                var parentInfo = _categoryRepository.GetCategoryParentAsync(g.Key.Value).Result;
                var amount = g.Sum(s => s.Amount);
                total += amount;
                return new
                {
                    CategoryId = g.Key,
                    Amount = amount,
                    Info = info,
                    ParentInfo = parentInfo
                };
            });

            var childDtos = new List<ChildGroupDto>();
            var parentDtos = childDetails.GroupBy(p => new { p.ParentInfo.Id, p.ParentInfo.Name }).Select(g =>
            {
                var childDto = new ChildGroupDto();
                var childTotal = g.Sum(s => s.Amount);
                childDto.ParentName = g.Key.Name;
                childDto.Childs = childDetails.Where(d => d.Info.ParentId == g.Key.Id).Select(d => new
                {
                    Id = d.Info.Id,
                    Name = d.Info.Name,
                    Data = d.Amount,
                    Percent = Math.Round(d.Amount / childTotal, 4) * 100,
                    CategoryIconPath = _fileRepository.GetFileUrl(d.Info.IconUrl)
                });
                childDtos.Add(childDto);
                return new StatisticsDto
                {
                    Id = g.Key.Id,
                    Name = g.Key.Name,
                    Data = Math.Round(g.Sum(s => s.Amount) / total, 4) * 100
                };
            }).ToList();
            dto.ParentCategoryStas = parentDtos;
            dto.ChildCategoryStas = childDtos;
            return dto;
        }

        public async Task<IEnumerable<StatementExpendTrendDto>> GetWeekExpendTrendStatisticsAsync(StatementDateInputDto input)
        {

            var dateList = DateTimeUtil.GetWeeksOfMonth(input.Year.Value, input.Month.Value);
            var startDate = dateList.OrderBy(d => d.Number).FirstOrDefault().StartDate.Date;
            var EndDate = dateList.OrderBy(d => d.Number).LastOrDefault().EndDate.Date.AddDays(1).AddSeconds(-1);// 加上23:59:59

            var statements = await _statementRepository
              .Select
              .Where(s => s.IsDeleted == false)
              .WhereIf(input.Type.IsNotNullOrEmpty(), s => s.Type == input.Type)
              .WhereIf(input.UserId != null, s => s.CreateUserId == input.UserId)
              .WhereIf(input.Year != null, s => s.Year == input.Year)
              .WhereIf(input.Month != null, s => s.Time >= startDate && s.Time <= EndDate)
              .ToListAsync();

            var dtos = dateList.Select(d => new StatementExpendTrendDto
            {
                Name = $"{d.StartDate.Month}/{d.StartDate.Day}-{d.EndDate.Month}/{d.EndDate.Day}",
                Data = statements.Where(s => s.Time >= d.StartDate && s.Time <= d.EndDate.AddDays(1).AddSeconds(-1)).Select(t => new { t.Amount }).Sum(t => t.Amount),
                StartDate = d.StartDate.Date,
                EndDate = d.EndDate.Date
            });

            return dtos;
        }

        public async Task<IEnumerable<StatementExpendTrendDto>> GetMonthExpendTrendStatisticsAsync(StatementDateInputDto input, int count)
        {
            var dateList = new List<WeeksOfMonth>();
            // 获取当前月份前count个月份（包含当前月份）
            for (int i = 0; i < count; i++)
            {
                var currentDate = new DateTime(input.Year.Value, input.Month.Value, 1).AddMonths(-i);
                dateList.Add(new WeeksOfMonth
                {
                    Number = i + 1,
                    StartDate = currentDate,
                    EndDate = currentDate.AddMonths(1).AddDays(-1)
                });
            }

            var startDate = dateList.OrderBy(d => d.Number).LastOrDefault().StartDate.Date;//获取最小的月份（Number最大）
            var EndDate = dateList.OrderBy(d => d.Number).FirstOrDefault().EndDate.Date;

            var statements = await _statementRepository
             .Select
             .Where(s => s.IsDeleted == false)
             .WhereIf(input.Type.IsNotNullOrEmpty(), s => s.Type == input.Type)
             .WhereIf(input.UserId != null, s => s.CreateUserId == input.UserId)
             .WhereIf(input.Year != null, s => s.Year >= startDate.Year && s.Year <= EndDate.Year)
             .WhereIf(input.Month != null, s => s.Time >= startDate && s.Time <= EndDate.AddDays(1).AddSeconds(-1))
             .ToListAsync();

            var dtos = dateList.Select(d => new StatementExpendTrendDto
            {
                Name = $"{d.StartDate.Year}/{d.StartDate.Month}",
                Data = statements.Where(s => s.Year == d.StartDate.Year && s.Month == d.StartDate.Month).Select(t => new { t.Amount }).Sum(t => t.Amount),
                StartDate = d.StartDate.Date,
                EndDate = d.EndDate.Date
            });

            return dtos;


        }

        #region Private

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
                var category = _categoryRepository.Get(statement.CategoryId.Value) ?? throw new KnownException("分类数据查询失败！", ServiceResultCode.NotFound);
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
            var asset = _assetRepository.Get(statement.AssetId) ?? throw new KnownException("资产数据查询失败！", ServiceResultCode.NotFound);
            if (statement.TargetAssetId != null)
            {
                var targetAsset = _assetRepository.Get(statement.TargetAssetId.Value);
                dto.TargetAssetName = targetAsset.Name;
            }
            dto.AssetName = asset.Name;
            dto.TypeName = Switcher.StatementType(statement.Type);

            return dto;
        }

        #endregion

    }
}
