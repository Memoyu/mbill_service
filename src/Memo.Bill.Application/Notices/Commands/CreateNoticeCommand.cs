namespace Memo.Bill.Application.Notices.Commands;

[Authorize(Permissions = ApiPermission.Notice.Create)]
public record CreateNoticeCommand(string Content, string Range) : IAuthorizeableRequest<Result>;

public class CreateUserCommandValidator : AbstractValidator<CreateNoticeCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Content)
            .MinimumLength(1)
            .MaximumLength(250)
            .WithMessage("公告内容长度在1-250个字符之间");
    }
}

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
