using Memo.Bill.Application.Bills.Commands;
using Memo.Bill.Application.Bills.Queries;

namespace Memo.Bill.Application.Common.Mappings;

public class BillRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<UpdateBillCommand, Billing>().IgnoreNullValues(true);

        config.ForType<SearchBillQuery, BillSearchRecord>()
            .Map(d => d.Types, s => (s.Types ?? new()).ToJson())
            .Map(d => d.CategoryIds, s => (s.CategoryIds ?? new()).ToJson())
            .Map(d => d.AccountIds, s => (s.AccountIds ?? new()).ToJson())
            .IgnoreNullValues(true);
    }
}
