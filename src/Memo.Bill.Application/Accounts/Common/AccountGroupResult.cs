namespace Memo.Bill.Application.Accounts.Common;

internal record AccountGroupResult
{
    public List<AccountWithChildResult> Tops { get; set; } = [];

    public List<AccountWithChildResult> List { get; set; } = [];
}
