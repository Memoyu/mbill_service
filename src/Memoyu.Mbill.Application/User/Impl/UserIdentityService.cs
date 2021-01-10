/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.User.Impl
*   文件名称 ：UserIdentityService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-10 15:28:23
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Entities.System;
using Memoyu.Mbill.ToolKits.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.User.Impl
{
    public class UserIdentityService : IUserIdentityService
    {
        private readonly IAuditBaseRepository<UserIdentityEntity> _userIdentityRepository;
        public UserIdentityService(IAuditBaseRepository<UserIdentityEntity> userIdentityRepository)
        {
            _userIdentityRepository = userIdentityRepository;
        }
        public Task ChangePasswordAsync(long userId, string newpassword)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> VerifyUserPasswordAsync(long userId, string password)
        {
            UserIdentityEntity userIdentity = await this.GetFirstByUserIdAsync(userId);

            return userIdentity != null && EncryptUtil.Verify(userIdentity.Credential, password);
        }

        /// <summary>
        /// 通过Id获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserIdentityEntity> GetFirstByUserIdAsync(long userId)
        {
            return await _userIdentityRepository
                .Where(r => r.CreateUserId == userId && r.IdentityType == UserIdentityEntity.Password)
                .ToOneAsync();
        }
    }
}
