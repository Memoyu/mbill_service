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
        /// 获得本月有几周及周日期数据
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static List<WeeksOfMonth> GetWeeksOfMonth(int year, int month)
        {
            if (year <= 0 || month <= 0)
                throw new ArgumentException("年、月不能小于等于0");

            var weeksOfMonth = new List<WeeksOfMonth>();
            //当前月第一天
            DateTime weekStart = new DateTime(year, month, 1);
            //该月的最后一天
            DateTime monEnd = weekStart.AddMonths(1).AddDays(-1);
            int i = 1;
            //当前月第一天是星期几
            int dayOfWeek = Convert.ToInt32(weekStart.DayOfWeek.ToString("d"));
            //该月第一周结束日期
            DateTime weekEnd = dayOfWeek == 0 ? weekStart : weekStart.AddDays(7 - dayOfWeek);

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
                weekEnd = weekEnd.AddDays(7) > monEnd ? monEnd : weekEnd.AddDays(7);

                weeksOfMonth.Add(new WeeksOfMonth
                {
                    Number = i,
                    StartDate = weekStart.Date,
                    EndDate = weekEnd
                });
            }

            return weeksOfMonth;
        }
    }

    public class WeeksOfMonth
    {
        public int Number { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
