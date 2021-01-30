/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Shared.Configurations
*   文件名称 ：FileStorageOption.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-23 18:54:28
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/

namespace Memoyu.Mbill.Domain.Shared.Configurations
{
    public class FileStorageOption
    { 
        /// <summary>
      /// 上传文件总大小
      /// </summary>
        public long MaxFileSize { get; set; }
        /// <summary>
        /// 多文件上传时，支持的最大文件数量
        /// </summary>
        public int NumLimit { get; set; }
        /// <summary>
        /// 允许某些类型文件上传，文件格式以,隔开
        /// </summary>
        public string Include { get; set; }
        /// <summary>
        /// 禁止某些类型文件上传，文件格式以,隔开
        /// </summary>
        public string Exclude { get; set; }

        public string ServiceName { get; set; }
        public LocalFileOption LocalFile { get; set; }
    }

    public class LocalFileOption
    {
        public string PrefixPath { get; set; }
        public string Host { get; set; }
    }


}
