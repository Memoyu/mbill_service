namespace Mbill.Core.Domains.Common.Enums;

public enum PreOrderStatusEnum
{
    UnDone,//未购买
    Done,//已购买
}

public enum BillTypeEnum
{
    expend,//支出
    income,//收入
    transfer,//转账
    repayment,//还款
}

public enum RoleType
{
    User, // 普通用户
    Administrator, // 超级管理员
    Admin, // 管理员
    Custom, // 用户自定义
}

public enum DataSeedType
{
    [Description("账单分类种子数据")]
    BillCategory, // 账单分类
    [Description("资产分类种子数据")]
    AssetCategory, // 资产分类
}
