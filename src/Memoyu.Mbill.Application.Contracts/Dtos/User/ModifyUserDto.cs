/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Dtos.User
*   文件名称 ：ModifyUserDto.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 23:51:21
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System.ComponentModel.DataAnnotations;

namespace Memoyu.Mbill.Application.Contracts.Dtos.User
{
    public class ModifyUserDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "必须传入用户名")]
        public string Username { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Required(ErrorMessage = "必须传入昵称")]
        public string Nickname { get; set; }

        /// <summary>
        /// 性别，0：未知，1：男，2：女
        /// </summary>
        [Required(ErrorMessage = "必须传入昵称")]
        public int Gender { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 街道
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string AvatarUrl { get; set; }

    }
}
