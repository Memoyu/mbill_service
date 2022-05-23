namespace mbill.Core.Exceptions;

[Serializable]
public class KnownException : ApplicationException
{
    /// <summary>
    /// 状态码
    /// </summary>
    private readonly int _statusCode;
    /// <summary>
    /// 错误码，当为1时，代表正常
    /// </summary>
    private readonly ServiceResultCode _errorCode;
    /// <summary>
    /// 
    /// </summary>
    public KnownException() : base("服务器繁忙，请稍后再试!")
    {
        _errorCode = ServiceResultCode.Failed;
        _statusCode = 400;
    }

    public KnownException(string message = "服务器繁忙，请稍后再试!", ServiceResultCode errorCode = ServiceResultCode.Failed, int statusCode = 400) : base(message)
    {
        this._errorCode = errorCode;
        _statusCode = statusCode;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetStatusCode()
    {
        return _statusCode;
    }

    public ServiceResultCode GetErrorCode()
    {
        return _errorCode;
    }
}
