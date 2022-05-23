namespace mbill.Core.AOP.Middleware;

/// <summary>
/// 异常处理中间件
/// </summary>
public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await ExceptionHandlerAsync(context, ex.Message);
        }
        finally
        {
            var statusCode = context.Response.StatusCode;
            if (statusCode != StatusCodes.Status200OK)
            {
                //获取状态码对应的值
                Enum.TryParse(typeof(HttpStatusCode), statusCode.ToString(), out object message);
                await ExceptionHandlerAsync(context, message.ToString());
            }
        }
    }

    private async Task ExceptionHandlerAsync(HttpContext context, string message)
    {
        context.Response.ContentType = "application/json;charset=utf-8";

        var result = new ServiceResult();
        result.IsFailed(message);
        await context.Response.WriteAsync(result.ToJson());
    }
}
