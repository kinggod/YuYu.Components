using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace YuYu.Components
{
    /// <summary>
    /// ExtendMethods
    /// </summary>
    internal static class ExtendMethods
    {
        /// <summary>
        /// 取平台的字符串值
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        public static string GetString(this Platform platform)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (platform.Contains(Platform.Android))
                stringBuilder.AppendFormat("{0},", Platform.Android.ToString().ToLowerInvariant());
            if (platform.Contains(Platform.iOS))
                stringBuilder.AppendFormat("{0},", Platform.iOS.ToString().ToLowerInvariant());
            return stringBuilder.ToString().TrimEnd(',');
        }

        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="originalInput"></param>
        /// <returns></returns>
        public static string UrlEncode(this string originalInput)
        {
            return HttpUtility.UrlEncode(originalInput, Encoding.UTF8);
        }

        /// <summary>
        /// 指定的值是否包含目标平台
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool Contains(this Platform platform, Platform other)
        {
            return other == (platform & other);
        }
    }
}
