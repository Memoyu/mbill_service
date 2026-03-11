namespace Memo.Bill.Application.Common.Extensions;

public static class ValueExtensions
{
    public static string FormatAmount(this decimal amount)
    {
        return string.Format("{0:#,0.##}", amount);
    }
}
