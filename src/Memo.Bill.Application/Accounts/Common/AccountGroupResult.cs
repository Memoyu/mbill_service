namespace Memo.Bill.Application.Accounts.Common;

internal record AccountGroupResult
{
    public List<AccountGroupItem> Items { get; set; } = [];
}


internal record AccountGroupItem : AccountBaseResult
{
    public List<AccountResult> Childs { get; set; } = [];
}