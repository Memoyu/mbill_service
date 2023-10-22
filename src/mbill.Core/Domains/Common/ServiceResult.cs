namespace Mbill.Core.Domains.Common;

/// <summary>
/// 服务层响应实体
/// </summary>
public class ServiceResult
{
    public ServiceResult()
    {

    }

    public ServiceResult(ServiceResultCode code, string message)
    {
        Code = code;
        Message = message;
    }

    /// <summary>
    /// 响应码
    /// </summary>
    public ServiceResultCode Code { get; set; }

    /// <summary>
    /// 响应信息
    /// </summary>
    public object Message { get; set; }

    /// <summary>
    /// 成功与否
    /// </summary>
    public bool Success => Code == ServiceResultCode.Succeed;

    /// <summary>
    /// 时间戳（毫秒）
    /// </summary>
    public long Timestamp { get; } = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;

    /// <summary>
    /// 响应成功
    /// </summary>
    /// <param name="message"></param>
    public void IsSuccess(string message = "")
    {
        Message = message;
        Code = ServiceResultCode.Succeed;
    }

    /// <summary>
    /// 响应失败
    /// </summary>
    /// <param name="message"></param>
    public void IsFailed(string message = "")
    {
        Message = message;
        Code = ServiceResultCode.Failed;
    }

    /// <summary>
    /// 响应失败-异常
    /// </summary>
    /// <param name="exception"></param>
    public void IsFailed(Exception exception)
    {
        Message = exception.InnerException?.StackTrace;
        Code = ServiceResultCode.Failed;
    }

    /// <summary>
    /// 响应成功(静态返回实例)
    /// </summary>
    /// <param name="message"></param>
    public static ServiceResult Successed(string message = "")
    {
        var result = new ServiceResult();
        result.Message = message;
        result.Code = ServiceResultCode.Succeed;
        return result;
    }

    /// <summary>
    /// 响应失败(静态返回实例)
    /// </summary>
    /// <param name="message"></param>
    public static ServiceResult Failed(string message = "")
    {
        var result = new ServiceResult();
        result.Message = message;
        result.Code = ServiceResultCode.Failed;
        return result;
    }

    /// <summary>
    /// 响应失败(静态返回实例)
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    public static ServiceResult Failed(ServiceResultCode code, string message = "")
    {
        var result = new ServiceResult();
        result.Message = message;
        result.Code = code;
        return result;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
        });
    }
}

/// <summary>
/// 服务层响应实体（泛型）
/// </summary>
/// <typeparam name="T"></typeparam>
public class ServiceResult<T> : ServiceResult
{
    /// <summary>
    /// 响应结果
    /// </summary>
    public T Result { get; set; }
    /// <summary>
    /// 响应成功-带结果
    /// </summary>
    /// <param name="result"></param>
    /// <param name="message"></param>
    public void IsSuccess(T result = default, string message = "")
    {
        Message = message;
        Result = result;
        Code = ServiceResultCode.Succeed;
    }

    /// <summary>
    /// 响应成功(静态返回实例)
    /// </summary>
    /// <param name="message"></param>
    public static ServiceResult<T> Successed(T result = default, string message = "")
    {
        var res = new ServiceResult<T>();
        res.Message = message;
        res.Result = result;
        res.Code = ServiceResultCode.Succeed;
        return res;
    }

    /// <summary>
    /// 响应失败(静态返回实例)
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    public new static ServiceResult<T> Failed(ServiceResultCode code, string message = "")
    {
        var result = new ServiceResult<T>();
        result.Message = message;
        result.Code = code;
        return result;
    }

    /// <summary>
    /// 响应失败(静态返回实例)
    /// </summary>
    /// <param name="message"></param>
    public new static ServiceResult<T> Failed(string message = "")
    {
        var result = new ServiceResult<T>();
        result.Message = message;
        result.Code = ServiceResultCode.Failed;
        return result;
    }
}
