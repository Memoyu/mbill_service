namespace Memo.Bill.Application.Common.Security;

public record CurrentUser(
    long Id,
    string Username,
    string Email);
