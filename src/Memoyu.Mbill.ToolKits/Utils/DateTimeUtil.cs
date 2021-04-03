using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.ToolKits.Utils
{
    public static class DateTimeUtil
    {
        /// <summary>
        /// 获得本月有几周及周日期数据(完整周)
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns></returns>
        public static List<WeeksOfMonth> GetWeeksOfMonth(int year, int month)
        {
            if (year <= 0 || month <= 0)
                throw new ArgumentException("年、月不能小于等于0");

            var weeksOfMonth = new List<WeeksOfMonth>();
            //当前月第一天
            DateTime monStart = new DateTime(year, month, 1);
            //该月的最后一天
            DateTime monEnd = monStart.AddMonths(1).AddDays(-1);
            int i = 1;
            //当前月第一天是星期几
            int dayOfWeek = Convert.ToInt32(monStart.DayOfWeek.ToString("d"));
            var weekStart = monStart;
            //确认这个月第一周的第一天
            if (dayOfWeek != 1)
                weekStart = monStart.AddDays(-(dayOfWeek - 1));

            //该月第一周结束日期
            DateTime weekEnd = dayOfWeek == 0 ? monStart : monStart.AddDays(7 - dayOfWeek);

            weeksOfMonth.Add(new WeeksOfMonth
            {
                Number = i,
                StartDate = weekStart.Date,
                EndDate = weekEnd
            });

            //当日期小于或等于该月的最后一天
            while (weekEnd.AddDays(1) <= monEnd)
            {
                i++;
                //该周的开始时间
                weekStart = weekEnd.AddDays(1);
                //该周结束时间
                weekEnd = weekEnd.AddDays(7);// > monEnd ? monEnd : weekEnd.AddDays(7);

                weeksOfMonth.Add(new WeeksOfMonth
                {
                    Number = i,
                    StartDate = weekStart.Date,
                    EndDate = weekEnd
                });
            }
            return weeksOfMonth;
        }

        /// <summary>
        /// 获得本月有几周及周日期数据
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns></returns>
        public static List<WeeksOfMonth> GetWeeksOnlyMonth(int year, int month)
        {
            if (year <= 0 || month <= 0)
                throw new ArgumentException("年、月不能小于等于0");

            var weeksOnlyMonth = new List<WeeksOfMonth>();
            //当前月第一天
            DateTime weekStart = new DateTime(year, month, 1);
            //该月的最后一天
            DateTime monEnd = weekStart.AddMonths(1).AddDays(-1);
            int i = 1;
            //当前月第一天是星期几
            int dayOfWeek = Convert.ToInt32(weekStart.DayOfWeek.ToString("d"));
            //该月第一周结束日期
            DateTime weekEnd = dayOfWeek == 0 ? weekStart : weekStart.AddDays(7 - dayOfWeek);

            weeksOnlyMonth.Add(new WeeksOfMonth
            {
                Number = i,
                StartDate = weekStart.Date,
                EndDate = weekEnd
            });

            //当日期小于或等于该月的最后一天
            while (weekEnd.AddDays(1) <= monEnd)
            {
                i++;
                //该周的开始时间
                weekStart = weekEnd.AddDays(1);
                //该周结束时间
                weekEnd = weekEnd.AddDays(7) > monEnd ? monEnd : weekEnd.AddDays(7);

                weeksOnlyMonth.Add(new WeeksOfMonth
                {
                    Number = i,
                    StartDate = weekStart.Date,
                    EndDate = weekEnd
                });
            }

            return weeksOnlyMonth;
        }

        /// <summary>
        /// 获得本月有几周及周日期数据
        /// </summary>
        /// <param name="date">基准时间</param>
        /// <param name="weekCount">获取周数</param>
        /// <param name="isLeft">是否为取基准时间前的周数，否则为基准时间后的周数</param>
        /// <returns></returns>
        public static List<WeeksOfMonth> GetWeeksByDate(DateTime date, int weekCount, bool isLeft = true)
        {
            if (date.CompareTo(DateTime.MinValue) == 0)
                throw new ArgumentException("基准时间有误");
            if (weekCount <= 0)
                throw new ArgumentException("获取周数不能小于等于0");

            var weeks = new List<WeeksOfMonth>();
            DateTime weekStart = date.AddDays(1 - Convert.ToInt32(date.DayOfWeek.ToString("d")));
            DateTime weekEnd = weekStart.AddDays(6);
            //当前周
            weeks.Add(new WeeksOfMonth
            {
                Number = isLeft ? weekCount : 1,//如果是往前，则为最后一周，否则为第一周
                StartDate = weekStart,
                EndDate = weekEnd
            });
            weekCount -= 1;
            while (weekCount > 0)
            {
                var baseDate = weekStart.AddDays(-1);
                if (isLeft)
                {
                    var preWeekStart = baseDate.AddDays(Convert.ToInt32(1 - Convert.ToInt32(baseDate.DayOfWeek)) - 7);        //上周一
                    var preWeekEnd = baseDate.AddDays(Convert.ToInt32(1 - Convert.ToInt32(baseDate.DayOfWeek)) - 7).AddDays(6);     //上周末（星期日）//下周
                    weeks.Add(new WeeksOfMonth
                    {
                        Number = weekCount,
                        StartDate = preWeekStart,
                        EndDate = preWeekEnd
                    });
                    weekStart = preWeekStart;
                }
                else
                {
                    var nextWeekStart = baseDate.AddDays(Convert.ToInt32(1 - Convert.ToInt32(baseDate.DayOfWeek)) + 7);        //下周一
                    var nextWeekEnd = baseDate.AddDays(Convert.ToInt32(1 - Convert.ToInt32(baseDate.DayOfWeek)) + 7).AddDays(6); //下周末
                    weeks.Add(new WeeksOfMonth
                    {
                        Number = weeks.Count + 1,
                        StartDate = nextWeekStart,
                        EndDate = nextWeekEnd
                    });
                    weekStart = nextWeekStart;
                }

                weekCount--;
            }

            return weeks;
        }
    }

    public class WeeksOfMonth
    {
        public int Number { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
