using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace YuYu.Components
{
    /// <summary>
    /// 对 HttpRequest 类型的扩展
    /// </summary>
    public static class ExtendMethodsForString
    {
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="originalInput">原始字符串</param>
        /// <param name="encoding">字符编码，默认为Encoding.Default</param>
        /// <returns></returns>
        public static string UrlEncode(this string originalInput, Encoding encoding = null)
        {
            return HttpUtility.UrlEncode(originalInput, encoding ?? Encoding.UTF8);
        }
    }
}
