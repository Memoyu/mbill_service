using mbill_service.Core.Common.Configs;
using mbill_service.Core.Domains.Entities.Core;
using mbill_service.Core.Exceptions;
using mbill_service.Core.Interface.IRepositories.Core;
using mbill_service.Service.Core.Files.Output;
using mbill_service.ToolKits.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace mbill_service.Service.Core.Files
{
    public class LocalFileSvc : IFileSvc
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly IFileRepo _fileRepo;

        public LocalFileSvc(IWebHostEnvironment hostingEnv, IFileRepo fileRepo)
        {
            _hostingEnv = hostingEnv;
            _fileRepo = fileRepo;
        }

        /// <summary>
        /// 本地文件上传，秒传（根据lin_file表中的md5,与当前文件的路径是否在本地），如果不在，重新上传，覆盖文件表记录
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<FileDto> UploadAsync(IFormFile file, string type, int key = 0)
        {
            string md5 = HashUtil.GetHash<MD5>(file.OpenReadStream());
            FileEntity fileInfo = await _fileRepo.Where(r => r.Md5 == md5 && r.Type == 1).OrderByDescending(r => r.CreateTime).FirstAsync();

            if (fileInfo != null && File.Exists(Path.Combine(_hostingEnv.WebRootPath, fileInfo.Path)))//如果文件存在，则直接返回文件信息
            {
                return new FileDto
                {
                    Id = fileInfo.Id,
                    Key = "file_" + key,
                    Path = fileInfo.Path,
                    Url = Appsettings.FileStorage.LocalFileHost + fileInfo.Path
                };
            }

            long id;
            var (path, len) = await this.LocalUploadAsync(file, type);

            if (fileInfo == null)
            {
                FileEntity saveLinFile = new FileEntity()
                {
                    Extension = Path.GetExtension(file.FileName),
                    Md5 = md5,
                    Name = file.FileName,
                    Path = path,
                    Type = 1,
                    Size = len
                };
                id = (await _fileRepo.InsertAsync(saveLinFile)).Id;//插入文件数据
            }
            else
            {
                await _fileRepo.UpdateDiy.Where(a=>a.Id == fileInfo.Id).Set(a => a.Path, path).ExecuteAffrowsAsync();//仅更新路径字段
                id = fileInfo.Id;
            }

            return new FileDto
            {
                Id = id,
                Key = "file_" + key,
                Path = path,
                Url = Appsettings.FileStorage.LocalFileHost + path
            };

        }

        /// <summary>
        /// 本地文件上传，生成目录格式 {STORE_DIR}/{year}/{month}/{day}/{guid}.文件后缀
        /// images/2022-01-12/fba73a0c-f2f7-499a-8ed8-5b10554d43b0.jpg
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private async Task<Tuple<string, long>> LocalUploadAsync(IFormFile file, string type)
        {
            if (file.Length == 0)
            {
                throw new KnownException("文件为空");
            }

            string saveFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            //得到 images/2020-05-12
            string path = Path.Combine(Appsettings.FileStorage.LocalFilePrefixPath, DateTime.Now.ToString("yyyy-MM-dd"));
            if (type.IsNotNullOrEmpty() && FileEntity.SourceFileDic.ContainsKey(type))
            {
                path = FileEntity.SourceFileDic[type];
            }

            //得到wwwroot/images/2020-05-12 或 type.value/
            string createFolder = Path.Combine(_hostingEnv.WebRootPath, path);
            //创建这种不存在的目录
            if (!Directory.Exists(createFolder))
            {
                Directory.CreateDirectory(createFolder);
            }

            long len = 0;
            await using (FileStream fs = File.Create(Path.Combine(createFolder, saveFileName)))
            {
                await file.CopyToAsync(fs);
                len = fs.Length;
                await fs.FlushAsync();
            }

            //windows下Path.Combine,得到的\\，不符号路径的要求。替换一下
            //得到 路径与文件大小    images/2020-05-12/fba73a0c-f2f7-499a-8ed8-5b10554d43b0.jpg
            return Tuple.Create(Path.Combine(path, saveFileName).Replace("\\", "/"), len);
        }
    }
}
