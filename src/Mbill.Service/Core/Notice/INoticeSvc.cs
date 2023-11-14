using Mbill.Service.Core.Notice.Input;
using Mbill.Service.Core.Notice.Output;

namespace Mbill.Service.Core.Notice;

public interface INoticeSvc : IApplicationSvc
{
    /// <summary>
    /// 新增公告
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<NoticeDto>> InsertAsync(ModifyNoticeDto input);

    /// <summary>
    /// 获取最新的公告
    /// </summary>
    /// <returns></returns>
    Task<ServiceResult<NoticeDto>> GetLatestAsync();
}
