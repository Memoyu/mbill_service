using System.Security.Cryptography;
using System.Text;

namespace Memo.Bill.Application.Common.Utils.QiniuUtil
{
    public class QiniuSignature
    {
        private readonly string accessKey;
        private readonly string secretKey;

        public QiniuSignature( string ak, string sk)
        {
            accessKey = ak;
            secretKey = sk;
        }

        public string SignWithData(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            string text = UrlSafeBase64Encode(bytes);
            return $"{accessKey}:{encodedSign(text)}:{text}";
        }

        private string encodedSign(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return UrlSafeBase64Encode(new HMACSHA1(Encoding.UTF8.GetBytes(secretKey)).ComputeHash(bytes));
        }

        private string UrlSafeBase64Encode(byte[] data)
        {
            return Convert.ToBase64String(data).Replace('+', '-').Replace('/', '_');
        }
    }
}
