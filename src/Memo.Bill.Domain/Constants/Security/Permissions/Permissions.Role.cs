namespace Memo.Bill.Domain.Constants.Security.Permissions;

public static partial class Permissions
{
    [Description("角色")]
    public static class Role
    {
        [Description("创建角色")]
        public const string Create = "create:role";   

        [Description("更新角色")]
        public const string Update = "update:role";
      
        [Description("删除角色")]
        public const string Delete = "delete:role";

        [Description("获取角色")]
        public const string Get = "get:role";

        [Description("获取角色列表")]
        public const string List = "list:role";
    }
}
