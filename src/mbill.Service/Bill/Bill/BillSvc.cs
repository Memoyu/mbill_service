namespace mbill.Service.Bill.Bill;

public class BillSvc : ApplicationSvc, IBillSvc
{
    private readonly IBillRepo _billRepo;
    private readonly IPreOrderRepo _preOrderRepo;
    private readonly ICategoryRepo _categoryRepo;
    private readonly IAssetRepo _assetRepo;
    private readonly IFileRepo _fileRepo;

    public BillSvc(
        IBillRepo billRepo,
        IPreOrderRepo preOrderRepo,
        ICategoryRepo categoryRepo,
        IAssetRepo assetRepo,
        IFileRepo fileRepo)
    {
        _billRepo = billRepo;
        _preOrderRepo = preOrderRepo;
        _categoryRepo = categoryRepo;
        _assetRepo = assetRepo;
        _fileRepo = fileRepo;
    }

    public async Task<ServiceResult<BillSimpleDto>> CreateAsync(ModifyBillInput input)
    {
        var bill = Mapper.Map<BillEntity>(input);
        var entity = await _billRepo.InsertAsync(bill);
        if (entity == null) ServiceResult<BillSimpleDto>.Failed("新增账单失败！");
        return ServiceResult<BillSimpleDto>.Successed(await MapToSimpleDto(entity));
    }

    public async Task<ServiceResult> DeleteAsync(long id)
    {
        var exist = await _billRepo.Select.AnyAsync(s => s.Id == id && !s.IsDeleted);
        if (!exist) return ServiceResult.Failed("没有找到该账单信息");
        var re = await _billRepo.DeleteAsync(id);
        return ServiceResult.Successed("删除成功");
    }

    public async Task<ServiceResult<BillSimpleDto>> UpdateAsync(ModifyBillInput input)
    {
        var bill = Mapper.Map<BillEntity>(input);
        var exist = await _billRepo.Select.AnyAsync(s => s.Id == bill.Id && !s.IsDeleted);
        if (!exist) return ServiceResult<BillSimpleDto>.Failed("没有找到该账单信息");
        Expression<Func<BillEntity, object>> ignoreExp = e => new { e.CreateUserId, e.CreateTime };
        await _billRepo.UpdateWithIgnoreAsync(bill, ignoreExp);
        return ServiceResult<BillSimpleDto>.Successed(await MapToSimpleDto(bill));
    }

    public async Task<ServiceResult<BillDetailDto>> GetDetailAsync(long id)
    {
        var bill = await _billRepo.GetAsync(id);
        if (bill == null)
            return ServiceResult<BillDetailDto>.Failed("没有找到该账单信息");
        var dto = Mapper.Map<BillDetailDto>(bill);
        var category = await _categoryRepo.GetAsync(bill.CategoryId.Value);
        var asset = await _assetRepo.GetAssetAsync(dto.AssetId);
        dto.Asset = asset?.Name;
        dto.Category = category?.Name;
        dto.CategoryIcon = _fileRepo.GetFileUrl(category?.Icon);
        dto.AssetIcon = _fileRepo.GetFileUrl(asset?.Icon);
        return ServiceResult<BillDetailDto>.Successed(dto);
    }

