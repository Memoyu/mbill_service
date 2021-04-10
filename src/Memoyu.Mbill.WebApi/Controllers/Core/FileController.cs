/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Controllers.Core
*   文件名称 ：FileController.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-10 11:57:14
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Memoyu.Mbill.Application.Contracts.Dtos.Core;
using Memoyu.Mbill.Application.Contracts.Exceptions;
using Memoyu.Mbill.Application.Core;
using Memoyu.Mbill.Domain.Shared.Configurations;
using Memoyu.Mbill.Domain.Shared.Const;
using Memoyu.Mbill.ToolKits.Base;
using Memoyu.Mbill.ToolKits.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Memoyu.Mbill.WebApi.Controllers.Core
{
    [Route("api/file")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v3)]
    [Authorize]
    public class FileController : ApiControllerBase
    {
        private readonly FileStorageOption _fileStorageOption;
        private readonly IFileService _fileService;
        public FileController(IOptionsSnapshot<FileStorageOption> optionsSnapshot, IComponentContext componentContext)
        {
            _fileStorageOption = optionsSnapshot.Value;
            _fileService = componentContext.ResolveNamed<IFileService>(_fileStorageOption.ServiceName);
        }

        /// <summary>
        /// 单文件上传，键为file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<ServiceResult<FileDto>> UploadFile(IFormFile file, string type, int key = 0)
        {
            if (!MultipartRequestUtil.IsMultipartContentType(Request.ContentType))
            {
                throw new KnownException($"The request couldn't be processed (Error 1).");
            }

            this.ValidFile(file);
            return ServiceResult<FileDto>.Successed(await _fileService.UploadAsync(file, type, key));
        }

        /// <summary>
        /// 多文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost("uploads")]
        public async Task<ServiceResult<List<FileDto>>> UploadFiles(string type)
        {
            IFormFileCollection files = Request.Form.Files;

            if (files.Count > _fileStorageOption.NumLimit)
            {
                throw new KnownException($"最大文件数量{_fileStorageOption.NumLimit}");
            }
            long len = 0;
            foreach (var file in files)
            {
                len += file.Length;
                this.ValidFile(file);
            }

            if (len > _fileStorageOption.MaxFileSize)
            {
                throw new KnownException($"文件总大小{len}，超过上传文件总大小{_fileStorageOption.MaxFileSize}");
            }

            int i = 0;
            List<FileDto> fileDtos = new List<FileDto>();
            foreach (var file in files)
            {
                FileDto fileDto = await _fileService.UploadAsync(file, type, i++);
                fileDtos.Add(fileDto);
            }
            return ServiceResult<List<FileDto>>.Successed(fileDtos);
        }

        /// <summary>
        /// 校验上传文件
        /// </summary>
        /// <param name="file"></param>
        private void ValidFile(IFormFile file)
        {
            string ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(ext))
            {
                throw new KnownException($"不支持的文件类型");
            }

            if (_fileStorageOption.Include.IsNotNullOrEmpty())
            {
                if (!_fileStorageOption.Include.Contains(ext))
                {
                    throw new KnownException($"不支持文件类型{ext}");
                }
                return;
            }

            if (_fileStorageOption.Exclude.IsNotNullOrEmpty() && _fileStorageOption.Exclude.Contains(ext))
            {
                throw new KnownException($"文件类型{ext}被禁止上传");
            }
        }

    }
}