namespace Mbill.Service.PreOrder;

public interface IPreOrderGroupSvc : ICrudApplicationSvc<PreOrderGroupDto, PreOrderGroupWithStatDto, CreatePreOrderGroupInput, UpdatePreOrderGroupInput>
{
    /// <summary>
    /// 预购分组转账单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<PreOrderGroupDto>> GroupToBillAsync(GroupToBillInput input);

    /// <summary>
    /// 获取预购分组带组内预购总金额
    /// </summary>
    /// <param name="bId">预购分组Id</param>
    /// <returns></returns>
    Task<ServiceResult<PreOrderGroupWithPreAmountDto>> GetGroupWithAmountAsync(long bId);

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
