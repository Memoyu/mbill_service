namespace Memo.Bill.Domain.Constants.Security.Permissions;

public static partial class Permissions
{
    [Description("账单账本")]
    public static class Ledger
    {
        [Description("创建账本")]
        public const string Create = "create:ledger";

        [Description("更新账本")]
        public const string Update = "update:ledger";

        [Description("更新账本颜色")]
        public const string UpdateColor = "update:color:ledger";

        [Description("更新账本排序")]
        public const string UpdateSort = "update:sort:ledger";

        [Description("加入账本")]
        public const string Join = "join:ledger";

        [Description("删除账本")]
        public const string Delete = "delete:ledger";

        [Description("获取账本列表")]
        public const string List = "list:ledger";

        [Description("获取账本详情")]
        public const string Get = "get:ledger";
    }
}
