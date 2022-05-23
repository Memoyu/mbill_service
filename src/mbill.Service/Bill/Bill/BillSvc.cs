namespace mbill.Service.Bill.Statement;

public class BillSvc : ApplicationSvc, IBillSvc
{
    private readonly IBillRepo _billRepo;
    private readonly ICategoryRepo _categoryRepo;
    private readonly IAssetRepo _assetRepo;
    private readonly IFileRepo _fileRepo;

    public BillSvc(
        IBillRepo billRepo,
        ICategoryRepo categoryRepo,
        IAssetRepo assetRepo,
        IFileRepo fileRepo)
    {
        _billRepo = billRepo;
        _categoryRepo = categoryRepo;
        _assetRepo = assetRepo;
        _fileRepo = fileRepo;
    }

    public async Task<BillDto> InsertAsync(BillEntity bill)
    {
        var entity = await _billRepo.InsertAsync(bill);
        if (entity == null) throw new KnownException("新增账单失败！", ServiceResultCode.Failed);
        var statementDto = MapToDto<BillDto>(entity);
        return statementDto;
    }

    public async Task DeleteAsync(long id)
    {
        var exist = await _billRepo.Select.AnyAsync(s => s.Id == id && !s.IsDeleted);
        if (!exist) throw new KnownException("没有找到该账单信息", ServiceResultCode.NotFound);
        await _billRepo.DeleteAsync(id);
    }

    public async Task UpdateAsync(BillEntity bill)
    {
        var exist = await _billRepo.Select.AnyAsync(s => s.Id == bill.Id && !s.IsDeleted);
        if (!exist) throw new KnownException("没有找到该账单信息", ServiceResultCode.NotFound);
        Expression<Func<BillEntity, object>> ignoreExp = e => new { e.CreateUserId, e.CreateTime };
        await _billRepo.UpdateWithIgnoreAsync(bill, ignoreExp);
    }

    public async Task<BillDetailDto> GetDetailAsync(long id)
    {
        var bill = await _billRepo.GetAsync(id);
        var dto = MapToDto<BillDetailDto>(bill);
        var assetDto = await _assetRepo.GetAssetParentAsync(dto.AssetId);
        var categoryDto = await _categoryRepo.GetCategoryParentAsync(dto.CategoryId);
        dto.assetParentName = assetDto?.Name;
        dto.categoryParentName = categoryDto?.Name;
        return dto;
    }

    public async Task<PagedDto<BillDto>> GetPagesAsync(BillPagingDto pageDto)
    {
        // pageDto.UserId = pageDto.UserId ?? CurrentUser.Id;
        pageDto.Sort = pageDto.Sort.IsNullOrEmpty() ? "Time DESC" : pageDto.Sort.Replace("-", " ");
        var statements = await _billRepo
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
        List<BillDto> statementDtos = statements
            .Select(s =>
            {
                var dto = MapToDto<BillDto>(s);
                return dto;
            })
            .ToList();

        return new PagedDto<BillDto>(statementDtos, totalCount);
    }

