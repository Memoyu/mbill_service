namespace Memo.Bill.Application.Ledgers.Commands;

[Authorize(Permissions = ApiPermission.Ledger.UpdateSort)]
public record UpdateLedgerSortCommand : IAuthorizeableRequest<Result>;

