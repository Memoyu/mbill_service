namespace mbill.Controllers.PreOrder;

/// <summary>
/// 预购清单管理
/// </summary>
[Authorize]
[Route("api/preorder")]
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
    public async Task<ServiceResult<PreOrderDto>> CreateAsync([FromBody] CreatePreOrderInput input)
    {
        var result = await _preOrderSvc.CreateAsync(input);
        return ServiceResult<PreOrderDto>.Successed(result, "预购创建成功！");
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
        return ServiceResult<PreOrderDto>.Successed(await _preOrderSvc.GetAsync(id));
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
    public async Task<ServiceResult<PreOrderDto>> UpdateAsync([FromBody] UpdatePreOrderInput input)
    {
        return ServiceResult<PreOrderDto>.Successed(await _preOrderSvc.UpdateAsync(input.Id, input));
    }

    #endregion


    #region 预购分组

    /// <summary>
    /// 新增预购分组
    /// </summary>
    /// <param name="input">预购分组</param>
    [HttpPost("group")]
    [LocalAuthorize("新增", "预购分组")]
    public async Task<ServiceResult<PreOrderDto>> CreateGroupAsync([FromBody] CreatePreOrderInput input)
    {
        var result = await _preOrderGroupSvc.CreateAsync(input);
        return ServiceResult<PreOrderDto>.Successed(result, "预购分组创建成功！");
    }

    /// <summary>
    /// 获取预购分组详情
    /// </summary>
    /// <param name="id">预购分组id</param>
    [HttpGet("group")]
    [LocalAuthorize("详情", "预购分组")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult<PreOrderDto>> GetGroupAsync([FromQuery] long id)
    {
        return ServiceResult<PreOrderDto>.Successed(await _preOrderGroupSvc.GetAsync(id));
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
    public async Task<ServiceResult<PreOrderDto>> UpdateGroupAsync([FromBody] UpdatePreOrderInput input)
    {
        return ServiceResult<PreOrderDto>.Successed(await _preOrderGroupSvc.UpdateAsync(input.Id, input));
    }

    #endregion
}
