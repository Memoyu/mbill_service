namespace Memo.Bill.Application.Ledgers.Commands;

[Authorize(Permissions = ApiPermission.Ledger.Delete)]
public record DeleteLedgerCommand : IAuthorizeableRequest<Result>;