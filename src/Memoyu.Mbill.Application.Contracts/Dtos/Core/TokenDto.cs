/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Dtos.Core
*   文件名称 ：TokenDto.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-10 15:15:26
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Contracts.Dtos.Core
{
    public class TokenDto
    {
        public TokenDto(string accessToken, string refreshToken)
        {
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            RefreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
        }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public override string ToString()
        {
            return $"TokenDto - 授权Token:{AccessToken},刷新Token:{RefreshToken}";
        }
    }
}
