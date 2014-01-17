using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtendMethodsForDateTime
    {
        /// <summary>
        /// 获取当前时间是当前年中的第几周
        /// </summary>
        /// <param name="dateTime">当前时间</param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime dateTime)
        {
            DateTime firstDayOfYear = new DateTime(dateTime.Year, 1, 1);
            int skipWeek = firstDayOfYear.DayOfWeek > 0 ? 1 : 0;
            int days = dateTime.DayOfYear - (firstDayOfYear.DayOfWeek > 0 ? 7 - (int)firstDayOfYear.DayOfWeek : 0);
            int weekOfYear = days / 7 + 1;
            if (days % 7 > 0)
                weekOfYear += 1;
            return weekOfYear;
        }

        /// <summary>
        /// 获取当前时间与目标时间的间隔
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="taregtDateTime"></param>
        /// <returns></returns>
        public static TimeSpan TimeSpan(this DateTime dateTime, DateTime taregtDateTime)
        {
            return taregtDateTime - dateTime;
        }

        /// <summary>
        /// 获取当前时间与目标时间的间隔
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="taregtDateTime">目标时间</param>
        /// <param name="resultType">返回值类型。dd:天数;hh:小时数;mm:分钟数;ss:秒数;其它:毫秒数;</param>
        /// <returns></returns>
        public static double TimeSpan(this DateTime dateTime, DateTime taregtDateTime, string resultType)
        {
            TimeSpan ts = taregtDateTime - dateTime;
            double result = 0;
            switch (resultType.ToLower())
            {
                case "dd":
                    result = ts.TotalDays;
                    break;
                case "hh":
                    result = ts.TotalHours;
                    break;
                case "mm":
                    result = ts.TotalMinutes;
                    break;
                case "ss":
                    result = ts.TotalSeconds;
                    break;
                default:
                    result = ts.TotalMilliseconds;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 将当前日期转换为中国农历日期
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static CNDateTime ToCNDateTime(this DateTime dateTime)
        {
            return CNDateTime.ToCNDateTime(dateTime);
        }
    }
}