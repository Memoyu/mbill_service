using AutoMapper;
using Memoyu.Mbill.Application.Contracts.Attributes;
using Memoyu.Mbill.Application.Contracts.Dtos.User;
using Memoyu.Mbill.Application.User;
using Memoyu.Mbill.Domain.Entities.System;
using Memoyu.Mbill.Domain.Entities.User;
using Memoyu.Mbill.Domain.Shared.Const;
using Memoyu.Mbill.ToolKits.Base;
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
    }
}
