namespace Mbill.Service.PreOrder;

public interface IPreOrderSvc : ICrudApplicationSvc<PreOrderDto, PreOrderSimpleDto, long, CreatePreOrderInput, UpdatePreOrderInput>
{
    /// <summary>
    /// 更新预购状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<int>> UpdateStatusAsync(UpdatePreOrderStatusInput input);

    /// <summary>
    /// 获取指定月份分页预购分组
    /// </summary>
    /// <param name="input">分页查询</param>
    /// <returns></returns>
    Task<ServiceResult<PagedDto<PreOrderSimpleDto>>> GetByGroupPagesAsync(GroupPreOrderPagingInput input);


    /// <summary>
    /// 获取指定分组预购清单统计
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<IndexPreOrderStatDto>> GetIndexStatAsync(IndexPreOrderStatInput input);
}
