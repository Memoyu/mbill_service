using System.Threading.Tasks;

namespace mbill_service.Service.Core.User
{
    public interface IUserIdentityService
    {
        /// <summary>
        /// 验证用户密码是否正确
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> VerifyUserPasswordAsync(long userId, string password);

        /// <summary>
        /// 根据用户ID，修改用户的密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newpassword"></param>
        Task ChangePasswordAsync(long userId, string newpassword);
    }
}
