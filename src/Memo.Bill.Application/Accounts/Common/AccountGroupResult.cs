namespace Memo.Bill.Application.Accounts.Common;

internal record AccountGroupResult
{
    public long AccountId { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<AccountResult> Childs { get; set; } = [];
}
