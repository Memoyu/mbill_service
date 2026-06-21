namespace Memo.Bill.Application.Ledgers.Commands;


[Authorize(Permissions = ApiPermission.Ledger.Join)]
public record JoinLedgerUserCommand : IAuthorizeableRequest<Result>;