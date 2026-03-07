namespace Memo.Bill.Application.Notices.Commands.Delete;

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
