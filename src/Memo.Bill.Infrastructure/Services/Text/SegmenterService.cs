using JiebaNet.Segmenter;
using Memo.Bill.Application.Common.Interfaces.Services.Text;

namespace Memo.Bill.Infrastructure.Services.Text;

[AppService(ServiceLifeType = ServiceLifeType.Singleton)]
public class SegmenterService(JiebaSegmenter jiebaSegmenter) : ISegmenterService
{
    public List<string> Cut(string text, bool cutAll = false, bool hmm = true)
    {
        return [.. jiebaSegmenter.Cut(text, cutAll, hmm)];
    }

    public string CutWithSplit(string text, string split = " ", bool cutAll = false, bool hmm = true)
    {
        var segs = jiebaSegmenter.Cut(text, cutAll, hmm);

        return string.Join(split, segs);
    }

    public List<string> CutForSearch(string text, bool hmm = true)
    {
        return [.. jiebaSegmenter.CutForSearch(text, hmm)];
    }

    public string CutWithSplitForSearch(string text, string split = " ", bool hmm = true)
    {
        var segs = jiebaSegmenter.CutForSearch(text, hmm);

        return string.Join(split, segs);
    }
}
