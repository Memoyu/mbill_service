namespace Memo.Bill.Domain.Constants.Security.Permissions;

public static partial class Permissions
{
    [Description("账单标签")]
    public static class Tag
    {
        [Description("创建标签")]
        public const string Create = "create:tag";

        [Description("更新标签")]
        public const string Update = "update:tag";

        [Description("更新标签排序")]
        public const string Sort = "sort:tag";

        [Description("删除标签")]
        public const string Delete = "delete:tag";

        [Description("获取标签")]
        public const string Get = "get:tag";

        [Description("获取标签分组列表")]
        public const string ListGroup = "list:group:tag";
    }
}
