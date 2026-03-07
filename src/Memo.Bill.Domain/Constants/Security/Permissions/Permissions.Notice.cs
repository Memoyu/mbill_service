namespace Memo.Bill.Domain.Constants.Security.Permissions;

public static partial class Permissions
{
    [Description("公告")]
    public static class Notice
    {
        [Description("创建公告")]
        public const string Create = "create:notice";

        [Description("更新公告")]
        public const string Update = "update:notice";
        
        [Description("删除公告")]
        public const string Delete = "delete:notice";
    }
}
