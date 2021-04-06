/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Controllers.User
*   文件名称 ：UserController.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 23:51:28
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using AutoMapper;
using Memoyu.Mbill.Application.Contracts.Attributes;
using Memoyu.Mbill.Application.Contracts.Dtos.User;
using Memoyu.Mbill.Application.User;
using Memoyu.Mbill.Domain.Entities.Core;
using Memoyu.Mbill.Domain.Entities.User;
using Memoyu.Mbill.Domain.Shared.Const;
using Memoyu.Mbill.ToolKits.Base;
using Memoyu.Mbill.ToolKits.Base.Page;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Memoyu.Mbill.WebApi.Controllers.User
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