    public async Task<BillTotalDto> GetStatisticsTotalAsync(BillDateInputDto input)
    {
        // var userId = input.UserId ?? CurrentUser.Id;
        var statements = await _billRepo
           .Select
           .Where(s => s.IsDeleted == false)
           .WhereIf(input.UserId != null, s => s.CreateUserId == input.UserId)
           .WhereIf(input.Year != null, s => s.Year == input.Year)
           .WhereIf(input.Month != null, s => s.Month == input.Month)
           .ToListAsync();
        var dto = new BillTotalDto();
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

    public async Task<BillExpendCategoryDto> GetExpendCategoryStatisticsAsync(BillDateInputDto input)
    {
        var dto = new BillExpendCategoryDto();
        var statements = await _billRepo
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

    public async Task<IEnumerable<BillExpendTrendDto>> GetWeekExpendTrendStatisticsAsync(BillDateInputDto input)
    {

        var dateList = DateTimeUtil.GetWeeksOfMonth(input.Year.Value, input.Month.Value);
        var startDate = dateList.OrderBy(d => d.Number).FirstOrDefault().StartDate.Date;
        var EndDate = dateList.OrderBy(d => d.Number).LastOrDefault().EndDate.Date.AddDays(1).AddSeconds(-1);// 加上23:59:59

        var statements = await _billRepo
          .Select
          .Where(s => s.IsDeleted == false)
          .WhereIf(input.Type.IsNotNullOrEmpty(), s => s.Type == input.Type)
          .WhereIf(input.UserId != null, s => s.CreateUserId == input.UserId)
          .WhereIf(input.Year != null, s => s.Year == input.Year)
          .WhereIf(input.Month != null, s => s.Time >= startDate && s.Time <= EndDate)
          .ToListAsync();

        var dtos = dateList.Select(d => new BillExpendTrendDto
        {
            Name = $"{d.StartDate.Month}/{d.StartDate.Day}-{d.EndDate.Month}/{d.EndDate.Day}",
            Data = statements.Where(s => s.Time >= d.StartDate && s.Time <= d.EndDate.AddDays(1).AddSeconds(-1)).Select(t => new { t.Amount }).Sum(t => t.Amount),
            StartDate = d.StartDate.Date,
            EndDate = d.EndDate.Date
        });

        return dtos;
    }

    public async Task<IEnumerable<BillExpendTrendDto>> GetMonthExpendTrendStatisticsAsync(BillDateInputDto input, int count)
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

        var bills = await _billRepo
         .Select
         .Where(s => s.IsDeleted == false)
         .WhereIf(input.Type.IsNotNullOrEmpty(), s => s.Type == input.Type)
         .WhereIf(input.UserId != null, s => s.CreateUserId == input.UserId)
         .WhereIf(input.Year != null, s => s.Year >= startDate.Year && s.Year <= EndDate.Year)
         .WhereIf(input.Month != null, s => s.Time >= startDate && s.Time <= EndDate.AddDays(1).AddSeconds(-1))
         .ToListAsync();

        var dtos = dateList.Select(d => new BillExpendTrendDto
        {
            Name = $"{d.StartDate.Year}/{d.StartDate.Month}",
            Data = bills.Where(s => s.Year == d.StartDate.Year && s.Month == d.StartDate.Month).Select(t => new { t.Amount }).Sum(t => t.Amount),
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
    /// <param name="bill"></param>
    /// <returns></returns>
    private T MapToDto<T>(BillEntity bill) where T : MapDto
    {
        T dto = Mapper.Map<T>(bill);
        if (bill.CategoryId != null)
        {
            var category = _categoryRepo.Get(bill.CategoryId.Value) ?? throw new KnownException("账单分类数据查询失败！", ServiceResultCode.NotFound);
            dto.CategoryName = category.Name;
            dto.CategoryIconPath = _fileRepo.GetFileUrl(category.IconUrl);
        }
        else
        {
            if (bill.Type.Equals(BillTypeEnum.transfer.ToString()))
            {
                dto.CategoryIconPath = _fileRepo.GetFileUrl("core/images/category/icon_transfer_64.png");
            }
            else if (bill.Type.Equals(BillTypeEnum.repayment.ToString()))
            {
                dto.CategoryIconPath = _fileRepo.GetFileUrl("core/images/category/icon_repayment_64.png");
            }
        }
        var asset = _assetRepo.Get(bill.AssetId) ?? throw new KnownException("资产分类数据查询失败！", ServiceResultCode.NotFound);
        if (bill.TargetAssetId != null)
        {
            var targetAsset = _assetRepo.Get(bill.TargetAssetId.Value);
            dto.TargetAssetName = targetAsset.Name;
        }
        dto.AssetName = asset.Name;
        dto.TypeName = Switcher.StatementType(bill.Type);

        return dto;
    }

    #endregion

}
