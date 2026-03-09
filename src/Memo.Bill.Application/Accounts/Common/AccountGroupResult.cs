namespace Memo.Bill.Application.Accounts.Common;

public record AccountGroupResult
{
    public long AccountId { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<AccountResult> Childs { get; set; } = [];
}
