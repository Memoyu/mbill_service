namespace mbill_service.Controllers.Core;

/// <summary>
/// 用户管理
/// </summary>
[Route("api/admin/user")]
public class UserController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserSvc _userService;

    public UserController(IMapper mapper, IUserSvc userService)
    {
        _mapper = mapper;
        _userService = userService;
    }

    /// <summary>
    /// 超级管理员新增用户
    /// </summary>
    /// <param name="userInput">用户更改</param>
    [Logger("超级管理员新建了一个用户")]
    [HttpPost]
    [LocalAuthorize("新增用户", "管理员")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
    public async Task<ServiceResult> CreateAsync([FromBody] ModifyUserDto userInput)
    {
        await _userService.CreateAsync(_mapper.Map<UserEntity>(userInput), userInput.RoleIds, userInput.Password);
        return ServiceResult.Successed("用户创建成功");
    }

    /// <summary>
    /// 获取用户信息分页
    /// </summary>
    /// <param name="pagingDto">分页</param>
    [HttpGet("pages")]
    [Authorize]
    [LocalAuthorize("获取用户分页数据", "管理员")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v3)]
    public async Task<ServiceResult<PagedDto<UserDto>>> GetPagesAsync([FromQuery] UserPagingDto pagingDto)
    {
        return ServiceResult<PagedDto<UserDto>>.Successed(await _userService.GetPagesAsync(pagingDto));
    }
}
