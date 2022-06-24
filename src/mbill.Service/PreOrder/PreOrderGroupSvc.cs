namespace mbill.Service.PreOrder;

public class PreOrderGroupSvc : CrudApplicationSvc<PreOrderGroupEntity, PreOrderGroupDto, PreOrderGroupWithStatDto, long, CreatePreOrderGroupInput, UpdatePreOrderGroupInput>, IPreOrderGroupSvc
{
    private IPreOrderGroupRepo _groupRepo;
    private IPreOrderRepo _preOrderRepo;

    public PreOrderGroupSvc(
        IAuditBaseRepo<PreOrderGroupEntity, long> repository,
        IPreOrderGroupRepo preOrderGroupRepo,
        IPreOrderRepo preOrderRepo
        ) : base(repository)
    {
        _groupRepo = preOrderGroupRepo;
        _preOrderRepo = preOrderRepo;
    }

    public override async Task<ServiceResult<PreOrderGroupWithStatDto>> CreateAsync(CreatePreOrderGroupInput input)
    {
        var exist = await _groupRepo.Select.AnyAsync(g => g.Name.Equals(input.Name));
        if (exist) return ServiceResult<PreOrderGroupWithStatDto>.Failed("已存在同名分组");
        var result = await base.CreateAsync(input);
        var dto = result.Result;
        var week = dto.CreateTime.GetWeek();
        dto.Time = $"{week}-{dto.CreateTime.Day}日-{dto.CreateTime:HH:mm}";
        return result;
    }

    public override async Task<ServiceResult<PreOrderGroupWithStatDto>> UpdateAsync(long id, UpdatePreOrderGroupInput input)
    {
        var result = await base.UpdateAsync(id, input);
        var dto = result.Result;
        var week = dto.CreateTime.GetWeek();
        dto.Time = $"{week}-{dto.CreateTime.Day}日-{dto.CreateTime:HH:mm}";
        return result;
    }

    [Transactional]
    public override async Task DeleteAsync(long id)
    {
        var cnt = await _preOrderRepo.DeleteAsync(o => o.GroupId == id);
        await base.DeleteAsync(id);
    }

    public async Task<ServiceResult<PreOrderGroupDto>> GroupToBillAsync(GroupToBillInput input)
    {
        var entity = await _groupRepo.Select.Where(g => g.Id == input.Id).FirstAsync();
        if (entity == null) ServiceResult<PreOrderGroupWithPreAmountDto>.Failed("预购分组不存在");
        entity.BillId = input.BillId;
        var cnt = await _groupRepo.UpdateAsync(entity);
        if (cnt <= 0)
            return ServiceResult<PreOrderGroupDto>.Failed("转入账单失败");
        return ServiceResult<PreOrderGroupDto>.Successed(Mapper.Map<PreOrderGroupDto>(entity));
    }

    public async Task<ServiceResult<PreOrderGroupWithPreAmountDto>> GetGroupWithAmountAsync(long id)
    {
        var entity = await _groupRepo.Select.Where(g => g.Id == id).FirstAsync();
        if (entity == null) ServiceResult<PreOrderGroupWithPreAmountDto>.Failed("预购分组不存在");
        var orders = await _preOrderRepo.Select
            .Where(s => s.GroupId == id).ToListAsync();
        var pre = 0m;
        var real = 0m;
        foreach (var order in orders)
        {
            pre += order.PreAmount;
            real += order.RealAmount;
        }
        var dto = Mapper.Map<PreOrderGroupWithPreAmountDto>(entity);
        dto.PreAmount = pre;
        dto.RealAmount = real;
        dto.PreAmountFormate = pre.AmountFormat();
        dto.RealAmountFormate = real.AmountFormat();

        return ServiceResult<PreOrderGroupWithPreAmountDto>.Successed(dto);
    }

    public async Task<ServiceResult<PagedDto<PreOrderGroupWithStatDto>>> GetByMonthPagesAsync(MonthPreOrderGroupPagingInput input)
    {
        input.Sort = input.Sort.IsNullOrEmpty() ? "create_time DESC" : input.Sort.Replace("-", " ");
        var groups = await _groupRepo
            .Select
            .Where(s => s.CreateUserId == CurrentUser.Id)
            .Where(s => s.CreateTime.Year == input.Month.Year && s.CreateTime.Month == input.Month.Month)
            .WhereIf(!string.IsNullOrWhiteSpace(input.Name), s => s.Name.Contains(input.Name))
            .OrderBy(input.Sort)
            .ToPageListAsync(input, out long totalCount);
        var dtos = new List<PreOrderGroupWithStatDto>();
        foreach (var group in groups)
        {
            var dto = Mapper.Map<PreOrderGroupWithStatDto>(group);
            dto.Amount = await _preOrderRepo.GetPreAmountByGroupAsync(new List<long> { group.Id });
            var count = await _preOrderRepo.GetCountByStatusAsync(new List<long> { group.Id });
            var week = dto.CreateTime.GetWeek();
            dto.Time = $"{week}-{dto.CreateTime.Day}日-{dto.CreateTime:HH:mm}";
            dto.Done = count.done;
            dto.UnDone = count.unDone;
            dtos.Add(dto);
        }
        return ServiceResult<PagedDto<PreOrderGroupWithStatDto>>.Successed(new PagedDto<PreOrderGroupWithStatDto>(dtos, totalCount));
    }

    public async Task<ServiceResult<GroupPreOrderStatDto>> GetPreOrderStatAsync(GroupPreOrderStatInput input)
    {
        var group = await _groupRepo.GetAsync(input.Id);
        if (group == null) return ServiceResult<GroupPreOrderStatDto>.Failed("预购分组不存在");
        var dto = new GroupPreOrderStatDto { BillId = group.BillId };
        dto.GroupName = group.Name;
        var week = group.CreateTime.GetWeek();
        dto.Time = $"{week}-{group.CreateTime.ToString("yyyy-MM-dd")}日";
        dto.PreAmount = await _preOrderRepo.GetPreAmountByGroupAsync(new List<long> { input.Id });
        var count = await _preOrderRepo.GetCountByStatusAsync(new List<long> { input.Id });
        dto.Done = count.done;
        dto.UnDone = count.unDone;
        return ServiceResult<GroupPreOrderStatDto>.Successed(dto);
    }
}
