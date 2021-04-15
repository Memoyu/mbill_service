using AutoMapper;
using mbill_service.Core.Domains.Entities.Core;
using mbill_service.Service.Core.Log.Output;

namespace mbill_service.Service.Common.Mapper.Core
{
    public class LogMapper : Profile
    {
        public LogMapper()
        {
            CreateMap<LogEntity, LogDto>();
        }
    }
}
