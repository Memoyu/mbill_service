namespace Memo.Bill.Application.Accounts.Common;

internal record AccountGroupResult : AccountBaseResult
{
    public List<AccountResult> Childs { get; set; } = [];
}
