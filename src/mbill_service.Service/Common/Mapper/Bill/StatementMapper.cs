using AutoMapper;
using mbill_service.Core.Domains.Entities.Bill;
using mbill_service.Service.Bill.Statement.Input;
using mbill_service.Service.Bill.Statement.Output;
using System;

namespace mbill_service.Service.Common.Mapper.Bill
{
    public class StatementMapper : Profile
    {
        public StatementMapper()
        {
            CreateMap<ModifyStatementDto, StatementEntity>()
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => DateTime.Parse($"{src.Year}-{src.Month}-{src.Day} {src.Time}")));

            CreateMap<StatementEntity, StatementDto>()
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time.ToLongTimeString()));

            CreateMap<StatementEntity, StatementDetailDto>()
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time.ToLongTimeString()));
        }
    }
}
