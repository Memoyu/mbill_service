using Mapster;

namespace Mbill.Service.Common.Registers.Bill;

public class BillRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<BillEntity, BillSimpleDto>()
       .Map(d => d.AmountFormat, s => s.Amount.AmountFormat());

        config.ForType<BillEntity, BillDetailDto>()
            .Map(d => d.AmountFormat, s => s.Amount.AmountFormat())
            .Map(d => d.TimeFormat, s => $"{s.Time.GetWeek()} {s.Time:yyyy-MM-dd HH:mm}");
    }
}
