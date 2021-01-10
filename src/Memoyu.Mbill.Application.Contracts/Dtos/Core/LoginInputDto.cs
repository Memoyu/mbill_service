/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Dtos.Core
*   文件名称 ：LoginInputDto.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-10 15:17:04
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System.ComponentModel.DataAnnotations;

namespace Memoyu.Mbill.Application.Contracts.Dtos.Core
{
    public class LoginInputDto
    {
        /// <summary>
        /// 登录名
        /// </summary>
        /// <example>
        /// admin
        /// </example>
        [Required(ErrorMessage = "登录名为必填项")]
        public string Username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        /// <example>
        /// 123456
        /// </example>
        [Required(ErrorMessage = "密码为必填项")]
        public string Password { get; set; }
    }
}
