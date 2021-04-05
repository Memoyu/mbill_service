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
using Memoyu.Mbill.Application.Contracts.Attributes;
using Memoyu.Mbill.Application.Contracts.Dtos.User;
using Memoyu.Mbill.Application.Contracts.Exceptions;
using Memoyu.Mbill.Domain.Entities.Core;
using Memoyu.Mbill.Domain.Entities.User;
using Memoyu.Mbill.Domain.IRepositories.Core;
using Memoyu.Mbill.Domain.IRepositories.User;
using Memoyu.Mbill.ToolKits.Base.Enum.Base;
using Memoyu.Mbill.ToolKits.Base.Page;
using Memoyu.Mbill.ToolKits.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.User.Impl
{
    public class UserService : ApplicationService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileRepository _fileRepository;

        public UserService(IUserRepository userRepository , IFileRepository fileRepository)
        {
            _userRepository = userRepository;
            _fileRepository = fileRepository;
        }

        [Transactional]
        public async Task CreateAsync(UserEntity user, List<long> roleIds, string password)
        {
            if (!string.IsNullOrEmpty(user.Username))
            {
                bool isRepeatName = await _userRepository.Select.AnyAsync(r => r.Username == user.Username);
                if (isRepeatName)//用户名重复
                {
                    throw new KnownException("用户名重复，请重新输入", ServiceResultCode.RepeatField);
                }
            }

            if (!string.IsNullOrEmpty(user.Email.Trim()))
            {
                var isRepeatEmail = await _userRepository.Select.AnyAsync(r => r.Email == user.Email.Trim());
                if (isRepeatEmail)//邮箱重复
                {
                    throw new KnownException("注册邮箱重复，请重新输入", ServiceResultCode.RepeatField);
                }
            }

            user.UserRoles = new List<UserRoleEntity>();
            roleIds?.ForEach(roleId =>//遍历构建赋值角色
            {
                user.UserRoles.Add(new UserRoleEntity()
                {
                    RoleId = roleId
                });
            });

            user.UserIdentitys = new List<UserIdentityEntity>()//构建赋值用户身份认证登录信息
            {
                new UserIdentityEntity(UserIdentityEntity.Password,user.Username,EncryptUtil.Encrypt(password),DateTime.Now)
            };
            await _userRepository.InsertAsync(user);
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto> GetAsync()
        {
            var user = await _userRepository
                .Select
                .IncludeMany(u=>u.Roles)
                .Where(r => r.Id == CurrentUser.Id).FirstAsync();
            user.AvatarUrl = _fileRepository.GetFileUrl(user.AvatarUrl);
            //user.Roles.Select 
            return Mapper.Map<UserDto>(user);
        }

        public Task<UserDto> GetAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedDto<UserDto>> GetListAsync(PagingDto pageDto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(long id, UserEntity inputDto)
        {
            throw new NotImplementedException();
        }
    }
}
