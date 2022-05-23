namespace mbill_service.Service.Core.Logger;

public interface ILogSvc
{
    /// <summary>
    /// 获取日志分页
    /// </summary>
    /// <param name="pagingDto"></param>
    /// <returns></returns>
    Task<PagedDto<LogDto>> GetPagesAsync(LogPagingDto pagingDto);
}