/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Controllers.User
*   文件名称 ：UserController.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 17:44:21
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Application.Contracts.Dtos.User;
using Memoyu.Mbill.Application.User;
using Memoyu.Mbill.ToolKits.Base;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Memoyu.Mbill.WebApi.Controllers.User
{
    /// <summary>
    /// 用户信息控制器
    /// </summary>
    [Route("api/user")]
    public class UserController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ServiceResult> CreateAsync([FromBody] ModifyUserDto dto)
        {
            var reponse = new ServiceResult();
            await _userService.CreateAsync(dto);
            reponse.IsSuccess("新增用户成功！");
            return reponse;
        }

    }
}
