namespace Memo.Bill.Domain.Constants.Security.Permissions;

public static partial class Permissions
{
    [Description("账单分类")]
    public static class Category
    {
        [Description("创建分类")]
        public const string Create = "create:account";

        [Description("更新分类")]
        public const string Update = "update:account";

        [Description("删除分类")]
        public const string Delete = "delete:account";
    }
}
