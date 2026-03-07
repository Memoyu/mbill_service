namespace Memo.Bill.Domain.Constants.Security.Permissions;

public static partial class Permissions
{
    [Description("聚合能力")]
    public static class Aggregation
    {
        [Description("获取地理位置信息")]
        public const string GetLocation = "get:location:aggregation";

        [Description("获取天气预报信息")]
        public const string GetWeatherInfo = "get:weatherinfo:aggregation";
    }
}
