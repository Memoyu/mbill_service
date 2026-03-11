using Memo.Bill.Application.Bills.Commands;

namespace Memo.Bill.Application.Common.Mappings;

public class BillRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<UpdateBillCommand, Billing>().IgnoreNullValues(true);
    }
}
