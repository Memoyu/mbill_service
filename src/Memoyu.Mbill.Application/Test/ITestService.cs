/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Application.Test
*   文件名称 ：TestService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 8:34:12
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Application.Contracts.Test;
using Memoyu.Mbill.ToolKits.Base.Page;
using System;
using System.Threading.Tasks;

namespace Memoyu.Application.Test
{
    public interface ITestService
    {
        Task<PagedDto<TestDto>> GetListAsync(PagingDto pageDto);

        Task<TestDto> GetAsync(Guid id);

        Task CreateAsync(ModifyTestDto inputDto);

        Task UpdateAsync(Guid id, ModifyTestDto inputDto);

        Task DeleteAsync(Guid id);
    }
}
