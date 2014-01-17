using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtendMethodsForDateTime
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="cycleMode"></param>
        /// <returns></returns>
        public static string GetDirectoryName(this DateTime dateTime, string cycleMode = "DD")
        {
            string directoryName = string.Empty;
            cycleMode = cycleMode ?? string.Empty;
            switch (cycleMode.ToUpper())
            {
                case "MM":
                    directoryName = dateTime.ToString("yyyyMM");
                    break;
                case "WW":
                    directoryName = dateTime.ToString("yyyy") + _WeekOfYear(dateTime);
                    break;
                case "DD":
                default:
                    directoryName = dateTime.ToString("yyMMdd");
                    break;
            }
            return directoryName;
        }

        private static string _WeekOfYear(DateTime dateTime)
        {
            DateTime firstDayOfYear = new DateTime(dateTime.Year, 1, 1);
            int skipWeek = firstDayOfYear.DayOfWeek > 0 ? 1 : 0;
            int days = dateTime.DayOfYear - (firstDayOfYear.DayOfWeek > 0 ? 7 - (int)firstDayOfYear.DayOfWeek : 0);
            int weekOfYear = days / 7 + 1;
            if (days % 7 > 0)
                weekOfYear += 1;
            if (weekOfYear > 9)
                return weekOfYear.ToString();
            else
                return "0" + weekOfYear;
        }
    }
}
