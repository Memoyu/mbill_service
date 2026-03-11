namespace Memo.Bill.Application.Notices.Commands;

[Authorize(Permissions = ApiPermission.Notice.Delete)]
public record DeleteNoticeCommand(long NoticeId) : IAuthorizeableRequest<Result>;

public class DeleteNoticeCommandValidator : AbstractValidator<DeleteNoticeCommand>
{
    public DeleteNoticeCommandValidator()
    {
        RuleFor(x => x.NoticeId)
       .Must(x => x > 0)
       .WithMessage("通知Id必须大于0");
    }
}

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
