namespace Memo.Bill.Application.Accounts.Common;

internal record AccountGroupResult
{
    /// <summary>
    /// 常用项
    /// 数量为10个
    /// </summary>
    public List<AccountResult> Tops { get; set; } = [];

    public List<AccountGroupItem> Items { get; set; } = [];
}


internal record AccountGroupItem : AccountBaseResult
{
    public List<AccountResult> Childs { get; set; } = [];
}