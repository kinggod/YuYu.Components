using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace YuYu.Components
{
    /// <summary>
    /// 数值类型的扩展方法
    /// </summary>
    public static class ExtendMethodsForNumber
    {
        /// <summary>
        /// 检索枚举中是否存在指定 Int32 值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">指定 Int32 值</param>
        /// <returns></returns>
        public static bool InEnum<T>(this int value) where T : struct
        {
            return Enum.IsDefined(typeof(T), value);
        }

        /// <summary>
        /// 获取一个非负整型的所有正约数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static IList<int> Divisors(this int number)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException("number", "必须为非负整型！");
            if (number == 0)
                return new List<int> { 1 };
            IList<int> divisors = new List<int>(2);
            divisors.Add(1);
            for (int j = 2; j < number; j++)
            {
                if (number % j == 0)
                    divisors.Add(j);
            }
            if (number > 1)
                divisors.Add(number);
            return divisors;
        }

        /// <summary>
        /// 将 int 型转换为 x 进制字符串
        /// </summary>
        /// <param name="number"></param>
        /// <param name="x">进制</param>
        /// <returns></returns>
        public static string To(this int number, int x)
        {
            return To((long)number, x);
        }

        /// <summary>
        /// 将 long 型转换为 x 进制字符串
        /// </summary>
        /// <param name="number"></param>
        /// <param name="x">进制</param>
        /// <returns></returns>
        public static string To(this long number, int x)
        {
            string result = string.Empty;
            long tmp = 0;
            while (number >= x)
            {
                tmp = number % x;
                result = (tmp > 9 ? ((char)(tmp + 55)).ToString() : tmp.ToString()) + result;
                number = number / x;
            }
            tmp = number;
            result = (tmp > 9 ? ((char)(tmp + 55)).ToString() : tmp.ToString()) + result;
            return result.ToString();
        }

        /// <summary>
        /// 将当前数值转换成百分比表示格式字符串
        /// 仅支持 short,int,long,float,double,decimal,sbyte 类型
        /// </summary>
        /// <typeparam name="T">仅支持 short,int,long,float,double,decimal,sbyte 类型</typeparam>
        /// <param name="number"></param>
        /// <param name="custDecimal">是否舍去小数部分</param>
        /// <returns></returns>
        public static string Percent<T>(this T number, bool custDecimal = true) where T : struct
        {
            Type t = typeof(T);
            if (custDecimal)
            {
                if (t == typeof(short))
                    return Math.Truncate((double)Math.Abs(Convert.ToInt16(number)) * 100) + "%";
                else if (t == typeof(int))
                    return Math.Truncate((double)Math.Abs(Convert.ToInt32(number)) * 100) + "%";
                else if (t == typeof(long))
                    return Math.Truncate((double)Math.Abs(Convert.ToInt64(number)) * 100) + "%";
                else if (t == typeof(float))
                    return Math.Truncate(Math.Abs(Convert.ToDouble(number)) * 100) + "%";
                else if (t == typeof(double))
                    return Math.Truncate(Math.Abs(Convert.ToDouble(number)) * 100) + "%";
                else if (t == typeof(decimal))
                    return Math.Truncate(Math.Abs(Convert.ToDecimal(number)) * 100) + "%";
                else if (t == typeof(sbyte))
                    return Math.Truncate((double)Math.Abs(Convert.ToSByte(number)) * 100) + "%";
                return "Parse Error";
            }
            else
            {
                if (t == typeof(short))
                    return Convert.ToInt16(number) * 100 + "%";
                else if (t == typeof(int))
                    return Convert.ToInt32(number) * 100 + "%";
                else if (t == typeof(long))
                    return Convert.ToInt64(number) * 100 + "%";
                else if (t == typeof(float))
                    return Convert.ToDouble(number) * 100 + "%";
                else if (t == typeof(double))
                    return Convert.ToDouble(number) * 100 + "%";
                else if (t == typeof(decimal))
                    return Convert.ToDecimal(number) * 100 + "%";
                else if (t == typeof(sbyte))
                    return Convert.ToSByte(number) * 100 + "%";
                return "Parse Error";
            }
        }

        /// <summary>
        /// 将当前数值转换成百分比表示格式字符串
        /// 仅支持 short,int,long,float,double,decimal,sbyte 类型
        /// </summary>
        /// <typeparam name="T">仅支持 short,int,long,float,double,decimal,sbyte 类型</typeparam>
        /// <param name="number"></param>
        /// <param name="format">将数字格式化显示</param>
        /// <returns></returns>
        public static string Percent<T>(this T number, string format) where T : struct
        {
            Type t = typeof(T);
            if (t == typeof(short))
                return ((double)Convert.ToInt16(number) * 100).ToString(format) + "%";
            else if (t == typeof(int))
                return ((double)Convert.ToInt32(number) * 100).ToString(format) + "%";
            else if (t == typeof(long))
                return ((double)Convert.ToInt64(number) * 100).ToString(format) + "%";
            else if (t == typeof(float))
                return (Convert.ToDouble(number) * 100).ToString(format) + "%";
            else if (t == typeof(double))
                return (Convert.ToDouble(number) * 100).ToString(format) + "%";
            else if (t == typeof(decimal))
                return (Convert.ToDecimal(number) * 100).ToString(format) + "%";
            else if (t == typeof(sbyte))
                return ((double)Convert.ToSByte(number) * 100).ToString(format) + "%";
            return "Parse Error";
        }

        /// <summary>
        /// 将当前数值转换成千分比表示格式字符串
        /// 仅支持 short,int,long,float,double,decimal,sbyte 类型
        /// </summary>
        /// <typeparam name="T">仅支持 short,int,long,float,double,decimal,sbyte 类型</typeparam>
        /// <param name="number"></param>
        /// <param name="custDecimal">是否舍去小数部分</param>
        /// <returns></returns>
        public static string Millesimal<T>(this T number, bool custDecimal = true) where T : struct
        {
            Type t = typeof(T);
            if (custDecimal)
            {
                if (t == typeof(short))
                    return Math.Truncate((double)Convert.ToInt16(number) * 100) + "‰";
                else if (t == typeof(int))
                    return Math.Truncate((double)Convert.ToInt32(number) * 100) + "‰";
                else if (t == typeof(long))
                    return Math.Truncate((double)Convert.ToInt64(number) * 100) + "‰";
                else if (t == typeof(float))
                    return Math.Truncate(Convert.ToDouble(number) * 100) + "‰";
                else if (t == typeof(double))
                    return Math.Truncate(Convert.ToDouble(number) * 100) + "‰";
                else if (t == typeof(decimal))
                    return Math.Truncate(Convert.ToDecimal(number) * 100) + "‰";
                else if (t == typeof(sbyte))
                    return Math.Truncate((double)Convert.ToSByte(number) * 100) + "‰";
                return "Parse Error";
            }
            else
            {
                if (t == typeof(short))
                    return Convert.ToInt16(number) * 100 + "‰";
                else if (t == typeof(int))
                    return Convert.ToInt32(number) * 100 + "‰";
                else if (t == typeof(long))
                    return Convert.ToInt64(number) * 100 + "‰";
                else if (t == typeof(float))
                    return Convert.ToDouble(number) * 100 + "‰";
                else if (t == typeof(double))
                    return Convert.ToDouble(number) * 100 + "‰";
                else if (t == typeof(decimal))
                    return Convert.ToDecimal(number) * 100 + "‰";
                else if (t == typeof(sbyte))
                    return Convert.ToSByte(number) * 100 + "‰";
                return "Parse Error";
            }
        }

        /// <summary>
        /// 将当前数值转换成千分比表示格式字符串
        /// 仅支持 short,int,long,float,double,decimal,sbyte 类型
        /// </summary>
        /// <typeparam name="T">仅支持 short,int,long,float,double,decimal,sbyte 类型</typeparam>
        /// <param name="number"></param>
        /// <param name="format">将数字格式化显示</param>
        /// <returns></returns>
        public static string Millesimal<T>(this T number, string format) where T : struct
        {
            Type t = typeof(T);
            if (t == typeof(short))
                return ((double)Convert.ToInt16(number) * 100).ToString(format) + "‰";
            else if (t == typeof(int))
                return ((double)Convert.ToInt32(number) * 100).ToString(format) + "‰";
            else if (t == typeof(long))
                return ((double)Convert.ToInt64(number) * 100).ToString(format) + "‰";
            else if (t == typeof(float))
                return (Convert.ToDouble(number) * 100).ToString(format) + "‰";
            else if (t == typeof(double))
                return (Convert.ToDouble(number) * 100).ToString(format) + "‰";
            else if (t == typeof(decimal))
                return (Convert.ToDecimal(number) * 100).ToString(format) + "‰";
            else if (t == typeof(sbyte))
                return ((double)Convert.ToSByte(number) * 100).ToString(format) + "‰";
            return "Parse Error";
        }

        /// <summary>
        /// 将数字转换成中文表示形式
        /// 仅支持 short,int,long,float,double,decimal,sbyte 类型
        /// </summary>
        /// <typeparam name="T">仅支持 short,int,long,float,double,decimal,sbyte 类型</typeparam>
        /// <param name="number"></param>
        /// <param name="currency">是否输出中文大写货币</param>
        /// <returns></returns>
        public static string ToCN<T>(this T number, bool currency = false) where T : struct
        {
            long num = 0;//整数部分
            string dec = null;//小数部分
            bool output0 = false;//输出0
            StringBuilder stringBuilder = new StringBuilder();
            Type type = typeof(T);
            if (type == typeof(short) || type == typeof(int) || type == typeof(long) || type == typeof(sbyte))
                num = Convert.ToInt64(number);
            else if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
            {
                num = (long)Math.Truncate(Convert.ToDouble(number));
                string decStr = number.ToString();
                if (decStr.IndexOf('.') >= 0)
                    dec = decStr.Substring(decStr.IndexOf('.') + 1);
            }
            else
                return "Parse Error!";
            if (num < 0)
                stringBuilder.Append("负");
            if (num > 1)
            {
                long l = num;
                for (int ii = 4; ii >= 0; ii--)
                {
                    long level = (long)Math.Pow(10000, ii);
                    if (num >= level)
                    {
                        l = num % level;
                        num /= level;
                        if (num > 19)
                        {
                            int j = 1000;
                            while (num % (j * 10) >= 1)
                            {
                                long tmp = num / j;
                                if (tmp != 0)
                                {
                                    stringBuilder.Append(_ToCNNumber(tmp, currency));
                                    if (j > 1)
                                        stringBuilder.Append(_ToCNNumber(j, currency));
                                    output0 = true;
                                }
                                else if (output0)
                                {
                                    stringBuilder.Append(_ToCNNumber(0L, currency));
                                    output0 = false;
                                }
                                if (j == 1)
                                    break;
                                num %= j;
                                j /= 10;
                            }
                        }
                        else if (num >= 10)
                        {
                            stringBuilder.Append(_ToCNNumber(10L, currency));
                            if (num % 10 > 0)
                            {
                                stringBuilder.Append(_ToCNNumber(num % 10, currency));
                                output0 = true;
                            }
                        }
                        else
                            stringBuilder.Append(_ToCNNumber(num, currency));
                        if (level > 1)
                            stringBuilder.Append(_ToCNNumber(level, currency));
                    }
                    num = l;
                }
            }
            else if (!currency)
                stringBuilder.Append(_ToCNNumber(num, currency));
            if (dec != null && Regex.IsMatch(dec, @"\d*[1-9]+\d*"))
            {
                #region 处理小数部分
                IList<char> list = dec.ToList();
                if (currency)
                {
                    if (stringBuilder.Length > 0)
                        stringBuilder.Append("圆");
                    if (output0)
                        stringBuilder.Append(_ToCNNumber(0L, currency));
                    int i = 0;
                    do
                    {
                        long dd = long.Parse(list[i].ToString());
                        if (dd > 0)
                        {
                            stringBuilder.Append(_ToCNNumber(dd, currency));
                            stringBuilder.Append(_Unit(i));
                            output0 = true;
                        }
                        else if (list.Count > i + 1 && output0)
                        {
                            stringBuilder.Append(_ToCNNumber(0L, currency));
                            output0 = false;
                        }
                        i++;
                    } while (list.Count > i);
                }
                else
                {
                    stringBuilder.Append("点");
                    foreach (char c in list)
                        stringBuilder.Append(_ToCNNumber(long.Parse(c.ToString()), currency));
                }
                #endregion
            }
            else if (currency)
                stringBuilder.Append("圆整");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 将 0-9，10，100，1000，10000，100000000等数字转换成中文
        /// </summary>
        /// <typeparam name="T">仅支持 short,int,long,float,double,decimal,sbyte 类型</typeparam>
        /// <param name="number"></param>
        /// <param name="upperorLower">true：中文大写；false：中文小写</param>
        /// <returns></returns>
        public static string ToCNNumber<T>(this T number, bool upperorLower = false) where T : struct
        {
            try
            {
                long i = Convert.ToInt64(number);
                if (i >= 100000000)
                    i = 100000000;
                else if (i >= 10000)
                    i = 10000;
                else if (i >= 1000)
                    i = 1000;
                else if (i >= 100)
                    i = 100;
                else if (i >= 10)
                    i = 10;
                else if (i < 0)
                    i = 0;
                return _ToCNNumber(i, upperorLower);
            }
            catch (Exception)
            {
                return "Parse Error!";
            }
        }

        private static string _Unit(int i)
        {
            string s = string.Empty;
            switch (i)
            {
                case 0:
                    s = "角";
                    break;
                case 1:
                    s = "分";
                    break;
                case 2:
                    s = "厘";
                    break;
                default:
                    break;
            }
            return s;
        }

        private static string _ToCNNumber(long i, bool upperorLower)
        {
            if (i > 100000000)
                i /= 100000000;
            string f = string.Empty;
            if (upperorLower)
            {
                switch (i)
                {
                    case 0:
                        f = "零";
                        break;
                    case 1:
                        f = "壹";
                        break;
                    case 2:
                        f = "贰";
                        break;
                    case 3:
                        f = "叁";
                        break;
                    case 4:
                        f = "肆";
                        break;
                    case 5:
                        f = "伍";
                        break;
                    case 6:
                        f = "陆";
                        break;
                    case 7:
                        f = "柒";
                        break;
                    case 8:
                        f = "捌";
                        break;
                    case 9:
                        f = "玖";
                        break;
                    case 10:
                        f = "拾";
                        break;
                    case 100:
                        f = "佰";
                        break;
                    case 1000:
                        f = "仟";
                        break;
                    case 10000:
                        f = "万";
                        break;
                    case 100000000:
                        f = "亿";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (i)
                {
                    case 0:
                        f = "〇";
                        break;
                    case 1:
                        f = "一";
                        break;
                    case 2:
                        f = "二";
                        break;
                    case 3:
                        f = "三";
                        break;
                    case 4:
                        f = "四";
                        break;
                    case 5:
                        f = "五";
                        break;
                    case 6:
                        f = "六";
                        break;
                    case 7:
                        f = "七";
                        break;
                    case 8:
                        f = "八";
                        break;
                    case 9:
                        f = "九";
                        break;
                    case 10:
                        f = "十";
                        break;
                    case 100:
                        f = "百";
                        break;
                    case 1000:
                        f = "千";
                        break;
                    case 10000:
                        f = "万";
                        break;
                    case 100000000:
                        f = "亿";
                        break;
                    default:
                        break;
                }
            }
            return f;
        }
    }
}