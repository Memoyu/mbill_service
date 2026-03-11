namespace Memo.Bill.Application.Common.Attributes;

/// <summary>
/// 属性形式定义生命周期
/// 默认Scoped
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AppServiceAttribute : Attribute
{
    public ServiceLifeType ServiceLifeType { get; set; } = ServiceLifeType.Scoped;

}
public enum ServiceLifeType
{
    Transient, Scoped, Singleton
}
