namespace Mbill.Core.Domains.Common.Enums.Base;

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
