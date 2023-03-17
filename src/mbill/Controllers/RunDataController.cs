using mbill.Core.Interface.IRepositories.Bill;
using mbill.Core.Interface.IRepositories.Core;

namespace mbill.Controllers;

[Route("api/rundata")]
public class RunDataController : ApiControllerBase
{
    private readonly IBillRepo _billRepo;
    private readonly IBillMongoRepo _billMongoRepo;

    public RunDataController(IBillRepo billRepo, IBillMongoRepo billMongoRepo)
    {
        _billRepo = billRepo;
        _billMongoRepo = billMongoRepo;
    }

    [HttpGet]
    public async Task<ServiceResult> BillWriteToMongoDBAsync()
    {
        var list = await _billRepo.Select.Where(b => b.IsDeleted == false).ToListAsync();
        var s = await _billMongoRepo.InsertManyAsync(list);
        return ServiceResult.Successed("成功！");
    }
}
