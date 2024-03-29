﻿using Mapster;
using Mbill.Core.Common;
using MongoDB.Driver;

namespace Mbill.Service.Bill.Bill;

public class BillSvc : ApplicationSvc, IBillSvc
{
    private readonly IBillRepo _billRepo;
    private readonly ICategoryRepo _categoryRepo;
    private readonly IAssetRepo _assetRepo;
    private readonly IFileRepo _fileRepo;
    private readonly IBillMongoRepo _mongoRepo;
    private readonly IBillSearchRecordRepo _mongoSearchRecordRepo;

    public BillSvc(
        IBillRepo billRepo,
        ICategoryRepo categoryRepo,
        IAssetRepo assetRepo,
        IFileRepo fileRepo,
        IBillMongoRepo mongoRepo,
        IBillSearchRecordRepo mongoSearchRecordRepo)
    {
        _billRepo = billRepo;
        _categoryRepo = categoryRepo;
        _assetRepo = assetRepo;
        _fileRepo = fileRepo;
        _mongoRepo = mongoRepo;
        _mongoSearchRecordRepo = mongoSearchRecordRepo;
    }

    [Transactional]
    public async Task<ServiceResult<BillSimpleDto>> CreateAsync(ModifyBillInput input)
    {
        var bill = Mapper.Map<BillEntity>(input);
        bill.BId = SnowFlake.NextId();
        var entity = await _billRepo.InsertAsync(bill);
        var resMongo = await _mongoRepo.InsertOneAsync(entity);
        if (!resMongo)
            throw new OperationCanceledException("插入失败");
        if (entity == null) ServiceResult<BillSimpleDto>.Failed("新增账单失败！");

        entity = await _billRepo.GetBillAsync(entity.BId);
        return ServiceResult<BillSimpleDto>.Successed(Mapper.Map<BillSimpleDto>(entity));
    }

    [Transactional]
    public async Task<ServiceResult> DeleteAsync(long bId)
    {
        var bill = await _billRepo.Select.Where(s => s.BId == bId && !s.IsDeleted).ToOneAsync();
        if (bill is null) return ServiceResult.Failed("没有找到该账单信息");
        var re = await _billRepo.DeleteAsync(bill);
        FilterDefinitionBuilder<BillEntity> buildFilter = Builders<BillEntity>.Filter;
        var filter = buildFilter.Eq(b => b.BId, bId);
        var resMongo = await _mongoRepo.DeleteOneAsync(filter);
        if (resMongo is null)
            throw new OperationCanceledException("删除失败");
        return ServiceResult.Successed("删除成功");
    }

    public async Task<ServiceResult<BillSimpleDto>> UpdateAsync(ModifyBillInput input)
    {
        var entity = await _billRepo.GetBillAsync(input.BId);
        if (entity is null) return ServiceResult<BillSimpleDto>.Failed("没有找到该账单信息");
        Expression<Func<BillEntity, object>> ignoreExp = e => new { e.CreateUserBId, e.CreateTime };
        input.Adapt(entity);
        await _billRepo.UpdateAsync(entity);
        var find = await _mongoRepo.FindOneAsync(entity.Id, false);
        if (find is null)
        {
            var resMongo = await _mongoRepo.InsertOneAsync(entity);
            if (!resMongo)
                throw new OperationCanceledException("更新失败");
        }
        else
        {
            var update = Builders<BillEntity>.Update
                 .Set(nameof(entity.CategoryBId), input.CategoryBId)
                 .Set(nameof(entity.AssetBId), input.AssetBId)
                 .Set(nameof(entity.Amount), input.Amount)
                 .Set(nameof(entity.Type), input.Type)
                 .Set(nameof(entity.Description), input.Description)
                 .Set(nameof(entity.Address), input.Address)
                 .Set(nameof(entity.Time), input.Time);
            var filter = Builders<BillEntity>.Filter.Eq(b => b.BId, entity.BId);
            var resMongo = await _mongoRepo.UpdateOneAsync(update, filter);
            if (resMongo is null)
                throw new OperationCanceledException("更新失败");
        }
        return ServiceResult<BillSimpleDto>.Successed(Mapper.Map<BillSimpleDto>(entity));
    }

