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
    }
}
