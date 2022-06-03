namespace mbill.Service.PreOrder;

public class PreOrderSvc : CrudApplicationSvc<PreOrderEntity, PreOrderDto, PreOrderSimpleDto, long, CreatePreOrderInput, UpdatePreOrderInput>, IPreOrderSvc
{
    private readonly IPreOrderRepo _orderRepo;

    public PreOrderSvc(
        IAuditBaseRepo<PreOrderEntity, long> repository,
        IPreOrderRepo orderRepo,
        IPreOrderGroupRepo preOrderGroupRepo
        ) : base(repository)
    {
        _orderRepo = orderRepo;
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
}
