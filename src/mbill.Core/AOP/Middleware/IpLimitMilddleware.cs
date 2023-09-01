namespace mbill.Core.AOP.Middleware;

public class IpLimitMiddleware : IpRateLimitMiddleware
{
    public IpLimitMiddleware(RequestDelegate next, IProcessingStrategy processingStrategy, IOptions<IpRateLimitOptions> options, IIpPolicyStore policyStore, IRateLimitConfiguration config, ILogger<IpRateLimitMiddleware> logger)
         : base(next, processingStrategy, options, policyStore, config, logger)
    {
    }

    public override Task ReturnQuotaExceededResponse(HttpContext httpContext, RateLimitRule rule, string retryAfter)
    {
        httpContext.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        return base.ReturnQuotaExceededResponse(httpContext, rule, retryAfter);
    }
}
