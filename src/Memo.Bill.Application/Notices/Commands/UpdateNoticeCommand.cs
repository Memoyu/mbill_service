namespace Memo.Bill.Application.Notices.Commands;

[Authorize(Permissions = ApiPermission.Notice.Update)]
public record UpdateNoticeCommand(long NoticeId, string Content, string Range) : IAuthorizeableRequest<Result>;

public class UpdateNoticeCommandValidator : AbstractValidator<UpdateNoticeCommand>
{
    public UpdateNoticeCommandValidator()
    {
        RuleFor(x => x.NoticeId)
       .Must(x => x > 0)
       .WithMessage("通知Id必须大于0");

        RuleFor(x => x.Content)
            .MinimumLength(1)
            .MaximumLength(250)
            .WithMessage("公告内容长度在1-250个字符之间");
    }
}

public class UpdateNoticeCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Notice> noticeRepo
    ) : IRequestHandler<UpdateNoticeCommand, Result>
{
    public async Task<Result> Handle(UpdateNoticeCommand request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}
