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
using Memoyu.Mbill.Domain.Entities.User;
using Memoyu.Mbill.ToolKits.Base.Page;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.User
{
    public interface IUserService
    {
        /// <summary>
        /// 注册-新增一个用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="roleIds">分组Id集合</param>
        /// <param name="password">密码</param>
        Task CreateAsync(UserEntity user, List<long> roleIds, string password);

        /// <summary>
        /// 获取用户信息，id为空时，通过Token获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserDto> GetAsync(long? id);

        /// <summary>
        /// 获取用户分页信息
        /// </summary>
        /// 
        /// <returns></returns>
        Task<PagedDto<UserDto>> GetPagesAsync(UserPagingDto pagingDto);

        Task UpdateAsync(long id, UserEntity entity);

        Task DeleteAsync(long id);
    }
}
