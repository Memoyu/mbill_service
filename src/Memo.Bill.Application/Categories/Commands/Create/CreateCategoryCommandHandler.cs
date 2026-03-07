namespace Memo.Bill.Application.Categories.Commands.Create;

public class CreateAccountCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Category> categoryRepo
    ) : IRequestHandler<CreateCategoryCommand, Result>
{
    public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}