    public async Task<ServiceResult<BillsByDayWithStatDto>> GetByDayAsync(DayBillInput input)
    {
        var begin = input.Date.Date;
        var end = begin.AddDays(1).AddSeconds(-1);
        var bills = await _billRepo
            .Select
            .Where(s => s.IsDeleted == false)
            .Where(s => s.CreateUserId == CurrentUser.Id)
            .Where(s => s.Time >= begin && s.Time <= end)
            .WhereIf(input.Type.HasValue, s => s.Type == input.Type)
            .WhereIf(input.CategoryId.HasValue, s => s.CategoryId == input.CategoryId)
            .WhereIf(input.AssetId.HasValue, s => s.AssetId == input.AssetId)
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
            dto.Items.Add(await MapToSimpleDto(i));
        }
        dto.Expend = expend.AmountFormat();
        dto.Income = income.AmountFormat();
        return ServiceResult<BillsByDayWithStatDto>.Successed(dto);
    }

    public async Task<ServiceResult<PagedDto<BillSimpleDto>>> GetPagesAsync(BillPagingInput input)
    {
        input.Sort = input.Sort.IsNullOrEmpty() ? "time DESC" : input.Sort.Replace("-", " ");
        var bills = await _billRepo
            .Select
            .Where(s => s.IsDeleted == false)
            .Where(s => s.CreateUserId == CurrentUser.Id)
            .WhereIf(input.DateType == 0, s => s.Time.Year == input.Date.Year && s.Time.Month == input.Date.Month)
            .WhereIf(input.DateType == 1, s => s.Time.Year == input.Date.Year)
            .WhereIf(input.Type.HasValue, s => s.Type == input.Type)
            .WhereIf(input.CategoryId.HasValue, s => s.CategoryId == input.CategoryId)
            .WhereIf(input.AssetId.HasValue, s => s.AssetId == input.AssetId)
            .OrderBy(input.Sort)
            .ToPageListAsync(input, out long totalCount);

        List<BillSimpleDto> dtos = new List<BillSimpleDto>();
        foreach (var i in bills)
            dtos.Add(await MapToSimpleDto(i));
        return ServiceResult<PagedDto<BillSimpleDto>>.Successed(new PagedDto<BillSimpleDto>(dtos, totalCount));
    }

    public async Task<ServiceResult<PagedDto<BillsByDayDto>>> GetByMonthPagesAsync(MonthBillPagingInput input)
    {
        input.Sort = input.Sort.IsNullOrEmpty() ? "time DESC" : input.Sort.Replace("-", " ");
        var bills = await _billRepo
            .Select
            .Where(s => s.IsDeleted == false)
            .Where(s => s.CreateUserId == CurrentUser.Id)
            .Where(s => s.Time.Year == input.Month.Year && s.Time.Month == input.Month.Month)
            .WhereIf(input.Type.HasValue, s => s.Type == input.Type)
            .WhereIf(input.CategoryId.HasValue, s => s.CategoryId == input.CategoryId)
            .WhereIf(input.AssetId.HasValue, s => s.AssetId == input.AssetId)
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
                dto.Items.Add(await MapToSimpleDto(i));
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
            .Where(s => s.IsDeleted == false)
            .Where(s => s.CreateUserId == CurrentUser.Id)
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
               .Where(s => s.CreateUserId == CurrentUser.Id)
                .Where(s => s.Type == (int)BillTypeEnum.expend)
               .Where(s => s.Time <= end && s.Time >= begin);

        // 支出
        expend = await GetExpendSelect().SumAsync(e => e.Amount);

        // 收入
        income = await _billRepo
             .Select
             .Where(s => s.IsDeleted == false)
             .Where(s => s.CreateUserId == CurrentUser.Id)
             .Where(s => s.Type == (int)BillTypeEnum.income)
              .Where(s => s.Time <= end && s.Time >= begin).SumAsync(e => e.Amount);


        // 平均支出
        if (input.Opearte == 1)
            expendAvg = (decimal)await GetExpendSelect().AvgAsync(e => e.Amount);


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
               .Where(s => s.CreateUserId == CurrentUser.Id)
               .Where(s => s.Type == (int)BillTypeEnum.expend)
               .Where(s => s.Time.Year == input.Year);

        // 支出
        expend = await GetExpendSelect().SumAsync(e => e.Amount);

        // 收入
        income = await _billRepo
             .Select
             .Where(s => s.IsDeleted == false)
             .Where(s => s.CreateUserId == CurrentUser.Id)
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
              .Where(s => s.CreateUserId == CurrentUser.Id)
              .Where(s => s.Time.Year == year);

        // 计算收、支总额
        var expend = await GetSelect().Where(s => s.Type == (int)BillTypeEnum.expend).GroupBy(s => s.Time.Month).ToListAsync(s => new { Month = s.Key, Sum = s.Sum(s.Value.Amount) });
        var income = await GetSelect().Where(s => s.Type == (int)BillTypeEnum.income).GroupBy(s => s.Time.Month).ToListAsync(s => new { Month = s.Key, Sum = s.Sum(s.Value.Amount) });

        // 计算收入总额
        var totalIncome = income.Sum(i => i.Sum);

        var month = 12;
        var curYear = DateTime.Now.Year;
        if (curYear == year) month = DateTime.Now.Month;
        var curExpend = 0m;
        // 构建返回Dto
        for (int m = 1; m <= month; m++)
        {
            var e = expend.FirstOrDefault(e => e.Month == m)?.Sum ?? 0;
            curExpend += e;
            dtos.Add(new YearSurplusStatDto
            {
                Month = m,
                Surplus = (totalIncome - curExpend).AmountFormat(),
                Expend = e.AmountFormat(),
                Income = (income.FirstOrDefault(e => e.Month == m)?.Sum ?? 0).AmountFormat(),
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
               .Where(s => s.CreateUserId == CurrentUser.Id)
               .Where(s => s.Type == (int)BillTypeEnum.expend)
               .Where(s => s.Time <= end && s.Time >= begin);

        ISelect<BillEntity> GetIncomeSelect() => _billRepo
               .Select
               .Where(s => s.IsDeleted == false)
               .Where(s => s.CreateUserId == CurrentUser.Id)
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
              .Where(s => s.CreateUserId == CurrentUser.Id)
              .Where(s => s.Type == (int)BillTypeEnum.expend)
              .Where(s => s.Time.Year == input.Year);

        ISelect<BillEntity> GetIncomeSelect() => _billRepo
               .Select
               .Where(s => s.CreateUserId == CurrentUser.Id)
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
              .LeftJoin(s => s.CategoryId == c.Id)
              .Where(s => s.CreateUserId == CurrentUser.Id)
              .WhereIf(input.BillType == 0, s => s.Type == (int)BillTypeEnum.expend)
              .WhereIf(input.BillType == 1, s => s.Type == (int)BillTypeEnum.income)
              .WhereIf(input.Type == 0, s => s.Time <= end && s.Time >= begin)
              .WhereIf(input.Type == 1, s => s.Time.Year == input.Date.Year));
        List<CategoryPercentSummaryDto> categoryGroups = null;
        if (input.SummaryType == 0)
        {
            categoryGroups = await GetSelect().GroupBy((b, c) => c.ParentId).ToListAsync(b => new CategoryPercentSummaryDto { Id = b.Key, Sum = b.Sum(b.Value.Item1.Amount) });
        }
        else
        {
            categoryGroups = await GetSelect().GroupBy((b, c) => b.CategoryId).ToListAsync(b => new CategoryPercentSummaryDto { Id = b.Key.Value, Sum = b.Sum(b.Value.Item1.Amount) });
        }
        var catrgoryIds = categoryGroups.Select(c => c.Id).ToList();
        var categories = await _categoryRepo.Select.Where(c => catrgoryIds.Contains(c.Id)).DisableGlobalFilter("IsDeleted").ToListAsync();

        foreach (var category in categoryGroups)
        {
            var ca = categories.FirstOrDefault(c => c.Id == category.Id);
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
              .Where(s => s.CreateUserId == CurrentUser.Id)
              .WhereIf(input.BillType == 0, s => s.Type == (int)BillTypeEnum.expend)
              .WhereIf(input.BillType == 1, s => s.Type == (int)BillTypeEnum.income)
              .WhereIf(input.Type == 0, s => s.Time <= end && s.Time >= begin)
              .WhereIf(input.Type == 1, s => s.Time.Year == input.Date.Year);

        var data = await GetSelect().GroupBy(s => s.CategoryId).ToListAsync(s => new { Id = s.Key, Sum = s.Sum(s.Value.Amount) });
        var catrgoryIds = data.Select(c => c.Id).ToList();
        var categories = await _categoryRepo.Select.Where(c => catrgoryIds.Contains(c.Id)).DisableGlobalFilter("IsDeleted").ToListAsync();
        var parentIds = categories.Select(c => c.ParentId).Distinct();
        var categoryParents = await _categoryRepo.Select.Where(c => parentIds.Contains(c.Id)).DisableGlobalFilter("IsDeleted").ToListAsync();
        var total = data.Sum(c => c.Sum);

        // 进行数据分组，并完善数据相关信息
        dtos = data.Select(cg =>
        {
            var c = categories.FirstOrDefault(c => c.Id == cg.Id);
            var g = categoryParents.FirstOrDefault(g => c.ParentId == g.Id);
            return new
            {
                Id = cg.Id,
                CategoryName = c.Name,
                CategoryIcon = _fileRepo.GetFileUrl(c?.Icon),
                Sum = cg.Sum,
                GroupId = g.Id,
                GroupName = g.Name,
                Percent = Math.Round((float)(cg.Sum / total) * 100, 2), // 保留两位小数的分类占比
                Amount = cg.Sum.AmountFormat()
            };
        })
        .GroupBy(cg => new { cg.GroupId, cg.GroupName })
        .Select(g => new CategoryPercentGroupDto
        {
            Group = g.Key.GroupName,
            Amount = g.Sum(c => c.Sum).AmountFormat(),
            Items = g.Select(c => new CategoryPercentItemDto
            {
                Id = c.Id.Value,
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
            .Where(s => s.IsDeleted == false)
            .Where(s => s.CreateUserId == CurrentUser.Id)
            .WhereIf(input.BillType == 0, s => s.Type == (int)BillTypeEnum.expend)
            .WhereIf(input.BillType == 1, s => s.Type == (int)BillTypeEnum.income)
            .WhereIf(input.DateType == 0, s => s.Time <= end && s.Time >= begin)
            .WhereIf(input.DateType == 1, s => s.Time.Year == input.Date.Year)
            .WhereIf(input.CategoryId.HasValue && input.CategoryId != 0, s => s.CategoryId == input.CategoryId)
            .OrderBy(sort)
            .ToPageListAsync(input, out long totalCount);

        var dtos = bills.Select(b => MapToSimpleDto(b).GetAwaiter().GetResult()).ToList();
        return ServiceResult<PagedDto<BillSimpleDto>>.Successed(new PagedDto<BillSimpleDto>(dtos, totalCount));
    }

    #region Private

    /// <summary>
    /// 映射Dto
    /// </summary>
    /// <param name="bill"></param>
    /// <returns></returns>
    private async Task<BillSimpleDto> MapToSimpleDto(BillEntity bill)
    {
        var dto = Mapper.Map<BillSimpleDto>(bill);
        dto.Date = bill.Time.ToString("yyyy-MM-dd");
        dto.Time = bill.Time.ToString("HH:mm");
        if (bill.CategoryId.HasValue)
        {
            var category = await _categoryRepo.GetAsync(bill.CategoryId.Value);
            dto.Category = category?.Name;
            dto.CategoryIcon = _fileRepo.GetFileUrl(category?.Icon);
        }
        return dto;
    }

    #endregion
}
