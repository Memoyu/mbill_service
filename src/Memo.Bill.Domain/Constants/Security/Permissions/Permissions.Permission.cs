namespace Memo.Bill.Domain.Constants.Security.Permissions;

public static partial class Permissions
{
    [Description("权限")]
    public static class Permission
    {
        //[Description("创建权限")]
        //public const string Create = "create:permission";

        //[Description("更新权限")]
        //public const string Update = "update:permission";

        [Description("删除权限")]
        public const string Delete = "delete:permission";

        [Description("获取权限")]
        public const string Get = "get:permission";

        [Description("获取权限列表")]
        public const string List = "lsit:permission";

        [Description("获取权限分组列表")]
        public const string Group = "group:permission";

    }
}
