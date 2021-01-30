/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.ToolKits.Utils
*   文件名称 ：MultipartRequestUtil.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-23 19:38:29
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.ToolKits.Utils
{
    public class MultipartRequestUtil
    {
        public static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
