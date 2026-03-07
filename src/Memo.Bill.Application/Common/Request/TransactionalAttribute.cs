using FreeSql;
using System.Data;

namespace Memo.Bill.Application.Common.Request;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class TransactionalAttribute : Attribute
{
    /// <summary>
    /// 事务传播方式
    /// </summary>
    public Propagation Propagation { get; set; } = Propagation.Required;

    /// <summary>
    /// 事务隔离级别
    /// </summary>
    public IsolationLevel? IsolationLevel { get; set; }
}
