using mbill.ToolKits.Qiniu;
using System.Text;

namespace mbill.Service.Core.Files;

public class QiniuFileSvc : IQiniuFileSvc
{
    private readonly IFileRepo _fileRepo;

    private readonly IQiniuClient _qiniuClient;

    public QiniuFileSvc(IWebHostEnvironment hostingEnv, IFileRepo fileRepo, IQiniuClient qiniuClient)
    {
        _fileRepo = fileRepo;
        _qiniuClient = qiniuClient;
    }

    public Task<ServiceResult<FileDto>> CheckMD5(string md5)
    {
        throw new NotImplementedException();
    }

    public ServiceResult<string> GetUploadToken()
    {
       /* //获得当前文件夹下所有文件夹
        string path = "C://Users//mmy60//Pictures//mbill";
        var root = new DirectoryInfo(path);

        //获得当前文件夹下的文件
        var dics = root.GetDirectories();
        for (int i = 0; i < dics.Length; i++)
        {
            var name = dics[i].Name;
            if (name == "asset")
            {
                var files = dics[i].GetFiles();
                foreach (var f in files)
                {
                    InsertFile(f, "asset_icons");
                }
            }

            if (name == "category")
            {
                var files = dics[i].GetFiles();
                foreach (var f in files)
                {
                    InsertFile(f, "category_icons");
                }
            }


            void InsertFile(FileInfo file, string dic)
            {
                var fname = file.Name;
                var ext = file.Extension;
                string md5 = HashUtil.GetHash<MD5>(file.OpenRead());
                var path = $"{dic}/{fname}";
                var s = file.Length;
                _fileRepo.InsertAsync(new FileEntity
                {
                    Extension = ext,
                    Md5 = md5,
                    Name = fname,
                    Path = path,
                    Type = 1,
                    Size = s
                }).GetAwaiter().GetResult();
            }
        }*/


        return ServiceResult<string>.Successed(_qiniuClient.CreateUploadToken());
    }

    public Task<ServiceResult<FileDto>> UploadAsync(IFormFile file, string type, int key = 0)
    {
        throw new NotImplementedException();
    }
}
