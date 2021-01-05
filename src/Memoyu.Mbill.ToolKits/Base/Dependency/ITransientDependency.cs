/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.ToolKits.Base.Dependency
*   文件名称 ：ITransientDependency.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2020-12-28 0:34:42
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
namespace Memoyu.Mbill.ToolKits.Base.Dependency
{
    /// <summary>
    /// 实现该接口将自动注册到Ioc容器，生命周期为每次调用，都会重新实例化对象；每次请求都创建一个新的对象
    /// </summary>
    public interface ITransientDependency
    {
    }
}
