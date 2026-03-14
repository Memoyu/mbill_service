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

        [Description("账单搜索")]
        public const string Search = "search:bill";

        [Description("获取搜索账单记录")]
        public const string SearchRecord = "search:record:bill";

        [Description("获取账单分页")]
        public const string Page = "page:bill";

        [Description("获取账单分页日期分组")]
        public const string PageGroupDate = "page:group:date:bill";

        [Description("获取账单日历")]
        public const string Calendar = "calendar:bill";

        [Description("获取账单汇总金额")]
        public const string SummaryAmount = "summary:amount:bill";

        [Description("获取账单汇总分类")]
        public const string SummaryCategory = "summary:category:bill";
        
        [Description("获取账单排行榜")]
        public const string Ranking = "ranking:bill";
    }
}
