namespace mbill.Service.PreOrder;

public class PreOrderGroupSvc : CrudApplicationSvc<PreOrderGroupEntity, PreOrderGroupDto, long, CreatePreOrderGroupInput, UpdatePreOrderGroupInput>, IPreOrderGroupSvc
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

    public override async Task<ServiceResult<PreOrderGroupDto>> CreateAsync(CreatePreOrderGroupInput input)
    {
        var exist = await _groupRepo.Select.AnyAsync(g => g.Name.Equals(input.Name));
        if (exist) return ServiceResult<PreOrderGroupDto>.Failed("已存在同名分组");
        return await base.CreateAsync(input);
    }

    public async Task<ServiceResult<PagedDto<PreOrderGroupWithStatDto>>> GetByMonthPagesAsync(MonthPreOrderPagingInput input)
    {
        input.Sort = input.Sort.IsNullOrEmpty() ? "create_time DESC" : input.Sort.Replace("-", " ");
        var groups = await _groupRepo
            .Select
            .Where(s => s.IsDeleted == false)
            .Where(s => s.CreateUserId == CurrentUser.Id)
             .Where(s => s.CreateTime.Year == input.Month.Year && s.CreateTime.Month == input.Month.Month)
            .WhereIf(!string.IsNullOrWhiteSpace(input.Name), s => s.Name.Contains(input.Name))
            .OrderBy(input.Sort)
            .ToPageListAsync(input, out long totalCount);
        var dtos = new List<PreOrderGroupWithStatDto>();
        foreach (var group in groups)
        {
            var dto = Mapper.Map<PreOrderGroupWithStatDto>(group);
            var count = await _preOrderRepo.GetCountByStatusAsync((int)PreOrderStatusEnum.Done);
            var week = dto.CreateTime.GetWeek();
            dto.Time = $"{week}-{dto.CreateTime.Day}日-{dto.CreateTime:HH:mm}";
            dto.Done = count.done;
            dto.UnDone = count.unDone;
            dtos.Add(dto);
        }
        return ServiceResult<PagedDto<PreOrderGroupWithStatDto>>.Successed(new PagedDto<PreOrderGroupWithStatDto>(dtos, totalCount));
    }
}
