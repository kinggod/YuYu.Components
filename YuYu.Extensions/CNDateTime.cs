using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

namespace YuYu.Components
{
    /// <summary>
    /// 中国农历时间
    /// </summary>
    public struct CNDateTime : IComparable, ISerializable, IComparable<CNDateTime>, IEquatable<CNDateTime>
    {
        /// <summary>
        /// 当前日期的年份
        /// </summary>
        public int Year { get; private set; }

        /// <summary>
        /// 当前日期的月份
        /// </summary>
        public int Month { get; private set; }

        /// <summary>
        /// 当前日期的月的天
        /// </summary>
        public int DayOfMonth { get; private set; }

        /// <summary>
        /// 当前日期的时
        /// </summary>
        public int Hour { get; private set; }

        /// <summary>
        /// 当前日期的分
        /// </summary>
        public int Minute { get; private set; }

        /// <summary>
        /// 当前日期的秒
        /// </summary>
        public int Second { get; private set; }

        /// <summary>
        /// 当前日期的毫秒
        /// </summary>
        public double Milliseconds { get; private set; }

        /// <summary>
        /// 当前日期是否为闰年
        /// </summary>
        public bool IsLeap { get; private set; }

        /// <summary>
        /// 当前日期是否为闰月
        /// </summary>
        public bool IsLeapMonth { get; private set; }

        /// <summary>
        /// 当前日期是否为闰年
        /// </summary>
        public int LeapMonth { get; private set; }

        /// <summary>
        /// 当前日期的公历日期
        /// </summary>
        public DateTime DateTime { get; private set; }

        /// <summary>
        /// 将时间分成多个部分来表示，如分成年、月和日。年按农历计算，而日和月按阴阳历计算。
        /// </summary>
        public ChineseLunisolarCalendar ChineseLunisolarCalendar { get; private set; }

        /// <summary>
        /// 返回指定公历日期的阴历时间
        /// </summary>
        /// <param name="dateTime"></param>
        public CNDateTime(DateTime dateTime)
        {
            this = new CNDateTime();
            this.ChineseLunisolarCalendar = new ChineseLunisolarCalendar();
            if (dateTime > ChineseLunisolarCalendar.MaxSupportedDateTime || dateTime < ChineseLunisolarCalendar.MinSupportedDateTime)
                throw new Exception("参数日期时间不在支持的范围内,支持范围：" + ChineseLunisolarCalendar.MinSupportedDateTime.ToShortDateString() + "~" + ChineseLunisolarCalendar.MaxSupportedDateTime.ToShortDateString());
            this.Year = ChineseLunisolarCalendar.GetYear(dateTime);
            this.Month = ChineseLunisolarCalendar.GetMonth(dateTime);
            this.DayOfMonth = ChineseLunisolarCalendar.GetDayOfMonth(dateTime);
            this.Hour = ChineseLunisolarCalendar.GetHour(dateTime);
            this.Minute = ChineseLunisolarCalendar.GetMinute(dateTime);
            this.Second = ChineseLunisolarCalendar.GetSecond(dateTime);
            this.Milliseconds = ChineseLunisolarCalendar.GetMilliseconds(dateTime);
            this.IsLeap = ChineseLunisolarCalendar.IsLeapYear(this.Year);
            int leapMonth = ChineseLunisolarCalendar.GetLeapMonth(this.Year);
            if (this.Month >= leapMonth)
            {
                this.IsLeapMonth = this.Month == leapMonth;
                this.Month--;
            }
            this.LeapMonth = leapMonth - 1;
            this.DateTime = dateTime;
        }

        /// <summary>
        /// 返回指定年，月，日，是否闰月，时，分，秒的农历日期时间
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="dayOfMonth"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <param name="millisecond"></param>
        /// <param name="isLeapMonth"></param>
        public CNDateTime(int year, int month, int dayOfMonth, int hour, int minute, int second, int millisecond, bool isLeapMonth)
        {
            this = new CNDateTime();
            this.ChineseLunisolarCalendar = new ChineseLunisolarCalendar();
            if (year < ChineseLunisolarCalendar.MinSupportedDateTime.Year || year > ChineseLunisolarCalendar.MaxSupportedDateTime.Year)
                throw new Exception("参数年份时间不在支持的范围内,支持范围：" + ChineseLunisolarCalendar.MinSupportedDateTime.ToString() + "~" + ChineseLunisolarCalendar.MaxSupportedDateTime.ToString());
            if (month < 1 || month > 12)
                throw new Exception("月份必须在1~12范围");
            if (ChineseLunisolarCalendar.GetLeapMonth(year) - 1 != month && isLeapMonth)
                throw new Exception("指定的月份不是当年的闰月");
            if (ChineseLunisolarCalendar.GetDaysInMonth(year, isLeapMonth ? month + 1 : month) < dayOfMonth || dayOfMonth < 1)
                throw new Exception("指定的月中的天数不在当前月天数有效范围");
            this.Year = year;
            this.Month = month;
            this.DayOfMonth = dayOfMonth;
            this.Hour = hour;
            this.Minute = minute;
            this.Second = second;
            this.Milliseconds = millisecond;
            this.IsLeap = ChineseLunisolarCalendar.IsLeapYear(this.Year);
            this.IsLeapMonth = isLeapMonth;
            this.LeapMonth = ChineseLunisolarCalendar.GetLeapMonth(Year) - 1;
            this.DateTime = ChineseLunisolarCalendar.ToDateTime(year, (month > this.LeapMonth || isLeapMonth) ? month + 1 : month, dayOfMonth, hour, minute, second, millisecond);
        }

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}/{1}/{2} {3}:{4}:{5}", this.Year, this.Month, this.DayOfMonth, this.Hour, this.Minute, this.Second);
        }

        /// <summary>
        /// 当前农历日期
        /// </summary>
        public static CNDateTime Now
        {
            get { return new CNDateTime(DateTime.Now); }
        }

        /// <summary>
        /// 获取指定公历时间转换为农历时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static CNDateTime ToCNDateTime(DateTime dateTime)
        {
            return new CNDateTime(dateTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return this.GetHashCode() - obj.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("CNDateTime", this.DateTime.Ticks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(CNDateTime other)
        {
            return (int)(this.DateTime.Ticks - other.DateTime.Ticks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(CNDateTime other)
        {
            return this.DateTime.Ticks == other.DateTime.Ticks;
        }
    }
}