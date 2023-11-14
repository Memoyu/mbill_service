using Mbill.Core.Common;
using Mbill.Service.Core.Notice.Input;
using Mbill.Service.Core.Notice.Output;

namespace Mbill.Service.Core.Notice;

public class NoticeSvc : ApplicationSvc, INoticeSvc
{
    private readonly ILogger _logger;
    private readonly INoticeRepo _noticeRepo;

    public NoticeSvc(ILoggerFactory loggerFactory, INoticeRepo noticeRepo)
    {
        _logger = loggerFactory.CreateLogger<NoticeSvc>();
        _noticeRepo = noticeRepo;
    }

    public async Task<ServiceResult<NoticeDto>> InsertAsync(ModifyNoticeDto input)
    {
        var entity = Mapper.Map<NoticeEntity>(input);
        entity.BId = SnowFlake.NextId();
        var dto = await _noticeRepo.InsertAsync(entity);
        return ServiceResult<NoticeDto>.Successed(Mapper.Map<NoticeDto>(dto));
    }

    public async Task<ServiceResult<NoticeDto>> GetLatestAsync()
    {
        var notice = await _noticeRepo.Select.OrderByDescending(n => n.CreateTime).FirstAsync();
        if (notice is not null)
        {
            // 不再可见范围内，则不返回
            var ranges = JsonConvert.DeserializeObject<List<long>>(string.IsNullOrWhiteSpace(notice.Range) ? "[]" : notice.Range);
            if (ranges.Count > 0 && !ranges.Any(r => r == CurrentUser.BId)) notice = null;
        }

        return ServiceResult<NoticeDto>.Successed(Mapper.Map<NoticeDto>(notice ?? new NoticeEntity()));
    }
}
