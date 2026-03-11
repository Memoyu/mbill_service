namespace Memo.Bill.Domain.Constants;
public static class CacheKeyConst
{
    /// <summary>
    /// 前缀
    /// </summary>
    private const string prefix = "memo.bill";

    /// <summary>
    /// 用户刷新token
    /// </summary>
    /// <param name="refreshToken">刷新token</param>
    /// <returns></returns>
    public static string UserRefreshToken(string refreshToken) => $"{prefix}:user:refresh-token:{refreshToken}";

    /// <summary>
    /// 天气信息缓存
    /// </summary>
    /// <param name="city">区域编码</param>
    /// <returns></returns>
    public static string WeatherInfo(string city) => $"{prefix}:aggregation:weatherinfo:{city}";
}
