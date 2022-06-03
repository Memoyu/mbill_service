namespace mbill.Service.PreOrder;

public class PreOrderSvc : CrudApplicationSvc<PreOrderEntity, PreOrderDto, PreOrderSimpleDto, long, CreatePreOrderInput, UpdatePreOrderInput>, IPreOrderSvc
{
    private readonly IPreOrderRepo _orderRepo;
    private readonly IPreOrderGroupRepo _groupRepo;

    public PreOrderSvc(
        IAuditBaseRepo<PreOrderEntity, long> repository,
        IPreOrderRepo orderRepo,
        IPreOrderGroupRepo groupRepo
        ) : base(repository)
    {
        _orderRepo = orderRepo;
        _groupRepo = groupRepo;
    }

    public override async Task<ServiceResult<PreOrderSimpleDto>> CreateAsync(CreatePreOrderInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Color))
            input.Color = $"#{ColorUtil.GetRandomColor()}";

        var result = await base.CreateAsync(input);
        var dto = result.Result;
        var week = input.Time.GetWeek();
        dto.Time = $"{week}-{input.Time.ToString("yyyy年MM月dd日")}";
        return result;
    }

    public override async Task<ServiceResult<PreOrderSimpleDto>> UpdateAsync(long id, UpdatePreOrderInput input)
    {
        var result = await base.UpdateAsync(id, input);
        var dto = result.Result;
        var week = input.Time.GetWeek();
        dto.Time = $"{week}-{input.Time.ToString("yyyy年MM月dd日")}";
        return result;
    }

    public async Task<ServiceResult<int>> UpdateStatusAsync(UpdatePreOrderStatusInput input)
    {
        var order = await _orderRepo.GetAsync(input.Id);
        order.Status = input.Status;
        var cnt = await _orderRepo.UpdateAsync(order);
        if (cnt <= 0) return ServiceResult<int>.Failed("更新状态失败");
        return ServiceResult<int>.Successed(1);
    }

    public async Task<ServiceResult<PagedDto<PreOrderSimpleDto>>> GetByGroupPagesAsync(GroupPreOrderPagingInput input)
    {
        input.Sort = input.Sort.IsNullOrEmpty() ? "create_time DESC" : input.Sort.Replace("-", " ");
        var orders = await _orderRepo
            .Select
            .Where(s => s.IsDeleted == false)
            .Where(s => s.CreateUserId == CurrentUser.Id)
            .Where(s => s.GroupId == input.Id)
            .WhereIf(input.Status.HasValue, s => s.Status == input.Status)
            .OrderBy(input.Sort)
            .ToPageListAsync(input, out long totalCount);
        var dtos = new List<PreOrderSimpleDto>();
        foreach (var order in orders)
        {
            var dto = Mapper.Map<PreOrderSimpleDto>(order);
            var week = order.Time.GetWeek();
            dto.Time = $"{week}-{order.Time.ToString("yyyy年MM月dd日")}";
            dtos.Add(dto);
        }
        return ServiceResult<PagedDto<PreOrderSimpleDto>>.Successed(new PagedDto<PreOrderSimpleDto>(dtos, totalCount));
    }

    public async Task<ServiceResult<IndexPreOrderStatDto>> GetIndexStatAsync(IndexPreOrderStatInput input)
    {
        var groups = await _groupRepo
            .Select
            .Where(s => s.IsDeleted == false)
            .Where(s => s.CreateUserId == CurrentUser.Id)
             .Where(s => s.CreateTime.Year == input.Month.Year && s.CreateTime.Month == input.Month.Month)
            .ToListAsync();
        var dto = new IndexPreOrderStatDto();
        dto.Total = groups.Count;
        dto.Amount = await _orderRepo.GetAmountByGroupAsync(groups.Select(g => g.Id).ToList());
        var count = await _orderRepo.GetCountByStatusAsync(groups.Select(g => g.Id).ToList());
        dto.Done = count.done;
        dto.UnDone = count.unDone;
        return ServiceResult<IndexPreOrderStatDto>.Successed(dto);
    }
}
