using Mapster;

namespace Mbill.Service.Common.Registers.PreOrder;

public class PreOrderRegister : BaseRegister
{
    protected override void TypeRegister(TypeAdapterConfig config)
    {
        config.ForType<PreOrderEntity, PreOrderSimpleDto>()
             .Map(d => d.Time, s => $"{s.Time.GetWeek()} {s.Time:MM月-dd日}");

        config.ForType<PreOrderGroupEntity, PreOrderGroupWithStatDto>()
            .Map(d => d.Time, s => $"{s.CreateTime.GetWeek()} {s.CreateTime.Day}日 {s.CreateTime:HH:mm}");

        config.ForType<PreOrderGroupEntity, GroupPreOrderStatDto>()
           .Map(d => d.Time, s => $"{s.CreateTime.GetWeek()} {s.CreateTime.Month}/{s.CreateTime.Day} {s.CreateTime:HH:mm}");
    }
}
