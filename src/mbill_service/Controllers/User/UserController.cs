using AutoMapper;
using mbill_service.Core.AOP.Attributes;
using mbill_service.Core.Domains.Common;
using mbill_service.Core.Domains.Common.Consts;
using mbill_service.Core.Domains.Entities.Core;
using mbill_service.Core.Domains.Entities.User;
using mbill_service.Service.Core.User;
using mbill_service.Service.Core.User.Input;
using mbill_service.Service.Core.User.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace mbill_service.Controllers.Core
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Route("api/user")]
    public class UserController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        /// <summary>
        /// 超级管理员新增用户
        /// </summary>
        /// <param name="userInput"></param>
        [Logger("超级管理员新建了一个用户")]
        [HttpPost("register")]
        [Authorize(Roles = RoleEntity.Administrator)]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
        public async Task<ServiceResult> CreateAsync([FromBody] ModifyUserDto userInput)
        {
            await _userService.CreateAsync(_mapper.Map<UserEntity>(userInput), userInput.RoleIds, userInput.Password);
            return ServiceResult.Successed("用户创建成功");
        }

        /// <summary>
        /// 获取用户信息，By Id
        /// </summary>
        [HttpGet("get")]
        [Authorize]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v3)]
        public async Task<ServiceResult<UserDto>> GetByIdAsync([FromQuery] long? id)
        {
            return ServiceResult<UserDto>.Successed(await _userService.GetAsync(id));
        }

        /// <summary>
        /// 获取用户信息分页
        /// </summary>
        [HttpGet("get/pages")]
        [Authorize]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v3)]
        public async Task<ServiceResult<PagedDto<UserDto>>> GetPagesAsync([FromQuery] UserPagingDto pagingDto)
        {
            return ServiceResult<PagedDto<UserDto>>.Successed(await _userService.GetPagesAsync(pagingDto));
        }
    }
}
