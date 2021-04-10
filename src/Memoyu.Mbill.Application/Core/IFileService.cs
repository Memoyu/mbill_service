/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Core.Files
*   文件名称 ：IFileService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-23 19:00:23
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Application.Contracts.Dtos.Core;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Core
{
    public interface IFileService
    {
        /// <summary>
        /// 单文件上传，键为file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<FileDto> UploadAsync(IFormFile file, string type, int key = 0);
    }
}
