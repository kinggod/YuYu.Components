using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace YuYu.Components
{
    /// <summary>
    /// CultureInfo 类扩展方法
    /// </summary>
    public static class ExtendMethodsForCultureInfo
    {
        /// <summary>
        /// 获取当前区域的语言类型并转换成 Cultures 枚举值
        /// </summary>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static Culture GetCulture(this CultureInfo cultureInfo)
        {
            return cultureInfo.Name.Replace('-', '_').ToEnum<Culture>(true);
        }
    }
}
