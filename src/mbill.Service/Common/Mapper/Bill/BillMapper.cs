namespace mbill.Service.Common.Mapper.Bill;

public class BillMapper : Profile
{
    public BillMapper()
    {
        CreateMap<ModifyBillInput, BillEntity>();

        CreateMap<BillEntity, BillDto>();

        CreateMap<BillEntity, BillSimpleDto>()
            .ForMember(dest => dest.AmountFormat, opt => opt.MapFrom(src => src.Amount.AmountFormat()));

        CreateMap<BillEntity, BillDetailDto>()
            .ForMember(dest => dest.AmountFormat, opt => opt.MapFrom(src => src.Amount.AmountFormat()))
            .ForMember(dest => dest.TimeFormat, opt => opt.MapFrom(src => $"{src.Time.GetWeek()} {src.Time:yyyy-MM-dd HH:mm}"));
    }
}
