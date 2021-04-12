using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mbill_service.Service.Core.User.Input
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


        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "新密码不可为空")]
        [Compare("ConfirmPassword", ErrorMessage = "两次输入的密码不一致，请输 入相同的密码")]
        [RegularExpression("^[A-Za-z0-9_*&$#@]{6,22}$", ErrorMessage = "密码长度必须在6~22位之间，包含字符、数字和 _")]
        public string Password { get; set; }
         
        /// <summary>
        /// 确认密码
        /// </summary>
        [Required(ErrorMessage = "请确认密码")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public List<long> RoleIds { get; set; }

    }
}
