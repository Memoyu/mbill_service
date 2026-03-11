using System.Security.Cryptography;
using System.Text;

namespace Memo.Bill.Application.Common.Utils;
public static class EncryptUtil
{
    /// <summary>
    /// 通过创建哈希字符串适用于任何 MD5 哈希函数 （在任何平台） 上创建 32 个字符的十六进制格式哈希字符串
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string Encrypt(string source)
    {
        using MD5 md5Hash = MD5.Create();
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
        StringBuilder sBuilder = new StringBuilder();
        foreach (byte t in data)
        {
            sBuilder.Append(t.ToString("x2"));
        }

        string hash = sBuilder.ToString();
        return hash.ToUpper();
    }
}
