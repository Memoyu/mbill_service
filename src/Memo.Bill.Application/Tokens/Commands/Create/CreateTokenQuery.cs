namespace Memo.Bill.Application.Tokens.Commands.Create;

public record CreateTokenQuery(string Username, string Password) : IRequest<Result>;

public class CreateTokenQueryValidator : AbstractValidator<CreateTokenQuery>
{
    public CreateTokenQueryValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("用户名不能为空");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("密码不能为空");
    }
}

