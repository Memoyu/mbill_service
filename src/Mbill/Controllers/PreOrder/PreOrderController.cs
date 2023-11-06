namespace Mbill.Controllers.PreOrder;

/// <summary>
/// 预购清单管理
/// </summary>
[Authorize]
[Route("api/pre-order")]
[ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
public class PreOrderController : ApiControllerBase
{
    private readonly IPreOrderSvc _preOrderSvc;
    private readonly IPreOrderGroupSvc _preOrderGroupSvc;

    public PreOrderController(IPreOrderSvc preOrderSvc, IPreOrderGroupSvc preOrderGroupSvc)
    {
        _preOrderSvc = preOrderSvc;
        _preOrderGroupSvc = preOrderGroupSvc;
    }

    #region 预购

    /// <summary>
    /// 新增预购
    /// </summary>
    /// <param name="input">账单</param>
    [HttpPost]
    [LocalAuthorize("新增", "预购")]
    public async Task<ServiceResult<PreOrderSimpleDto>> CreateAsync([FromBody] CreatePreOrderInput input)
    {
        return await _preOrderSvc.CreateAsync(input);
    }

    /// <summary>
    /// 获取预购详情
    /// </summary>
    /// <param name="id">账单id</param>
    [HttpGet]
    [LocalAuthorize("详情", "预购")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<PreOrderDto>> GetAsync([FromQuery] long id)
    {
        return await _preOrderSvc.GetAsync(id);
    }

    /// <summary> 
    /// 删除预购信息
    /// </summary>
    /// <param name="id">账单id</param>
    [HttpDelete]
    [LocalAuthorize("删除", "预购")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult> DeleteAsync([FromBody] long id)
    {
        await _preOrderSvc.DeleteAsync(id);
        return ServiceResult.Successed("预购删除成功！");
    }

    /// <summary>
    /// 更新预购信息
    /// </summary>
    /// <param name="input">账单信息</param>
    [HttpPut]
    [LocalAuthorize("更新", "预购")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<PreOrderSimpleDto>> UpdateAsync([FromBody] UpdatePreOrderInput input)
    {
        return await _preOrderSvc.UpdateAsync(input);
    }

    /// <summary>
    /// 更新预购状态
    /// </summary>
    /// <param name="input">账单信息</param>
    [HttpPut("status")]
    [LocalAuthorize("更新状态", "预购")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult> UpdateStatusAsync([FromBody] UpdatePreOrderStatusInput input)
    {
        return await _preOrderSvc.UpdateStatusAsync(input);
    }


    /// <summary>
    /// 获取指定分组分页预购
    /// </summary>
    /// <param name="input">分页条件</param>
    [HttpGet("pages")]
    [LocalAuthorize("获取指定分组分页预购", "预购")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<PagedDto<PreOrderSimpleDto>>> GetByGroupPagesAsync([FromQuery] GroupPreOrderPagingInput input)
    {
        return await _preOrderSvc.GetByGroupPagesAsync(input);
    }

    /// <summary>
    /// 获取预购清单首页统计
    /// </summary>
    /// <param name="input">查询条件</param>
    [HttpGet("index/stat")]
    [LocalAuthorize("获取预购清单首页统计", "预购")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<IndexPreOrderStatDto>> GetIndexStatAsync([FromQuery] IndexPreOrderStatInput input)
    {
        return await _preOrderSvc.GetIndexStatAsync(input);
    }

    #endregion


    #region 预购分组

    /// <summary>
    /// 新增预购分组
    /// </summary>
    /// <param name="input">预购分组</param>
    [HttpPost("group")]
    [LocalAuthorize("新增", "预购分组")]
    public async Task<ServiceResult<PreOrderGroupWithStatDto>> CreateGroupAsync([FromBody] CreatePreOrderGroupInput input)
    {
        return await _preOrderGroupSvc.CreateAsync(input);
    }

    /// <summary>
    /// 获取预购分组详情
    /// </summary>
    /// <param name="id">预购分组id</param>
    [HttpGet("group")]
    [LocalAuthorize("详情", "预购分组")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<PreOrderGroupDto>> GetGroupAsync([FromQuery] long id)
    {
        return await _preOrderGroupSvc.GetAsync(id);
    }
    
    /// <summary>
    /// 获取预购分组详情(With 相关预购总金额)
    /// </summary>
    /// <param name="id">预购分组id</param>
    [HttpGet("group/amount")]
    [LocalAuthorize("详情带预购分组总金额", "预购分组")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<PreOrderGroupWithPreAmountDto>> GetGroupWithAmountAsync([FromQuery] long id)
    {
        return await _preOrderGroupSvc.GetGroupWithAmountAsync(id);
    }

    /// <summary>
    /// 预购分组转入账单
    /// </summary>
    /// <param name="input">预购分组</param>
    [HttpPost("group/to-bill")]
    [LocalAuthorize("分组转入账单", "预购分组")]
    public async Task<ServiceResult<PreOrderGroupDto>> GroupToBillAsync([FromBody] GroupToBillInput input)
    {
        return await _preOrderGroupSvc.GroupToBillAsync(input);
    }


    /// <summary> 
    /// 删除预购分组信息
    /// </summary>
    /// <param name="id">预购分组id</param>
    [HttpDelete("group")]
    [LocalAuthorize("删除", "预购分组")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult> DeleteGroupAsync([FromBody] long id)
    {
        await _preOrderGroupSvc.DeleteAsync(id);
        return ServiceResult.Successed("预购分组删除成功！");
    }

    /// <summary>
    /// 更新预购分组信息
    /// </summary>
    /// <param name="input">预购分组信息</param>
    [HttpPut("group")]
    [LocalAuthorize("更新", "预购分组")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<PreOrderGroupWithStatDto>> UpdateGroupAsync([FromBody] UpdatePreOrderGroupInput input)
    {
        return await _preOrderGroupSvc.UpdateAsync(input);
    }

    /// <summary>
    /// 获取指定月份分页预购分组
    /// </summary>
    /// <param name="input">分页条件</param>
    [HttpGet("group/month/pages")]
    [LocalAuthorize("获取指定月份分页预购分组", "预购分组")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<PagedDto<PreOrderGroupWithStatDto>>> GetByMonthPagesAsync([FromQuery] MonthPreOrderGroupPagingInput input)
    {
        return await _preOrderGroupSvc.GetByMonthPagesAsync(input);
    }


    /// <summary>
    /// 获取指定分组预购清单统计
    /// </summary>
    /// <param name="input">查询条件</param>
    [HttpGet("group/order/stat")]
    [LocalAuthorize("获取指定分组预购清单统计", "预购分组")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<GroupPreOrderStatDto>> GetPreOrderStatAsync([FromQuery] GroupPreOrderStatInput input)
    {
        return await _preOrderGroupSvc.GetPreOrderStatAsync(input);
    }

    #endregion
}
