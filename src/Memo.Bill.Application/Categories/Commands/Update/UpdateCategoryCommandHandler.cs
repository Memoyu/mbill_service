
namespace Memo.Bill.Application.Categories.Commands.Update;

public class UpdateBillCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Category> categoryRepo
    ) : IRequestHandler<UpdateCategoryCommand, Result>
{
    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}