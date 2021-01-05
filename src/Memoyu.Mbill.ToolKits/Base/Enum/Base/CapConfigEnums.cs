/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.ToolKits.Base.Enum.Base
*   文件名称 ：CapConfigEnums.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 14:05:37
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/

namespace Memoyu.Mbill.ToolKits.Base.Enum.Base
{
    public enum CapStorageTypeEnums
    {
        /// <summary>
        /// 引用包 DotNetCore.CAP.InMemoryStorage
        /// </summary>
        InMemoryStorage = 0,
        /// <summary>
        /// 引用包  DotNetCore.CAP.MySql
        /// </summary>
        Mysql = 1,
        /// <summary>
        /// 引用包  DotNetCore.CAP.SqlServer
        /// </summary>
        SqlServer = 2
    }

    public enum CapMessageQueueTypeEnums
    {
        /// <summary>
        /// 内存队列
        /// </summary>
        InMemoryQueue = 0,
        /// <summary>
        /// RabbitMQ
        /// </summary>
        RabbitMQ = 1,
    }
}
