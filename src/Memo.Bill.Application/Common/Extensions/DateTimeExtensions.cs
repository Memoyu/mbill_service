using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Memo.Bill.Application.Common.Extensions;

public static class DateTimeExtensions
{
    public enum DataTimeRangeType
    {
        Week,
        Month,
        Season,
        Year
    }

    /// <summary>
    /// 获取指定时间范围类型的起止日期
    /// </summary>
    /// <param name="date"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static (DateTime Begin, DateTime End) GetRange(this DateTime date, DataTimeRangeType type)
    {
        var begin = DateTime.MinValue;
        var end = DateTime.MinValue;
        switch (type)
        {
            case DataTimeRangeType.Week:
                begin = date.AddDays(-(int)date.DayOfWeek + 1).Date;
                end = date.AddDays(7 - (int)date.DayOfWeek).Date;
                break;

            case DataTimeRangeType.Month:
                begin = date.AddDays(-date.Day + 1).Date;
                end = date.AddMonths(1).AddDays(-date.AddMonths(1).Day + 1).AddDays(-1).Date;
                break;

            case DataTimeRangeType.Season:
                var seasonBeginDate = date.AddMonths(0 - ((date.Month - 1) % 3));
                begin = seasonBeginDate.AddDays(-seasonBeginDate.Day + 1).Date;

                var seasonEndDate = date.AddMonths((3 - ((date.Month - 1) % 3) - 1));
                end = seasonEndDate.AddMonths(1).AddDays(-seasonEndDate.AddMonths(1).Day + 1).AddDays(-1).Date;

                break;
            case DataTimeRangeType.Year:
                begin = date.AddDays(-date.DayOfYear + 1).Date;

                var yearDate = date.AddYears(1);
                end = yearDate.AddDays(-yearDate.DayOfYear).Date;

                break;
        }

        return (begin, end);
    }

    /// <summary>
    /// 获取指定时间范围类型的日期集合
    /// </summary>
    /// <param name="date"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static List<DateTime> GetRanges(this DateTime date, DataTimeRangeType type)
    {
        var (begin, end) = date.GetRange(type);
        return begin.GetRanges(end);
    }

    /// <summary>
    /// 获取指定时间范围的日期集合
    /// </summary>
    /// <param name="begin"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static List<DateTime> GetRanges(this DateTime begin, DateTime end)
    {
        var dates = new List<DateTime> { begin };
        while (begin < end)
        {
            begin = begin.AddDays(1);
            dates.Add(begin);
        }

        return dates;
    }

    /// <summary>
    /// 获取指定日期的年份第一天
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime FirstDayOfYear(this DateTime date) => DateTime.Parse($"{date.Year}-01-01");

    /// <summary>
    /// 获取指定日期的年份最后一天
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime LastDayOfYear(this DateTime date) => date.FirstDayOfYear().AddYears(1).AddSeconds(-1);

    /// <summary>
    /// 获取指定日期的月份第一天
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime FirstDayOfMonth(this DateTime date) => DateTime.Parse($"{date.Year}-{date.Month}-01");

    /// <summary>
    /// 获取指定日期的月份最后一天
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime LastDayOfMonth(this DateTime date)
    {
        var begin = date.FirstDayOfMonth();
        return begin.AddDays(1 - begin.Day).Date.AddMonths(1).AddSeconds(-1);
    }

    /// <summary>
    /// 获取指定日期当天起始时间
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime FirstTimeOfDay(this DateTime date) => date.Date;

    /// <summary>
    /// 获取指定日期当天截止时间
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime LastTimeOfDay(this DateTime date) => date.FirstTimeOfDay().AddDays(1).AddSeconds(-1);
}
