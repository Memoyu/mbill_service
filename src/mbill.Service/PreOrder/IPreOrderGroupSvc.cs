namespace mbill.Service.PreOrder;

public interface IPreOrderGroupSvc : ICrudApplicationSvc<PreOrderGroupDto, long, CreatePreOrderGroupInput, UpdatePreOrderGroupInput>
{
    /// <summary>
    /// 获取指定月份分页预购分组
    /// </summary>
    /// <param name="input">分页查询</param>
    /// <returns></returns>
    Task<ServiceResult<PagedDto<PreOrderGroupWithStatDto>>> GetByMonthPagesAsync(MonthPreOrderPagingInput input);
}
