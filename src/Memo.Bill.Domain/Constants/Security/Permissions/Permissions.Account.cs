namespace Memo.Bill.Domain.Constants.Security.Permissions;

public static partial class Permissions
{
    [Description("账单账户")]
    public static class Account
    {
        [Description("创建账户")]
        public const string Create = "create:account";

        [Description("更新账户")]
        public const string Update = "update:account";

        [Description("更新账户排序")]
        public const string Sort = "sort:account";

        [Description("删除账户")]
        public const string Delete = "delete:account";

        [Description("获取账户")]
        public const string Get = "get:account";

        [Description("获取账户分组列表")]
        public const string ListGroup = "list:group:account";
    }
}
