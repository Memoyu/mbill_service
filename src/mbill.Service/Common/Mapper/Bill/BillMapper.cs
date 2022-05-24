﻿namespace mbill.Service.Common.Mapper.Bill;

public class BillMapper : Profile
{
    public BillMapper()
    {
        CreateMap<ModifyBillInput, BillEntity>()
            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => DateTime.Parse($"{src.Year}-{src.Month}-{src.Day} {src.Time}")));

        CreateMap<BillEntity, BillDto>()
            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time.ToShortTimeString()));

        CreateMap<BillEntity, BillDetailDto>()
            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time.ToShortTimeString()));
    }
}
