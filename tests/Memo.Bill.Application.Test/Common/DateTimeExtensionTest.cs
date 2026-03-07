using Memo.Bill.Application.Common.Extensions;
using static Memo.Bill.Application.Common.Extensions.DateTimeExtensions;

namespace Memo.Bill.Application.Test.Common;
public class DateTimeExtensionTest
{
    [Fact]
    public void GetRange_Should_Success()
    {
        var date = DateTime.Parse("2024-04-11 13:46");
        var (begin, end) = date.GetRange(DataTimeRangeType.Week);
        Assert.Equal(begin, DateTime.Parse("2024-04-08 00:00"));
        Assert.Equal(end, DateTime.Parse("2024-04-14 00:00"));

        (begin, end) = date.GetRange(DataTimeRangeType.Month);
        Assert.Equal(begin, DateTime.Parse("2024-04-01 00:00"));
        Assert.Equal(end, DateTime.Parse("2024-04-30 00:00"));

        (begin, end) = date.GetRange(DataTimeRangeType.Season);
        Assert.Equal(begin, DateTime.Parse("2024-04-01 00:00"));
        Assert.Equal(end, DateTime.Parse("2024-04-30 00:00"));

        (begin, end) = date.GetRange(DataTimeRangeType.Year);
        Assert.Equal(begin, DateTime.Parse("2024-01-01 00:00"));
        Assert.Equal(end, DateTime.Parse("2024-12-31 00:00"));
    }

    [Fact]
    public void GetRanges_Should_Success()
    {
        var date = DateTime.Parse("2024-04-11 13:46");
        var ranges = date.GetRanges(DataTimeRangeType.Week);
       
    }
}
