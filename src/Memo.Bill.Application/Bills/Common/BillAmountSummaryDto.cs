namespace Memo.Bill.Application.Bills.Common;

internal record BillAmountSummaryDto(long BillId, BillType Type, decimal Amount, DateTime Date);
