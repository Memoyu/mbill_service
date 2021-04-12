using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace mbill_service.ToolKits.Utils
{
    public class HashUtil
    {
        /// <summary>
        /// 继承HashAlgorithm
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetHash<T>(Stream stream) where T : HashAlgorithm
        {
            StringBuilder sb = new StringBuilder();

            MethodInfo create = typeof(T).GetMethod("Create", new Type[] { });
            using (T crypt = (T)create.Invoke(null, null))
            {
                if (crypt != null)
                {
                    byte[] hashBytes = crypt.ComputeHash(stream);
                    foreach (byte bt in hashBytes)
                    {
                        sb.Append(bt.ToString("x2"));
                    }
                }
            }
            return sb.ToString();
        }
    }
}
