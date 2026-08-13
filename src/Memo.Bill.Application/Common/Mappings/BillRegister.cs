using Memo.Bill.Application.Common.Interfaces.Services.Text;
using Memo.Bill.Domain.Entities.Mongo;

namespace Memo.Bill.Application.Common.Mappings;

public class BillRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<Billing, BillingCollection>()
           .Map(d => d.Keyword, s => GetKeyword(s))
           .IgnoreNullValues(true);
    }

    private string GetKeyword(Billing bill)
    {
        var segmenter = MapContext.Current.GetService<ISegmenterService>();

        var keywords = new List<string>
        {
            segmenter.CutWithSplitForSearch(bill.Remark),
            segmenter.CutWithSplitForSearch(bill.Address),
            segmenter.CutWithSplitForSearch(bill.Category.Name),
            segmenter.CutWithSplitForSearch(bill.Account.Name),
            segmenter.CutWithSplitForSearch(bill.Ledger.Name),
            segmenter.CutWithSplitForSearch(bill.Category.Name),
        };
        keywords.AddRange(bill.Tags.Select(t => segmenter.CutWithSplitForSearch(t.Name)));

        return string.Join(";", keywords);
    }
}
