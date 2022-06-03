namespace mbill.Service.PreOrder;

public interface IPreOrderGroupSvc : ICrudApplicationSvc<PreOrderGroupDto, PreOrderGroupDto, long, CreatePreOrderGroupInput, UpdatePreOrderGroupInput>
{
    /// <summary>
    /// 获取指定月份分页预购分组
    /// </summary>
    /// <param name="input">分页查询</param>
    /// <returns></returns>
    Task<ServiceResult<PagedDto<PreOrderGroupWithStatDto>>> GetByMonthPagesAsync(MonthPreOrderGroupPagingInput input);

    /// <summary>
    /// 获取指定分组预购清单统计
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<GroupPreOrderStatDto>> GetPreOrderStatAsync(GroupPreOrderStatInput input);
}
