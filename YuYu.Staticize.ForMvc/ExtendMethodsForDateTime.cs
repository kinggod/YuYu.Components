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
                    directoryName = dateTime.ToString("yyyy") + dateTime.WeekOfYear().ToString("X2");
                    break;
                case "DD":
                default:
                    directoryName = dateTime.ToString("yyMMdd");
                    break;
            }
            return directoryName;
        }
    }
}
