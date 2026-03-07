namespace Memo.Bill.Application.Notices.Commands.Create;

public class CreateUserCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Notice> noticeRepo
    ) : IRequestHandler<CreateNoticeCommand, Result>
{
    public async Task<Result> Handle(CreateNoticeCommand request, CancellationToken cancellationToken)
    {      
        return Result.Success();
    }
}
