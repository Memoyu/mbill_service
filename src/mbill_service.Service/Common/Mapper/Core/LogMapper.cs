namespace mbill_service.Service.Common.Mapper.Core;

public class LogMapper : Profile
{
    public LogMapper()
    {
        CreateMap<LogEntity, LogDto>();
    }
}