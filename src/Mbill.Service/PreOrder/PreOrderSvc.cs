namespace Mbill.Service.PreOrder;

public class PreOrderSvc : CrudApplicationSvc<PreOrderEntity, PreOrderDto, PreOrderSimpleDto, CreatePreOrderInput, UpdatePreOrderInput>, IPreOrderSvc
{
    private readonly IPreOrderRepo _orderRepo;
    private readonly IPreOrderGroupRepo _groupRepo;

    public PreOrderSvc(
        IAuditBaseRepo<PreOrderEntity, long> repository,
        IPreOrderRepo orderRepo,
        IPreOrderGroupRepo groupRepo) : base(repository)
    {
        _orderRepo = orderRepo;
        _groupRepo = groupRepo;
    }

    [Transactional]
    public async Task<ServiceResult> UpdateStatusAsync(UpdatePreOrderStatusInput input)
    {
        var order = await _orderRepo.GetPreOrderAsync(input.BId);
        order.Status = input.Status;

        ////如果将状态变更为未完成状态，则需要清空实际购买金额信息
        if (input.Status == (int)PreOrderStatusEnum.UnDone)
        {
            order.RealAmount = 0;
        }
        else if (input.Status == (int)PreOrderStatusEnum.Done)
        {
            order.RealAmount = input.RealAmount;
        }

        var cnt = await _orderRepo.UpdateAsync(order);
        if (cnt <= 0) return ServiceResult.Failed("更新状态失败");
        return ServiceResult.Successed();
    }

    public async Task<ServiceResult<PagedDto<PreOrderSimpleDto>>> GetByGroupPagesAsync(GroupPreOrderPagingInput input)
    {
        input.Sort = input.Sort.IsNullOrEmpty() ? "create_time DESC" : input.Sort.Replace("-", " ");
        var orders = await _orderRepo
            .Select
            .Where(s => s.CreateUserBId == CurrentUser.BId)
            .Where(s => s.GroupBId == input.BId)
            .WhereIf(input.Status.HasValue, s => s.Status == input.Status)
            .OrderBy(input.Sort)
            .ToPageListAsync(input, out long totalCount);
        var dtos = Mapper.Map<List<PreOrderSimpleDto>>(orders);
        return ServiceResult<PagedDto<PreOrderSimpleDto>>.Successed(new PagedDto<PreOrderSimpleDto>(dtos, totalCount));
    }

    public async Task<ServiceResult<IndexPreOrderStatDto>> GetIndexStatAsync(IndexPreOrderStatInput input)
    {
        var groups = await _groupRepo
            .Select
            .Where(s => s.CreateUserBId == CurrentUser.BId)
             .Where(s => s.CreateTime.Year == input.Month.Year && s.CreateTime.Month == input.Month.Month)
            .ToListAsync();
        var dto = new IndexPreOrderStatDto();
        dto.Total = groups.Count;
        dto.ToBill = groups.Where(g=> g.BillBId != 0).Count();
        var count = await _orderRepo.GetCountByStatusAsync(groups.Select(g => g.BId).ToList());
        dto.Done = count.done;
        dto.UnDone = count.unDone;
        return ServiceResult<IndexPreOrderStatDto>.Successed(dto);
    }
}
