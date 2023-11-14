using Mapster;
using Mbill.Service.Core.Notice.Input;

namespace Mbill.Service.Common.Registers.Core;

internal class NoticeRegister : BaseRegister
{
    protected override void TypeRegister(TypeAdapterConfig config)
    {
        config.ForType<ModifyNoticeDto, NoticeEntity>()
             .Map(d => d.Range, s => JsonConvert.SerializeObject((string.IsNullOrWhiteSpace(s.Range) ? "" : s.Range).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)));
    }
}
