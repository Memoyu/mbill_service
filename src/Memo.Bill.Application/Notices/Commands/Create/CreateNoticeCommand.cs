namespace Memo.Bill.Application.Notices.Commands.Create;

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
