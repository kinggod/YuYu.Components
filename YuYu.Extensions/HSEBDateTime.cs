using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace YuYu.Components
{
    /// <summary>
    /// 使用天干地支纪年表示的时间
    /// </summary>
    public struct HSEBDateTime : IComparable, ISerializable, IComparable<HSEBDateTime>, IEquatable<HSEBDateTime>
    {
        /// <summary>
        /// 干支年
        /// </summary>
        public string Year { get; private set; }

        /// <summary>
        /// 阴历月
        /// </summary>
        public string Month { get; private set; }

        /// <summary>
        /// 阴历日
        /// </summary>
        public string DayOfMonth { get; private set; }

        /// <summary>
        /// 时辰
        /// </summary>
        public string Chrono { get; private set; }

        /// <summary>
        /// 当前时辰的刻度
        /// </summary>
        public string ClepsydraOfChrono { get; private set; }

        /// <summary>
        /// 当日刻度
        /// </summary>
        public string ClepsydraOfDay { get; private set; }

        /// <summary>
        /// 公历日期
        /// </summary>
        public DateTime DateTime { get; private set; }

        /// <summary>
        /// 农历日期
        /// </summary>
        public CNDateTime CNDateTime { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dateTime">公历日期</param>
        public HSEBDateTime(DateTime dateTime)
        {
            this = new HSEBDateTime();
            this.DateTime = dateTime;
            this.CNDateTime = CNDateTime.ToCNDateTime(dateTime);
            this._Initialize();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cnDateTime">农历日期</param>
        public HSEBDateTime(CNDateTime cnDateTime)
        {
            this = new HSEBDateTime();
            this.CNDateTime = cnDateTime;
            this.DateTime = this.CNDateTime.DateTime;
            this._Initialize();
        }

        /// <summary>
        /// 转换为字符串输出
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("农历{0}年{1}{2}{3}{4}", this.Year, this.Month, this.DayOfMonth, this.Chrono, this.ClepsydraOfChrono);
        }

        /// <summary>
        /// 获取指定年份的天干地支纪年
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static string GetHSEB(int year)
        {
            return _HSEB(year);
        }

        /// <summary>
        /// 天干
        /// </summary>
        public const string HeavenlyStems = "甲乙丙丁戊己庚辛壬癸";

        /// <summary>
        /// 地支
        /// </summary>
        public const string EarthlyBranches = "子丑寅卯辰巳午未申酉戌亥";

        private void _Initialize()
        {
            this.Year = _HSEB(this.CNDateTime.Year);
            if (this.CNDateTime.IsLeapMonth)
                this.Month = "闰月";
            else
            {
                string month = string.Empty;
                switch (this.CNDateTime.Month)
                {
                    case 1:
                        month = "正";
                        break;
                    case 11:
                        month = "冬";
                        break;
                    case 12:
                        month = "腊";
                        break;
                    default:
                        month = this.CNDateTime.Month.ToCNNumber(false);
                        break;
                }
                this.Month = month + "月";
            }
            if (this.CNDateTime.DayOfMonth == 30)
                this.DayOfMonth = "三十";
            else if (this.CNDateTime.DayOfMonth > 20)
                this.DayOfMonth = "廿" + (this.CNDateTime.DayOfMonth % 20).ToCNNumber(false);
            else if (this.CNDateTime.DayOfMonth == 20)
                this.DayOfMonth = "二十";
            else if (this.CNDateTime.DayOfMonth > 10)
                this.DayOfMonth = "十" + (this.CNDateTime.DayOfMonth % 10).ToCNNumber(false);
            else
                this.DayOfMonth = "初" + this.CNDateTime.DayOfMonth.ToCNNumber(false);
            int index = (this.CNDateTime.Hour + 1) / 2;
            if (index >= 12)
                index = 0;
            this.Chrono = EarthlyBranches[index] + "时";
            this.ClepsydraOfChrono = ((((this.CNDateTime.Hour + 1) % 2) * 3600 + this.CNDateTime.Minute * 60 + this.CNDateTime.Second) / 864).ToCNNumber(false) + "刻";
            this.ClepsydraOfDay = ((this.CNDateTime.Hour * 3600 + this.CNDateTime.Minute * 60 + this.CNDateTime.Second) / 864).ToCNNumber(false) + "刻";
        }

        private static string _HSEB(int year)
        {
            int num = (year - 3) % 60;
            return HeavenlyStems[num % HeavenlyStems.Length - 1].ToString() + EarthlyBranches[num % EarthlyBranches.Length - 1].ToString();
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
            info.AddValue("HSEBDateTime", this.DateTime.Ticks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(HSEBDateTime other)
        {
            return (int)(this.DateTime.Ticks - other.DateTime.Ticks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(HSEBDateTime other)
        {
            return this.DateTime.Ticks == other.DateTime.Ticks;
        }
    }
}
