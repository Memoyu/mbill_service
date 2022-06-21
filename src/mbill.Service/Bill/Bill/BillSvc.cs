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
        var assetDto = await _assetRepo.GetAssetAsync(dto.AssetId);
        dto.Asset = assetDto?.Name;
        dto.Category = category?.Name;
        dto.CategoryIcon = _fileRepo.GetFileUrl(category?.IconUrl);
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

        // 支出
        expend = await _billRepo
               .Select
               .Where(s => s.IsDeleted == false)
               .Where(s => s.CreateUserId == CurrentUser.Id)
                .Where(s => s.Type == (int)BillTypeEnum.expend)
               .Where(s => s.Time <= end && s.Time >= begin).SumAsync(e => e.Amount);

        // 收入
        income = await _billRepo
             .Select
             .Where(s => s.IsDeleted == false)
             .Where(s => s.CreateUserId == CurrentUser.Id)
             .Where(s => s.Type == (int)BillTypeEnum.income)
              .Where(s => s.Time <= end && s.Time >= begin).SumAsync(e => e.Amount);


        // 平均支出
        if (input.Opearte == 1)
            expendAvg = (decimal)await _billRepo
                   .Select
                   .Where(s => s.IsDeleted == false)
                   .Where(s => s.CreateUserId == CurrentUser.Id)
                   .Where(s => s.Type == (int)BillTypeEnum.expend)
                    .Where(s => s.Time <= end && s.Time >= begin).AvgAsync(e => e.Amount);


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
        var preOrder = 0m;

        // 支出
        expend = await _billRepo
               .Select
               .Where(s => s.IsDeleted == false)
               .Where(s => s.CreateUserId == CurrentUser.Id)
               .Where(s => s.Type == (int)BillTypeEnum.expend)
               .Where(s => s.Time.Year == input.Year).SumAsync(e => e.Amount);

        // 收入
        income = await _billRepo
             .Select
             .Where(s => s.IsDeleted == false)
             .Where(s => s.CreateUserId == CurrentUser.Id)
             .Where(s => s.Type == (int)BillTypeEnum.income)
             .Where(s => s.Time.Year == input.Year).SumAsync(e => e.Amount);

        // 预购总额
        preOrder = await _preOrderRepo
           .Select
           .Where(s => s.IsDeleted == false)
           .Where(s => s.CreateUserId == CurrentUser.Id)
           .Where(s => s.Time.Year == input.Year).SumAsync(e => e.Amount);

        if (input.Opearte == 1)
            // 平均支出
            expendAvg = (decimal)await _billRepo
               .Select
               .Where(s => s.IsDeleted == false)
               .Where(s => s.CreateUserId == CurrentUser.Id)
               .Where(s => s.Type == (int)BillTypeEnum.expend)
               .Where(s => s.Time.Year == input.Year).AvgAsync(e => e.Amount);

        return ServiceResult<YearTotalStatDto>.Successed(new YearTotalStatDto
        {
            Expend = expend.AmountFormat(),
            Income = income.AmountFormat(),
            PreOrder = preOrder.AmountFormat(),
            ExpendAvg = expendAvg.AmountFormat(),
        });
    }

    public async Task<ServiceResult<MonthTotalTrendStatDto>> GetMonthTotalTrendStatAsync(MonthTotalTrendStatInput input)
    {
        var begin = input.Month.FirstDayOfMonth();
        var end = input.Month.LastDayOfMonth().AddDays(1).AddSeconds(-1);
        var days = DateTime.DaysInMonth(input.Month.Year, input.Month.Month);
        var dto = new MonthTotalTrendStatDto();
        var expendTrend = await _billRepo
               .Select
               .Where(s => s.IsDeleted == false)
               .Where(s => s.CreateUserId == CurrentUser.Id)
               .Where(s => s.Type == (int)BillTypeEnum.expend)
               .Where(s => s.Time <= end && s.Time >= begin).GroupBy(s => s.Time.Date).ToListAsync(s => new { Date = s.Key, Sum = s.Sum(s.Value.Amount) });

        var incomeTrend = await _billRepo
               .Select
               .Where(s => s.IsDeleted == false)
               .Where(s => s.CreateUserId == CurrentUser.Id)
               .Where(s => s.Type == (int)BillTypeEnum.income)
               .Where(s => s.Time <= end && s.Time >= begin).GroupBy(s => s.Time.Date).ToListAsync(s => new { Date = s.Key, Sum = s.Sum(s.Value.Amount) });

        var expendSerie = new Serie { Name = "支出月趋势" };
        var incomeSerie = new Serie { Name = "收入月趋势" };
        for (int d = 1; d <= days; d++)
        {
            var income = incomeTrend.FirstOrDefault(i => i.Date.Day == d)?.Sum ?? 0 ;
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
        var months = 12;
        var dto = new YearTotalTrendStatDto();
        var expendTrend = await _billRepo
              .Select
              .Where(s => s.IsDeleted == false)
              .Where(s => s.CreateUserId == CurrentUser.Id)
              .Where(s => s.Type == (int)BillTypeEnum.expend)
              .Where(s => s.Time.Year == input.Year).GroupBy(s => s.Time.Month).ToListAsync(s => new { Date = s.Key, Sum = s.Sum(s.Value.Amount) });

        var incomeTrend = await _billRepo
               .Select
               .Where(s => s.IsDeleted == false)
               .Where(s => s.CreateUserId == CurrentUser.Id)
               .Where(s => s.Type == (int)BillTypeEnum.income)
               .Where(s => s.Time.Year == input.Year).GroupBy(s => s.Time.Month).ToListAsync(s => new { Date = s.Key, Sum = s.Sum(s.Value.Amount) });

        var expendSerie = new Serie { Name = "支出年趋势" };
        var incomeSerie = new Serie { Name = "收入年趋势" };
        for (int m = 1; m <= months; m++)
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

    public async Task<BillTotalDto> GetStatisticsTotalAsync(BillDateInput input)
    {
        // var userId = input.UserId ?? CurrentUser.Id;
        var bills = await _billRepo
           .Select
           .Where(s => s.IsDeleted == false)
           .WhereIf(input.UserId != null, s => s.CreateUserId == input.UserId)
           //.WhereIf(input.Year != null, s => s.Year == input.Year)
           //.WhereIf(input.Month != null, s => s.Month == input.Month)
           .ToListAsync();
        var dto = new BillTotalDto();
        bills.ForEach(s =>
        {
            switch (s.Type)
            {
                //case "expend":
                //    dto.MonthExpend += s.Amount;
                //    break;
                //case "income":
                //    dto.MonthIcome += s.Amount;
                //    break;
                //case "repayment":
                //    dto.MonthRepayment += s.Amount;
                //    break;
                //case "transfer":
                //    dto.MonthTransfer += s.Amount;
                //    break;
            }
            //if (input.Day != null && input.Day == s.Day)
            //{
            //    switch (s.Type)
            //    {
            //        case "expend":
            //            dto.DayExpend += s.Amount;
            //            break;
            //        case "income":
            //            dto.DayIcome += s.Amount;
            //            break;
            //        case "repayment":
            //            dto.DayRepayment += s.Amount;
            //            break;
            //        case "transfer":
            //            dto.DayTransfer += s.Amount;
            //            break;
            //    }
            //}
        });
        return dto;
    }

    public async Task<BillExpendCategoryDto> GetExpendCategoryStatisticsAsync(BillDateInput input)
    {
        var dto = new BillExpendCategoryDto();
        var bills = await _billRepo
           .Select
           .Where(s => s.IsDeleted == false)
           .Where(s => s.Type.Equals("expend"))
           .WhereIf(input.UserId != null, s => s.CreateUserId == input.UserId)
           //.WhereIf(input.Year != null, s => s.Year == input.Year)
           //.WhereIf(input.Month != null, s => s.Month == input.Month)
           //.WhereIf(input.Day != null, s => s.Day == input.Day)
           .ToListAsync();
        decimal total = 0;
        // 根据CategoryId分组，并统计总额
        var childDetails = bills.GroupBy(s => s.CategoryId).Select(g =>
        {
            var info = _categoryRepo.GetAsync(g.Key.Value).Result;
            var parentInfo = _categoryRepo.GetCategoryParentAsync(g.Key.Value).Result;
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
                CategoryIconPath = _fileRepo.GetFileUrl(d.Info.IconUrl)
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

    #region Private

    /// <summary>
    /// 映射Dto
    /// </summary>
    /// <param name="bill"></param>
    /// <returns></returns>
    private async Task<BillSimpleDto> MapToSimpleDto(BillEntity bill)
    {
        var dto = Mapper.Map<BillSimpleDto>(bill);
        dto.Time = bill.Time.ToString("HH:mm");
        if (bill.CategoryId.HasValue)
        {
            var category = await _categoryRepo.GetAsync(bill.CategoryId.Value);
            dto.Category = category?.Name;
            dto.CategoryIcon = _fileRepo.GetFileUrl(category?.IconUrl);
        }
        return dto;
    }

    #endregion

}
