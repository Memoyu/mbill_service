using Microsoft.Extensions.Logging;

namespace Memo.Bill.Application.Common.Behaviours;

public class GlobalExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : notnull
    where TResponse : Result
{
    private readonly ILogger<TRequest> _logger;

    public GlobalExceptionBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var failureMsg = string.Empty;
        try
        {
            return await next();
        }
        catch (ValidationException vex)
        {
            var erros = string.Join("; ", vex.Errors.Select(e => e.ErrorMessage).ToList());
            failureMsg = $"参数错误：{erros}"; ;
        }
        catch (ApplicationException ex)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogError(ex, "Request: 请求中发生业务错误 请求：{Name}；参数：{@Request}", requestName, request);
            failureMsg = $"{ex.Message}";
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogError(ex, "Request: 请求中发生未知异常 请求：{Name}；参数：{@Request}", requestName, request);
            failureMsg = $"请求异常：{ex.Message}";
        }

        return (dynamic)Result.Failure(failureMsg);
    }
}
