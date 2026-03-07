namespace Memo.Bill.Application.Categories.Commands.Delete;

public class DeleteCategoryCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Category> categoryRepo
    ) : IRequestHandler<DeleteCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}