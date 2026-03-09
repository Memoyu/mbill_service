namespace Memo.Bill.Domain.Constants.Security.Permissions;

public static partial class Permissions
{
    [Description("账单分类")]
    public static class Category
    {
        [Description("创建分类")]
        public const string Create = "create:category";

        [Description("更新分类")]
        public const string Update = "update:category";

        [Description("更新分类排序")]
        public const string UpdateSort = "update:Sort:category";

        [Description("删除分类")]
        public const string Delete = "delete:category";

        [Description("获取分类")]
        public const string Get = "get:category";

        [Description("获取分类分组")]
        public const string GetGroup = "get:group:category";
    }
}
