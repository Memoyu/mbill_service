namespace Memo.Bill.Domain.Constants.Security.Permissions;

public static partial class Permissions
{
    [Description("账单")]
    public static class Bill
    {
        [Description("创建账单")]
        public const string Create = "create:bill";

        [Description("更新账单")]
        public const string Update = "update:bill";

        [Description("删除账单")]
        public const string Delete = "delete:bill";

        [Description("获取账单")]
        public const string Get = "get:bill";

        [Description("获取指定日期账单")]
        public const string GetDate = "get:date:bill";

        [Description("账单搜索")]
        public const string Search = "search:bill";

        [Description("获取搜索账单记录")]
        public const string SearchRecord = "search:record:bill";
    }
}