    public async Task<ServiceResult<BillDetailDto>> GetDetailAsync(long bId)
    {
        var bill = await _billRepo.GetBillAsync(bId);
        if (bill == null)
            return ServiceResult<BillDetailDto>.Failed("没有找到该账单信息");

        var category = await _categoryRepo.GetCategoryAsync(bill.CategoryBId);
        var asset = await _assetRepo.GetAssetAsync(bill.AssetBId);
        bill.Category = category;
        bill.Asset = asset;

        var dto = Mapper.Map<BillDetailDto>(bill);
        return ServiceResult<BillDetailDto>.Successed(dto);
    }

    public async Task<ServiceResult<BillsByDayWithStatDto>> GetByDayAsync(DayBillInput input)
    {
        var begin = input.Date.Date;
        var end = begin.AddDays(1).AddSeconds(-1);
        var bills = await _billRepo
            .Select
            .Include(b => b.Category)
            .Include(b => b.Asset)
            .Where(s => s.IsDeleted == false)
            .Where(s => s.CreateUserBId == CurrentUser.BId)
            .Where(s => s.Time >= begin && s.Time <= end)
            .WhereIf(input.Type.HasValue, s => s.Type == input.Type)
            .WhereIf(input.CategoryId.HasValue, s => s.CategoryBId == input.CategoryId)
            .WhereIf(input.AssetId.HasValue, s => s.AssetBId == input.AssetId)
            .OrderBy("time DESC")
            .ToListAsync();

        var dto = new BillsByDayWithStatDto();
        dto.Day = begin.Day;
        dto.Week = begin.GetWeek();
        var expend = 0m;
        var income = 0m;
        foreach (var i in bills)
        {
            if (i.Type == (int)BillTypeEnum.expend)
                expend += i.Amount;
            else
                income += i.Amount;
            dto.Items.Add(Mapper.Map<BillSimpleDto>(i));
        }
        dto.Expend = expend.AmountFormat();
        dto.Income = income.AmountFormat();
        return ServiceResult<BillsByDayWithStatDto>.Successed(dto);
    }

    public async Task<ServiceResult<List<BillSearchRecordOutput>>> GetSearchRecordsAsync()
    {
        var filter = Builders<BillSearchRecordEntity>.Filter.Eq(b => b.UserBId, CurrentUser.BId);
        var sort = Builders<BillSearchRecordEntity>.Sort.Descending("SearchTime");
        var list = await _mongoSearchRecordRepo.FindListByPageAsync(filter, 1, 10, null, sort);
        return ServiceResult<List<BillSearchRecordOutput>>.Successed(Mapper.Map<List<BillSearchRecordOutput>>(list));
    }

