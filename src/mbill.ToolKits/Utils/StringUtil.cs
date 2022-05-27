namespace mbill.ToolKits.Utils;

public static class StringUtil
{
    public static string AmountFormat(this decimal amount)
    {
        return string.Format("{0:#,0.##}", amount);
    }
}
