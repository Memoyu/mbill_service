using Mapster;

namespace Mbill.Service.Common.Registers.Bill;

public class BillRegister : BaseRegister
{
    protected override void TypeRegister(TypeAdapterConfig config)
    {
        config.ForType<BillEntity, BillSimpleDto>()
            .Map(d => d.CategoryIcon, s => UrlConverter(s.Category.Icon))
            .Map(d => d.Category, s => s.Category == null ? string.Empty : s.Category.Name)
            .Map(d => d.AmountFormat, s => s.Amount.AmountFormat())
            .Map(d => d.Date, s => s.Time.ToString("yyyy-MM-dd"))
            .Map(d => d.Time, s => s.Time.ToString("HH:mm"));

        config.ForType<BillEntity, BillDetailDto>()
            .Map(d => d.CategoryIcon, s => UrlConverter(s.Category.Icon))
            .Map(d => d.Category, s => s.Category == null ? string.Empty : s.Category.Name)
            .Map(d => d.AssetIcon, s => UrlConverter(s.Asset.Icon))
            .Map(d => d.Asset, s => s.Asset == null ? string.Empty : s.Asset.Name)
            .Map(d => d.AmountFormat, s => s.Amount.AmountFormat())
            .Map(d => d.TimeFormat, s => $"{s.Time.GetWeek()} {s.Time:yyyy-MM-dd HH:mm}");
    }
}