    public async Task<ServiceResult<PagedDto<BillDetailDto>>> SearchPagesAsync(BillSearchPagingInput input)
    {
        var recordEntity = Mapper.Map<BillSearchRecordEntity>(input);
        recordEntity.UserBId = CurrentUser.BId ?? 0;
        // 插入检索记录
        await _mongoSearchRecordRepo.InsertOneAsync(recordEntity);

        var sort = Builders<BillEntity>.Sort.Descending("Time");
        var bFilter = Builders<BillEntity>.Filter;
        List<FilterDefinition<BillEntity>> filters = new List<FilterDefinition<BillEntity>>();

        // 账单类型
        if (input.Types != null && input.Types.Any())
            filters.Add(bFilter.And(bFilter.In(b => b.Type, input.Types)));

        // 账单分类
        if (input.CategoryBIds != null && input.CategoryBIds.Any())
            filters.Add(bFilter.And(bFilter.In(b => b.CategoryBId, input.CategoryBIds)));

        // 账单账户
        if (input.AssetBIds != null && input.AssetBIds.Any())
            filters.Add(bFilter.And(bFilter.In(b => b.AssetBId, input.AssetBIds)));

        // 金额区间
        if (input.Amount != null)
        {
            if (input.Amount.Max.HasValue && !input.Amount.Min.HasValue)
                filters.Add(bFilter.And(bFilter.Lte(b => b.Amount, input.Amount.Max.Value)));
            else if (!input.Amount.Max.HasValue && input.Amount.Min.HasValue)
                filters.Add(bFilter.And(bFilter.Gte(b => b.Amount, input.Amount.Min.Value)));
            else if (input.Amount.Max.HasValue && input.Amount.Min.HasValue)
                filters.Add(bFilter.And(bFilter.Gte(b => b.Amount, input.Amount.Min.Value), bFilter.Lte(b => b.Amount, input.Amount.Max.Value)));
        }
        // 金额区间
        if (input.Date != null && input.Date.Begin.HasValue && input.Date.End.HasValue)
            filters.Add(bFilter.And(bFilter.Gte(b => b.Time, input.Date.Begin.Value), bFilter.Lte(b => b.Time, input.Date.End.Value.AddDays(1).AddSeconds(-1))));

        // 地址
        if (!string.IsNullOrWhiteSpace(input.Address))
            filters.Add(bFilter.And(bFilter.Where(b => b.Address.Contains(input.Address))));

        // 备注
        if (!string.IsNullOrWhiteSpace(input.Remark))
            filters.Add(bFilter.And(bFilter.Where(b => b.Description.Contains(input.Remark))));

        // 关键词
        if (string.IsNullOrWhiteSpace(input.Address) && string.IsNullOrWhiteSpace(input.Remark) && !string.IsNullOrWhiteSpace(input.KeyWord))
            filters.Add(bFilter.And(bFilter.Or(bFilter.Where(b => b.Address.Contains(input.KeyWord)),
                bFilter.Where(b => b.Description.Contains(input.KeyWord)))));

        var filter = bFilter.And(bFilter.Eq(b => b.CreateUserBId, CurrentUser.BId), bFilter.And(filters));//时间段条件用OR拼在一起
        var paged = new PagedDto<BillDetailDto>();

        var total = await _mongoRepo.CountAsync(filter);
        if (total != 0)
        {
            var bills = await _mongoRepo.FindListByPageAsync(filter, input.Page, input.Size, null, sort);
            List<BillDetailDto> dtos = new List<BillDetailDto>();
            foreach (var bill in bills)
            {
                var category = await _categoryRepo.GetCategoryAsync(bill.CategoryBId);
                var asset = await _assetRepo.GetAssetAsync(bill.AssetBId);
                bill.Category = category;
                bill.Asset = asset;
                var dto = Mapper.Map<BillDetailDto>(bill);
                dtos.Add(dto);
            }

            paged.Total = total;
            paged.Items = dtos;
        }
        return ServiceResult<PagedDto<BillDetailDto>>.Successed(paged);
    }

    public async Task<ServiceResult<PagedDto<BillSimpleDto>>> GetPagesAsync(BillPagingInput input)
    {
        input.Sort = input.Sort.IsNullOrEmpty() ? "time DESC" : input.Sort.Replace("-", " ");
        var bills = await _billRepo
            .Select
            .Include(b => b.Category)
            .Include(b => b.Asset)
            .Where(s => s.IsDeleted == false)
            .Where(s => s.CreateUserBId == CurrentUser.BId)
            .WhereIf(input.DateType == 0, s => s.Time.Year == input.Date.Year && s.Time.Month == input.Date.Month)
            .WhereIf(input.DateType == 1, s => s.Time.Year == input.Date.Year)
            .WhereIf(input.Type.HasValue, s => s.Type == input.Type)
            .WhereIf(input.CategoryBId.HasValue, s => s.CategoryBId == input.CategoryBId)
            .WhereIf(input.AssetBId.HasValue, s => s.AssetBId == input.AssetBId)
            .OrderBy(input.Sort)
            .ToPageListAsync(input, out long totalCount);

        var dtos = Mapper.Map<List<BillSimpleDto>>(bills);
        return ServiceResult<PagedDto<BillSimpleDto>>.Successed(new PagedDto<BillSimpleDto>(dtos, totalCount));
    }

    public async Task<ServiceResult<PagedDto<BillsByDayDto>>> GetByMonthPagesAsync(MonthBillPagingInput input)
    {
        input.Sort = input.Sort.IsNullOrEmpty() ? "time DESC" : input.Sort.Replace("-", " ");

        var bills = await _billRepo
            .Select
            .Include(b => b.Category)
            .Include(b => b.Asset)
            .Where(s => s.CreateUserBId == CurrentUser.BId)
            .Where(s => s.Time.Year == input.Month.Year && s.Time.Month == input.Month.Month)
            .WhereIf(input.Type.HasValue, s => s.Type == input.Type)
            .WhereIf(input.CategoryBId.HasValue, s => s.CategoryBId == input.CategoryBId)
            .WhereIf(input.AssetBId.HasValue, s => s.AssetBId == input.AssetBId)
            .OrderBy(input.Sort)
            .ToPageListAsync(input, out long totalCount);

        List<BillsByDayDto> dtos = new List<BillsByDayDto>();
        var groups = bills.GroupBy(b => b.Time.Date);
        foreach (var group in groups)
        {
            var dto = new BillsByDayDto();
            dto.Day = group.Key.Day;
            dto.Week = group.Key.GetWeek();
            foreach (var i in group)
            {
                dto.Items.Add(Mapper.Map<BillSimpleDto>(i));
            }
            dtos.Add(dto);
        };

        return ServiceResult<PagedDto<BillsByDayDto>>.Successed(new PagedDto<BillsByDayDto>(dtos, totalCount));
    }

