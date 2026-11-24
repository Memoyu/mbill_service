using Memo.Bill.Application.Common.Interfaces.Persistence.Repositories;
using Memo.Bill.Domain.Entities;
using Memo.Bill.Domain.Entities.Mongo;
using Memo.Bill.Domain.Events.Bills;

namespace Memo.Bill.Api.Controllers
{
    [AllowAnonymous]
    public class RunDataController(IPublisher publisher, IBaseDefaultRepository<Billing> billRepo, IBaseMongoRepository<BillingCollection> billMongoRepo) : ApiControllerBase
    {
        ///// <summary>
        ///// 账单数据写入mongodb
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("to-mongodb")]
        //public async Task<Result> ToMongodb()
        //{
        //    var bills = await billRepo.Select
        //        .Include(a => a.Category)
        //        .Include(a => a.Account)
        //        .Include(a => a.Ledger)
        //        .ToListAsync();
        //    foreach (var b in bills)
        //    {
        //        var bill = await billMongoRepo.FindOneAsync(b.BillId, false, null);
        //        if (bill != null) continue;

        //        b.Category ??= new();
        //        b.Account ??= new();
        //        await publisher.Publish(new CreateBillEvent(b));
        //    }

        //    return Result.Success("数据处理完成");
        //}
    }
}
