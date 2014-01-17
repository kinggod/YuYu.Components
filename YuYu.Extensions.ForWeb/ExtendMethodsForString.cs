using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    internal static class ExtendMethodsForString
    {
        /// <summary>
        /// 是否为IPv4格式字符串
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        internal static bool IsIPv4Format(this string ipAddress)
        {
            if (ipAddress == null || ipAddress == string.Empty || ipAddress.Length < 7 || ipAddress.Length > 15)
                return false;
            return Regex.IsMatch(ipAddress, @"^/d{1,3}[/.]/d{1,3}[/.]/d{1,3}[/.]/d{1,3}$", RegexOptions.IgnoreCase);
        }
    }
}
