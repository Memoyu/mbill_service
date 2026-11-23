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
        };
        keywords.AddRange(bill.Tags.Select(t => segmenter.CutWithSplitForSearch(t.Name)));

        var result = string.Join(";", keywords);
        // 移除 jieba 分词拆散 emoji 代理对产生的孤立代理字符
        return new string([.. result.Where(c => !char.IsSurrogate(c))]);
    }
}
