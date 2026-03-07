namespace Memo.Bill.Application.Notices.Commands.Update;

public class UpdateUserCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Notice> noticeRepo
    ) : IRequestHandler<UpdateNoticeCommand, Result>
{
    public async Task<Result> Handle(UpdateNoticeCommand request, CancellationToken cancellationToken)
    {      
        return Result.Success();
    }
}
