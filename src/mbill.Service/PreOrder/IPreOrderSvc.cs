namespace mbill.Service.PreOrder;

public interface IPreOrderSvc : ICrudApplicationSvc<PreOrderDto, PreOrderSimpleDto, long, CreatePreOrderInput, UpdatePreOrderInput>
{
    /// <summary>
    /// 获取指定月份分页预购分组
    /// </summary>
    /// <param name="input">分页查询</param>
    /// <returns></returns>
    Task<ServiceResult<PagedDto<PreOrderSimpleDto>>> GetByGroupPagesAsync(GroupPreOrderPagingInput input);
}
