namespace mbill_service.Core.Interface.IDependency
{
    /// <summary>
    /// 实现该接口将自动注册到Ioc容器，生命周期为每次调用，都会重新实例化对象；每次请求都创建一个新的对象
    /// </summary>
    public interface ITransientDependency
    {
    }
}
