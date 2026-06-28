using Memo.Bill.Application.Users.Common;

namespace Memo.Bill.Application.Ledgers.Common;

public record LedgerWithCreaterResult : LedgerBaseResult
{
    public UserBaseResult Creater { get; set; } = new();

    public List<UserBaseResult> Users { get; set; } = [];
}