    public async Task<ServiceResult<List<BillDateWithTotalDto>>> RangeHasBillDaysAsync(RangeHasBillDaysInput input)
    {
        var begin = input.BeginDate.FirstDayOfMonth();
        var end = input.EndDate.LastDayOfMonth().AddDays(1).AddSeconds(-1);
        var bills = await _billRepo
            .Select
            .Include(b => b.Category)
            .Include(b => b.Asset)
            .Where(s => s.IsDeleted == false)
            .Where(s => s.CreateUserBId == CurrentUser.BId)
            .Where(s => s.Time <= end && s.Time >= begin)
            .ToListAsync();
        var dtos = bills.GroupBy(b => b.Time.Date).Select(
            g => new BillDateWithTotalDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Day = g.Key.Day,
                Total = g.Count(),
            }).ToList();
        return ServiceResult<List<BillDateWithTotalDto>>.Successed(dtos);
    }

    public async Task<ServiceResult<MonthTotalStatDto>> GetMonthTotalStatAsync(MonthTotalStatInput input)
    {
        var begin = input.Month.FirstDayOfMonth();
        var end = input.Month.LastDayOfMonth().AddDays(1).AddSeconds(-1);
        var expend = 0m;
        var income = 0m;
        var expendAvg = 0m;

        // 构建Select
        ISelect<BillEntity> GetExpendSelect() => _billRepo
               .Select
               .Where(s => s.IsDeleted == false)
               .Where(s => s.CreateUserBId == CurrentUser.BId)
                .Where(s => s.Type == (int)BillTypeEnum.expend)
               .Where(s => s.Time <= end && s.Time >= begin);

        // 支出
        expend = await GetExpendSelect().SumAsync(e => e.Amount);

        // 收入
        income = await _billRepo
             .Select
             .Where(s => s.IsDeleted == false)
             .Where(s => s.CreateUserBId == CurrentUser.BId)
             .Where(s => s.Type == (int)BillTypeEnum.income)
              .Where(s => s.Time <= end && s.Time >= begin).SumAsync(e => e.Amount);

        // 平均支出
        if (input.Opearte == 1)
            expendAvg = expend / DateTime.DaysInMonth(begin.Year, begin.Month);


        return ServiceResult<MonthTotalStatDto>.Successed(new MonthTotalStatDto
        {
            Expend = expend.AmountFormat(),
            Income = income.AmountFormat(),
            ExpendAvg = expendAvg.AmountFormat(),
        });
    }

    public async Task<ServiceResult<YearTotalStatDto>> GetYearTotalStatAsync(YearTotalStatInput input)
    {
        var expend = 0m;
        var expendAvg = 0m;
        var income = 0m;

        // 构建Select
        ISelect<BillEntity> GetExpendSelect() => _billRepo
               .Select
               .Where(s => s.IsDeleted == false)
               .Where(s => s.CreateUserBId == CurrentUser.BId)
               .Where(s => s.Type == (int)BillTypeEnum.expend)
               .Where(s => s.Time.Year == input.Year);

        // 支出
        expend = await GetExpendSelect().SumAsync(e => e.Amount);

        // 收入
        income = await _billRepo
             .Select
             .Where(s => s.IsDeleted == false)
             .Where(s => s.CreateUserBId == CurrentUser.BId)
             .Where(s => s.Type == (int)BillTypeEnum.income)
             .Where(s => s.Time.Year == input.Year).SumAsync(e => e.Amount);

        if (input.Opearte == 1)
            // 平均支出
            expendAvg = (decimal)await GetExpendSelect().AvgAsync(e => e.Amount);

        return ServiceResult<YearTotalStatDto>.Successed(new YearTotalStatDto
        {
            Expend = expend.AmountFormat(),
            Income = income.AmountFormat(),
            Surplus = (income - expend).AmountFormat(),
            ExpendAvg = expendAvg.AmountFormat(),
        });
    }

    public async Task<ServiceResult<List<YearSurplusStatDto>>> GetYearSurplusStatAsync(int year)
    {
        var dtos = new List<YearSurplusStatDto>();
        // 构建Select
        ISelect<BillEntity> GetSelect() => _billRepo
              .Select
              .Where(s => s.IsDeleted == false)
              .Where(s => s.CreateUserBId == CurrentUser.BId)
              .Where(s => s.Time.Year == year);

        // 计算收、支总额
        var expend = await GetSelect().Where(s => s.Type == (int)BillTypeEnum.expend).GroupBy(s => s.Time.Month).ToListAsync(s => new { Month = s.Key, Sum = s.Sum(s.Value.Amount) });
        var income = await GetSelect().Where(s => s.Type == (int)BillTypeEnum.income).GroupBy(s => s.Time.Month).ToListAsync(s => new { Month = s.Key, Sum = s.Sum(s.Value.Amount) });

        var month = 12;
        var curYear = DateTime.Now.Year;
        if (curYear == year) month = DateTime.Now.Month;
        // 构建返回Dto
        for (int m = 1; m <= month; m++)
        {
            var e = expend.FirstOrDefault(e => e.Month == m)?.Sum ?? 0;
            var i = income.FirstOrDefault(e => e.Month == m)?.Sum ?? 0;
            dtos.Add(new YearSurplusStatDto
            {
                Month = m,
                Surplus = (i - e).AmountFormat(),
                Expend = e.AmountFormat(),
                Income = i.AmountFormat(),
            });
        }
        return ServiceResult<List<YearSurplusStatDto>>.Successed(dtos);
    }

    public async Task<ServiceResult<MonthTotalTrendStatDto>> GetMonthTotalTrendStatAsync(MonthTotalTrendStatInput input)
    {
        var begin = input.Month.FirstDayOfMonth();
        var end = input.Month.LastDayOfMonth().AddDays(1).AddSeconds(-1);
        var days = DateTime.DaysInMonth(input.Month.Year, input.Month.Month);
        var dto = new MonthTotalTrendStatDto();

        // 构建Select
        ISelect<BillEntity> GetExpendSelect() => _billRepo
               .Select
               .Where(s => s.IsDeleted == false)
               .Where(s => s.CreateUserBId == CurrentUser.BId)
               .Where(s => s.Type == (int)BillTypeEnum.expend)
               .Where(s => s.Time <= end && s.Time >= begin);

        ISelect<BillEntity> GetIncomeSelect() => _billRepo
               .Select
               .Where(s => s.IsDeleted == false)
               .Where(s => s.CreateUserBId == CurrentUser.BId)
               .Where(s => s.Type == (int)BillTypeEnum.income)
               .Where(s => s.Time <= end && s.Time >= begin);

        // 查询趋势数据
        var expendTrend = await GetExpendSelect().GroupBy(s => s.Time.Date).ToListAsync(s => new { Date = s.Key, Sum = s.Sum(s.Value.Amount) });
        var incomeTrend = await GetIncomeSelect().GroupBy(s => s.Time.Date).ToListAsync(s => new { Date = s.Key, Sum = s.Sum(s.Value.Amount) });

        // 获取最高、最低金额
        dto.ExpendHighest = (await GetExpendSelect().MaxAsync(s => s.Amount)).AmountFormat();
        dto.ExpendLowst = (await GetExpendSelect().MinAsync(s => s.Amount)).AmountFormat();
        dto.IncomeHighest = (await GetIncomeSelect().MaxAsync(s => s.Amount)).AmountFormat();
        dto.IncomeLowst = (await GetIncomeSelect().MinAsync(s => s.Amount)).AmountFormat();

        var expendSerie = new BaseSerie { Name = "支出月趋势" };
        var incomeSerie = new BaseSerie { Name = "收入月趋势" };
        for (int d = 1; d <= days; d++)
        {
            var income = incomeTrend.FirstOrDefault(i => i.Date.Day == d)?.Sum ?? 0;
            var expend = expendTrend.FirstOrDefault(i => i.Date.Day == d)?.Sum ?? 0;
            dto.Categories.Add($"{d}日");
            incomeSerie.Data.Add(income);
            expendSerie.Data.Add(expend);
        }
        dto.Series.Add(expendSerie);
        dto.Series.Add(incomeSerie);

        return ServiceResult<MonthTotalTrendStatDto>.Successed(dto);
    }

    public async Task<ServiceResult<YearTotalTrendStatDto>> GetYearTotalTrendStatAsync(YearTotalTrendStatInput input)
    {
        var month = 12;
        var dto = new YearTotalTrendStatDto();

        // 构建Select
        ISelect<BillEntity> GetExpendSelect() => _billRepo
              .Select
              .Where(s => s.CreateUserBId == CurrentUser.BId)
              .Where(s => s.Type == (int)BillTypeEnum.expend)
              .Where(s => s.Time.Year == input.Year);

        ISelect<BillEntity> GetIncomeSelect() => _billRepo
               .Select
               .Where(s => s.CreateUserBId == CurrentUser.BId)
               .Where(s => s.Type == (int)BillTypeEnum.income)
               .Where(s => s.Time.Year == input.Year);

        // 查询趋势数据
        var expendTrend = await GetExpendSelect().GroupBy(s => s.Time.Month).ToListAsync(s => new { Date = s.Key, Sum = s.Sum(s.Value.Amount) });
        var incomeTrend = await GetIncomeSelect().GroupBy(s => s.Time.Month).ToListAsync(s => new { Date = s.Key, Sum = s.Sum(s.Value.Amount) });

        // 获取最高、最低金额
        dto.ExpendHighest = await GetExpendSelect().MaxAsync(s => s.Amount);
        dto.ExpendLowst = await GetExpendSelect().MinAsync(s => s.Amount);
        dto.IncomeHighest = await GetIncomeSelect().MaxAsync(s => s.Amount);
        dto.IncomeLowst = await GetIncomeSelect().MinAsync(s => s.Amount);

        var expendSerie = new BaseSerie { Name = "支出年趋势" };
        var incomeSerie = new BaseSerie { Name = "收入年趋势" };
        for (int m = 1; m <= month; m++)
        {
            var income = incomeTrend.FirstOrDefault(i => i.Date == m)?.Sum ?? 0;
            var expend = expendTrend.FirstOrDefault(i => i.Date == m)?.Sum ?? 0;
            dto.Categories.Add($"{m}月");
            incomeSerie.Data.Add(income);
            expendSerie.Data.Add(expend);
        }
        dto.Series.Add(expendSerie);
        dto.Series.Add(incomeSerie);

        return ServiceResult<YearTotalTrendStatDto>.Successed(dto);
    }

    public async Task<ServiceResult<CategoryPercentStatDto>> GetCategoryPercentStatAsync(CategoryPercentStatInput input)
    {

        var begin = input.Date.FirstDayOfMonth();
        var end = input.Date.LastDayOfMonth().AddDays(1).AddSeconds(-1);
        var dto = new CategoryPercentStatDto();
        ISelect<BillEntity, CategoryEntity> GetSelect() => _billRepo.Orm
              .Select<BillEntity>().From<CategoryEntity>((b, c) => b
              .LeftJoin(s => s.CategoryBId == c.BId)
              .Where(s => s.CreateUserBId == CurrentUser.BId)
              .WhereIf(input.BillType == 0, s => s.Type == (int)BillTypeEnum.expend)
              .WhereIf(input.BillType == 1, s => s.Type == (int)BillTypeEnum.income)
              .WhereIf(input.Type == 0, s => s.Time <= end && s.Time >= begin)
              .WhereIf(input.Type == 1, s => s.Time.Year == input.Date.Year));

        List<CategoryPercentSummaryDto> categoryGroups = null;
        if (input.SummaryType == 0)
        {
            categoryGroups = await GetSelect().GroupBy((b, c) => c.ParentBId).ToListAsync(b => new CategoryPercentSummaryDto { BId = b.Key, Sum = b.Sum(b.Value.Item1.Amount) });
        }
        else
        {
            categoryGroups = await GetSelect().GroupBy((b, c) => b.CategoryBId).ToListAsync(b => new CategoryPercentSummaryDto { BId = b.Key, Sum = b.Sum(b.Value.Item1.Amount) });
        }
        var catrgoryBIds = categoryGroups.Select(c => c.BId).ToList();
        var categories = await _categoryRepo.Select.Where(c => catrgoryBIds.Contains(c.BId)).DisableGlobalFilter("IsDeleted").ToListAsync();

        foreach (var category in categoryGroups)
        {
            var ca = categories.FirstOrDefault(c => c.BId == category.BId);
            dto.Series.Add(new RingSerie { Name = ca.Name, Value = category.Sum });
        }

        return ServiceResult<CategoryPercentStatDto>.Successed(dto);
    }

    public async Task<ServiceResult<List<CategoryPercentGroupDto>>> GetCategoryPercentGroupAsync(CategoryPercentGroupInput input)
    {
        var begin = input.Date.FirstDayOfMonth();
        var end = input.Date.LastDayOfMonth().AddDays(1).AddSeconds(-1);
        var dtos = new List<CategoryPercentGroupDto>();
        ISelect<BillEntity> GetSelect() => _billRepo
              .Select
              .Where(s => s.IsDeleted == false)
              .Where(s => s.CreateUserBId == CurrentUser.BId)
              .WhereIf(input.BillType == 0, s => s.Type == (int)BillTypeEnum.expend)
              .WhereIf(input.BillType == 1, s => s.Type == (int)BillTypeEnum.income)
              .WhereIf(input.Type == 0, s => s.Time <= end && s.Time >= begin)
              .WhereIf(input.Type == 1, s => s.Time.Year == input.Date.Year);

        var data = await GetSelect().GroupBy(s => s.CategoryBId).ToListAsync(s => new { BId = s.Key, Sum = s.Sum(s.Value.Amount) });
        var catrgoryBIds = data.Select(c => c.BId).ToList();
        var categories = await _categoryRepo.Select.Where(c => catrgoryBIds.Contains(c.BId)).DisableGlobalFilter("IsDeleted").ToListAsync();
        var parentIds = categories.Select(c => c.ParentBId).Distinct();
        var categoryParents = await _categoryRepo.Select.Where(c => parentIds.Contains(c.BId)).DisableGlobalFilter("IsDeleted").ToListAsync();
        var total = data.Sum(c => c.Sum);

        // 进行数据分组，并完善数据相关信息
        dtos = data.Select(cg =>
        {
            var c = categories.FirstOrDefault(c => c.BId == cg.BId);
            var g = categoryParents.FirstOrDefault(g => c.ParentBId == g.BId);
            return new
            {
                BId = cg.BId,
                CategoryName = c.Name,
                CategoryIcon = _fileRepo.GetFileUrl(c?.Icon),
                Sum = cg.Sum,
                GroupBId = g.BId,
                GroupName = g.Name,
                Percent = Math.Round((float)(cg.Sum / total) * 100, 2), // 保留两位小数的分类占比
                Amount = cg.Sum.AmountFormat()
            };
        })
        .GroupBy(cg => new { cg.GroupBId, cg.GroupName })
        .Select(g => new CategoryPercentGroupDto
        {
            Group = g.Key.GroupName,
            Amount = g.Sum(c => c.Sum).AmountFormat(),
            Items = g.Select(c => new CategoryPercentItemDto
            {
                BId = c.BId,
                Category = c.CategoryName,
                CategoryIcon = c.CategoryIcon,
                Percent = c.Percent,
                Amount = c.Amount,
            }).ToList(),
        }).ToList();

        return ServiceResult<List<CategoryPercentGroupDto>>.Successed(dtos);
    }

    public async Task<ServiceResult<PagedDto<BillSimpleDto>>> GetRankingAsync(RankingPagingInput input)
    {
        var begin = input.Date.FirstDayOfMonth();
        var end = input.Date.LastDayOfMonth().AddDays(1).AddSeconds(-1);
        var sort = "amount DESC";
        var bills = await _billRepo
            .Select
            .Include(b => b.Category)
            .Include(b => b.Asset)
            .Where(s => s.CreateUserBId == CurrentUser.BId)
            .WhereIf(input.BillType == 0, s => s.Type == (int)BillTypeEnum.expend)
            .WhereIf(input.BillType == 1, s => s.Type == (int)BillTypeEnum.income)
            .WhereIf(input.DateType == 0, s => s.Time <= end && s.Time >= begin)
            .WhereIf(input.DateType == 1, s => s.Time.Year == input.Date.Year)
            .WhereIf(input.CategoryBId.HasValue && input.CategoryBId != 0, s => s.CategoryBId == input.CategoryBId)
            .OrderBy(sort)
            .ToPageListAsync(input, out long totalCount);

        var dtos = Mapper.Map<List<BillSimpleDto>>(bills);
        return ServiceResult<PagedDto<BillSimpleDto>>.Successed(new PagedDto<BillSimpleDto>(dtos, totalCount));
    }
}
