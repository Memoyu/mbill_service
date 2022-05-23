namespace mbill_service.Core.Domains.Common.Consts;

public class SystemConst
    {
        /// <summary>
        /// 数据库表前缀
        /// </summary>
        public const string DbTablePrefix = "mbill";

        #region API Doc

        public static class Grouping
        {
            /// <summary>
            /// 前台客户端接口组
            /// </summary>
            public const string GroupName_v1 = "v1";

            /// <summary>
            /// 后台管理端接口组
            /// </summary>
            public const string GroupName_v2 = "v2";

            /// <summary>
            /// 其他通用接口组
            /// </summary>
            public const string GroupName_v3 = "v3";
        }

        #endregion API Doc

        /// <summary>
        /// 默认角色
        /// </summary>
        public static class Role
        {
            /// <summary>
            /// 超级管理员
            /// </summary>
            public static int Administrator = 1;
            /// <summary>
            /// 普通管理员
            /// </summary>
            public static int Admin = 2;
            /// <summary>
            /// 用户
            /// </summary>
            public static int User = 3;
        }


        /// <summary>
        /// 选取器
        /// </summary>
        public class Switcher
        {
            public static string StatementType(string type) => type switch
            {
                "expend" => "支出",
                "income" => "收入",
                "transfer" => "转账",
                "repayment" => "还款",
                _ => "",
            };

            public static string CategoryType(string type) => type switch
            {
                "expend" => "支出",
                "income" => "收入",
                _ => "",
            };

            public static string AssetType(string type) => type switch
            {
                "deposit" => "储蓄",
                "debt" => "债务",
                _ => "",
            };
        }
    }
