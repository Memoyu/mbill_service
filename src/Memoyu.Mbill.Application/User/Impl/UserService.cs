/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.User.Impl
*   文件名称 ：UserService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 21:16:37
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Application.Base.Impl;
using Memoyu.Mbill.Application.Contracts.Dtos.User;
using Memoyu.Mbill.ToolKits.Base.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.User.Impl
{
    public class UserService : ApplicationService, IUserService
    {
        public Task CreateAsync(ModifyUserDto inputDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedDto<UserDto>> GetListAsync(PagingDto pageDto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid id, ModifyUserDto inputDto)
        {
            throw new NotImplementedException();
        }
    }
}
