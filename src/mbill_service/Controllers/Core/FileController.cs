using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using mbill_service.Core.Common.Configs;
using mbill_service.Core.Domains.Common;
using mbill_service.Core.Domains.Common.Consts;
using mbill_service.Core.Exceptions;
using mbill_service.Service.Core.Files;
using mbill_service.Service.Core.Files.Output;
using mbill_service.ToolKits.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace mbill_service.Controllers.Core
{
    [Route("api/file")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v3)]
    [Authorize]
    public class FileController : ApiControllerBase
    {
        private readonly IFileService _fileService;
        public FileController(IComponentContext componentContext)
        {
            _fileService = componentContext.ResolveNamed<IFileService>(Appsettings.FileStorage.ServiceName);
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

            if (files.Count > Appsettings.FileStorage.NumLimit)
            {
                throw new KnownException($"最大文件数量{Appsettings.FileStorage.NumLimit}");
            }
            long len = 0;
            foreach (var file in files)
            {
                len += file.Length;
                this.ValidFile(file);
            }

            if (len > Appsettings.FileStorage.MaxFileSize)
            {
                throw new KnownException($"文件总大小{len}，超过上传文件总大小{Appsettings.FileStorage.MaxFileSize}");
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

            if (Appsettings.FileStorage.Include.IsNotNullOrEmpty())
            {
                if (!Appsettings.FileStorage.Include.Contains(ext))
                {
                    throw new KnownException($"不支持文件类型{ext}");
                }
                return;
            }

            if (Appsettings.FileStorage.Exclude.IsNotNullOrEmpty() && Appsettings.FileStorage.Exclude.Contains(ext))
            {
                throw new KnownException($"文件类型{ext}被禁止上传");
            }
        }

    }
}