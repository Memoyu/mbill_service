/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Controllers.Test
*   文件名称 ：TestController.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 17:44:21
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Application.Test;
using Memoyu.Mbill.Application.Contracts.Test;
using Memoyu.Mbill.ToolKits.Base;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Memoyu.Mbill.WebApi.Controllers.Test
{
    /// <summary>
    /// 演示控制器
    /// </summary>
    [Route("api/Test")]
    public class TestController : ApiControllerBase
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        /// <summary>
        /// 创建信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ServiceResult> CreateAsync([FromBody] ModifyTestDto dto)
        {
            var reponse = new ServiceResult();
            await _testService.CreateAsync(dto);
            reponse.IsSuccess("新增信息成功！");
            return reponse;
        }

    }
}
