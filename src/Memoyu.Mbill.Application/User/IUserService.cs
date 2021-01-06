/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.User
*   文件名称 ：IUserService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 21:16:28
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Application.Contracts.Dtos.User;
using Memoyu.Mbill.ToolKits.Base.Page;
using System;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.User
{
    public interface IUserService
    {
        Task<PagedDto<UserDto>> GetListAsync(PagingDto pageDto);

        Task<UserDto> GetAsync(Guid id);

        Task CreateAsync(ModifyUserDto inputDto);

        Task UpdateAsync(Guid id, ModifyUserDto inputDto);

        Task DeleteAsync(Guid id);
    }
}
