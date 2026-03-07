namespace Memo.Bill.Application.Notices.Commands.Delete;

public class UpdateUserCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Notice> noticeRepo
    ) : IRequestHandler<DeleteNoticeCommand, Result>
{
    public async Task<Result> Handle(DeleteNoticeCommand request, CancellationToken cancellationToken)
    {      
        return Result.Success();
    }
}
