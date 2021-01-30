/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Mapper.Bill.Statement
*   文件名称 ：StatementMapper.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-07 0:18:08
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using AutoMapper;
using Memoyu.Mbill.Application.Contracts.Dtos.Bill.Statement;
using Memoyu.Mbill.Domain.Entities.Bill.Statement;

namespace Memoyu.Mbill.Application.Contracts.Mapper.Bill.Statement
{
    public class StatementMapper : Profile
    {
        public StatementMapper()
        {
            CreateMap<ModifyStatementDto, StatementEntity>()
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Date.Year))
                .ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.Date.Month))
                .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.Date.Day));

            CreateMap<StatementEntity, StatementDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => string.Format("{0}-{1}-{2}", src.Year, src.Month, src.Day)))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>  src.Address));
        }
    }
}
