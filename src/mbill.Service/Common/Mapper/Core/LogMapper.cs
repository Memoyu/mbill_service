namespace mbill.Service.Common.Mapper.Core;

public class LogMapper : Profile
{
    public LogMapper()
    {
        CreateMap<LogEntity, LogDto>();
    }
}