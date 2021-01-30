/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.ToolKits.Utils
*   文件名称 ：HashUtil.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-23 19:28:24
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Memoyu.Mbill.ToolKits.Utils
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
