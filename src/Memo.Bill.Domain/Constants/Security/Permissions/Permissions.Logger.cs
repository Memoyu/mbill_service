namespace Memo.Bill.Domain.Constants.Security.Permissions;

public static partial class Permissions
{
    [Description("用户日志")]
    public static class LoggerUser
    {
        [Description("获取用户日志")]
        public const string Get = "get:logger:user";

        [Description("获取用户日志分页")]
        public const string Page = "page:logger:user";
    }

    [Description("系统日志")]
    public static class LoggerSystem
    {
        [Description("获取系统日志")]
        public const string Get = "get:logger:system";

        [Description("获取系统日志分页")]
        public const string Page = "page:logger:system";
    }
}
