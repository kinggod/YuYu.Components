using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace YuYu.Components
{
    /// <summary>
    /// 字符串类型扩展方法
    /// </summary>
    public static class ExtendMethodsForString
    {
        #region Validate

        /// <summary>
        /// 是否为IPv4格式
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool IsIPv4(this string ipAddress)
        {
            if (ipAddress == null || ipAddress == string.Empty || ipAddress.Length < 7 || ipAddress.Length > 15)
                return false;
            return Regex.IsMatch(ipAddress, @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证字符串为Null或者Empty
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// 验证字符串不为Null或者Empty
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string input)
        {
            return !IsNullOrEmpty(input);
        }

        /// <summary>
        /// 验证字符串为Null或者Empty或者空白字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string input)
        {
            if (input == null)
                return true;
            return string.IsNullOrEmpty(input.Trim());
        }

        /// <summary>
        /// 验证字符串不为Null或者Empty或者空白字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNotNullOrWhiteSpace(this string input)
        {
            return !IsNullOrWhiteSpace(input);
        }

        /// <summary>
        /// 验证字符串是否符合规则
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="options">枚举值的一个按位组合，这些枚举值提供匹配选项。</param>
        /// <returns></returns>
        public static bool IsMatch(this string input, string pattern, RegexOptions options = RegexOptions.None)
        {
            return Regex.IsMatch(input, pattern, options);
        }

        #endregion

        #region Format

        /// <summary>
        /// 将指定字符串中的格式项替换为指定数组中相应对象的字符串表示形式
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        /// <exception cref="System.ArgumentNullException">format 或 args 为 null</exception>
        /// <exception cref="System.FormatException">format 无效。- 或 -格式项的索引小于零或大于等于 args 数组的长度</exception>
        /// <returns>字符串的副本，其中的格式项已替换为 args 中相应对象的字符串表示形式</returns>
        public static string Format(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// 将指定字符串中的一个或多个格式项替换为指定对象的字符串表示形式
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg0">要设置格式的对象</param>
        /// <exception cref="System.ArgumentNullException">format 或 args 为 null</exception>
        /// <exception cref="System.FormatException">format 无效。- 或 -格式项的索引小于零或大于等于 args 数组的长度</exception>
        /// <returns>字符串的副本，其中的格式项已替换为 args 中相应对象的字符串表示形式</returns>
        public static string Format(this string format, object arg0)
        {
            return string.Format(format, arg0);
        }

        /// <summary>
        /// 将指定字符串中的格式项替换为两个指定对象的字符串表示形式
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg0">要设置格式的第一个对象</param>
        /// <param name="arg1">要设置格式的第二个对象</param>
        /// <exception cref="System.ArgumentNullException">format 或 args 为 null</exception>
        /// <exception cref="System.FormatException">format 无效。- 或 -格式项的索引小于零或大于等于 args 数组的长度</exception>
        /// <returns>字符串的副本，其中的格式项已替换为 args 中相应对象的字符串表示形式</returns>
        public static string Format(this string format, object arg0, object arg1)
        {
            return string.Format(format, arg0, arg1);
        }

        /// <summary>
        /// 将指定字符串中的格式项替换为三个指定对象的字符串表示形式
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg0">要设置格式的第一个对象</param>
        /// <param name="arg1">要设置格式的第二个对象</param>
        /// <param name="arg2">要设置格式的第三个对象</param>
        /// <exception cref="System.ArgumentNullException">format 或 args 为 null</exception>
        /// <exception cref="System.FormatException">format 无效。- 或 -格式项的索引小于零或大于等于 args 数组的长度</exception>
        /// <returns>字符串的副本，其中的格式项已替换为 args 中相应对象的字符串表示形式</returns>
        public static string Format(this string format, object arg0, object arg1, object arg2)
        {
            return string.Format(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// 将指定字符串中的格式项替换为指定数组中相应对象的字符串表示形式。指定的参数提供区域性特定的格式设置信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        /// <exception cref="System.ArgumentNullException">format 或 args 为 null</exception>
        /// <exception cref="System.FormatException">format 无效。- 或 -格式项的索引小于零或大于等于 args 数组的长度</exception>
        /// <returns>字符串副本，其中的格式项已替换为 args 中相应对象的字符串表示形式</returns>
        public static string Format(this string format, IFormatProvider provider, params object[] args)
        {
            return string.Format(provider, format, args);
        }

        #endregion

        #region Substring

        /// <summary>
        /// 从当前字符串中取 count 长度的随机字符并组成新字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string GetRandom(this string input, int count)
        {
            if (input.IsNullOrEmpty())
                throw new ArgumentNullException("input","字符串不可为空！");
            if (count <= 0)
                throw new ArgumentException("The value of parameter “count” must greater than 0!");
            StringBuilder builder = new StringBuilder(count);
            for (int i = 0; i < count; i++)
                builder.Append(input[HelperBase.Random.Next(input.Length)]);
            return builder.ToString();
        }

        /// <summary>
        /// 截取当前字符串的指定长度，并附加一个指定的结尾
        /// 如果指定长度大于当前字符串长度，则返回当前字符串本身
        /// 如果该字符串为null，则返回0长度字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startIndex">截取开始位置</param>
        /// <param name="length">截取后的字符串长度</param>
        /// <param name="endWith">为结果字符串附加一个结尾</param>
        /// <returns></returns>
        public static string Substring(this string input, int startIndex, int length, string endWith)
        {
            if (input == null)
                return string.Empty;
            if (input.Length <= length)
                return input;
            return input.Substring(startIndex, length) + endWith ?? string.Empty;
        }

        #endregion

        #region Convert

        /// <summary>
        /// 将字符串转换成等效的枚举类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string input) where T : struct
        {
            return ToEnum<T>(input, false);
        }

        /// <summary>
        /// 将字符串转换成等效的枚举类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static T ToEnum<T>(this string input, bool ignoreCase = true) where T : struct
        {
            Type type = typeof(T);
            if (type.IsEnum)
                return (T)Enum.Parse(type, input, ignoreCase);
            throw new Exception("T 必须是枚举类型！");
        }

        /// <summary>
        /// 将当前字符串转换成 Short? 型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static short? ToShort(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            try
            {
                return short.Parse(input);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 将当前字符串转换成 Int32? 型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int? ToInt(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            try
            {
                return int.Parse(input);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 将当前字符串转换成 Int64? 型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static long? ToLong(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            try
            {
                return long.Parse(input);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 将当前字符串转换成 Boolean? 型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool? ToBool(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            try
            {
                return bool.Parse(input);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 将当前字符串转换成 Guid? 型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Guid? ToGuid(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            try
            {
                return Guid.Parse(input);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 将当前字符串转换成 Double? 型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double? ToDouble(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            try
            {
                return double.Parse(input);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 将当前字符串转换成 Float? 型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static float? ToFloat(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            try
            {
                return float.Parse(input);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 将当前字符串转换成 Decimal? 型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static decimal? ToDecimal(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            try
            {
                return decimal.Parse(input);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 将当前字符串转换成 Byte? 型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte? ToByte(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            try
            {
                return byte.Parse(input);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 将当前字符串转换成 DateTime? 型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            try
            {
                return DateTime.Parse(input);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 将 x 进制数字字符串转换为 long 型
        /// </summary>
        /// <param name="input"></param>
        /// <param name="x">进制</param>
        /// <returns></returns>
        public static long ToLong(this string input, int x)
        {
            long result = 0;
            IList<char> array = input.ToUpper().ToArray().Reverse().ToList();
            for (int i = 0; i < array.Count; i++)
            {
                int num = 0;
                try
                {
                    num = Convert.ToInt32(array[i].ToString());
                }
                catch (Exception)
                {
                    num = (int)array[i];
                }
                if (num > x)
                    num -= 55;
                result += num * (long)Math.Pow(x, i);
            }
            return result;
        }

        #endregion

        #region Replace

        /// <summary>
        /// 在指定的输入字符串内，使用 System.Text.RegularExpressions.MatchEvaluator 委托返回的字符串替换与指定正则表达式匹配的所有字符串。指定的选项将修改匹配操作。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="evaluator"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string RegexReplace(this string input, string pattern, MatchEvaluator evaluator, RegexOptions options = RegexOptions.None)
        {
            return Regex.Replace(input, pattern, evaluator, options);
        }

        /// <summary>
        /// 在指定的输入字符串内，使用指定的替换字符串替换与指定正则表达式匹配的所有字符串。指定的选项将修改匹配操作。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string RegexReplace(this string input, string pattern, string replacement, RegexOptions options = RegexOptions.None)
        {
            return Regex.Replace(input, pattern, replacement, options);
        }

        /// <summary>
        /// 将HTML文档(部分文档)转换为HTML文本
        /// </summary>
        /// <param name="input">HTML文档(部分文档)</param>
        /// <returns></returns>
        public static string ToHTMLText(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            return input.Replace("&", "&amp").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        /// <summary>
        /// 获取HTML文档(部分文档)中的文本内容
        /// </summary>
        /// <param name="input">HTML文档(部分文档)</param>
        /// <returns></returns>
        public static string ClearHTML(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            input = Regex.Replace(input, @"(<script[^>]*>[.\n]*?</script>)+?", string.Empty, RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"(<[^>]*>)+?", string.Empty, RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"([\r\n])[\s]+", string.Empty, RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"-->", string.Empty, RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"<!--.*", string.Empty, RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&#(\d+);", string.Empty, RegexOptions.IgnoreCase);
            input.Replace("<", string.Empty);
            input.Replace(">", string.Empty);
            input.Replace(Environment.NewLine, string.Empty);
            return input;
        }

        /// <summary>
        /// 去除HTML文档(部分文档)中的脚本
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ClearScript(this string input)
        {
            return Regex.Replace(input, @"(<script[^>]*>[.\n]*?</script>)+?", string.Empty, RegexOptions.IgnoreCase);
        }

        #endregion

        #region DefaultValue

        /// <summary>
        /// 如果为 null 或空字符串或空白字符串时显示默认值
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string DefaultIfNullOrEmpty(this string input, string defaultValue)
        {
            if (string.IsNullOrEmpty(input))
                return defaultValue;
            else
                return input;
        }

        #endregion

        #region Encryption

        /// <summary>
        /// 验证字符串与加密后字符串是否一致
        /// </summary>
        /// <param name="input">原字符串</param>
        /// <param name="encrypted">加密后字符串</param>
        /// <param name="cipherCode">密钥</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static bool Verify(this string input, string encrypted, string cipherCode, Encoding encoding)
        {
            return Verify(Encrypt(input, cipherCode, encoding), encrypted);
        }

        /// <summary>
        /// 验证两个加密后字符串是否一致
        /// </summary>
        /// <param name="encrypted1">加密字符串1</param>
        /// <param name="encrypted2">加密字符串2</param>
        /// <returns></returns>
        public static bool Verify(this string encrypted1, string encrypted2)
        {
            if ((string.IsNullOrEmpty(encrypted1) || string.IsNullOrEmpty(encrypted2)) || (encrypted1.Length != encrypted2.Length))
                return false;
            bool flag = true;
            for (int i = 0; i < encrypted1.Length; i++)
                if (((i + 1) % 4) != 0)
                {
                    flag = flag && encrypted1[i].Equals(encrypted2[i]);
                    if (!flag)
                        return flag;
                }
            return flag;
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cipherCode">密钥</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string Encrypt(this string input, string cipherCode, Encoding encoding)
        {
            int num;
            input = input + cipherCode;
            string str = "0a12b34c56d78e9f";
            MD5 md = MD5.Create();
            byte[] buffer = md.ComputeHash(encoding.GetBytes(input));
            md.Clear();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (num = 0; num < buffer.Length; num++)
                builder.Append(buffer[num].ToString("x2"));
            string str2 = builder.ToString();
            builder.Clear();
            string str3 = string.Empty;
            for (num = 0; num < str2.Length; num += 4)
            {
                if (((num / 4) % 2) == 0)
                    str3 = str3 + str2.Substring(num, 3) + str[random.Next(0x10)];
                else
                    str3 = str2.Substring(num, 3) + str[random.Next(0x10)] + str3;
            }
            return str3;
        }

        private static char[] _Table = new char[64] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/' };

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string Base64Encrypt(this string input, Encoding encoding)
        {
            byte[] source1, source2, buffer;
            int length1, length2, blockCount, paddingCount;
            byte b1, b2, b3, temp1, temp2, temp3, temp4, temp5;
            char[] result;
            source1 = encoding.GetBytes(input);
            length1 = source1.Length;
            if ((length1 % 3) == 0)
            {
                paddingCount = 0;
                blockCount = length1 / 3;
            }
            else
            {
                paddingCount = 3 - (length1 % 3);
                blockCount = (length1 + paddingCount) / 3;
            }
            length2 = length1 + paddingCount;
            source2 = new byte[length2];
            for (int i = 0; i < length2; i++)
            {
                if (i < length1)
                    source2[i] = source1[i];
                else
                    source2[i] = 0;
            }
            buffer = new byte[blockCount * 4];
            result = new char[blockCount * 4];
            for (int x = 0; x < blockCount; x++)
            {
                b1 = source2[x * 3];
                b2 = source2[x * 3 + 1];
                b3 = source2[x * 3 + 2];
                temp2 = (byte)((b1 & 252) >> 2);
                temp1 = (byte)((b1 & 3) << 4);
                temp3 = (byte)((b2 & 240) >> 4);
                temp3 += temp1;
                temp1 = (byte)((b2 & 15) << 2);
                temp4 = (byte)((b3 & 192) >> 6);
                temp4 += temp1;
                temp5 = (byte)(b3 & 63);
                buffer[x * 4] = temp2;
                buffer[x * 4 + 1] = temp3;
                buffer[x * 4 + 2] = temp4;
                buffer[x * 4 + 3] = temp5;
            }
            for (int x = 0; x < blockCount * 4; x++)
            {
                byte b = buffer[x];
                if ((b >= 0) && (b <= 63))
                    result[x] = _Table[(int)b];
                else
                    result[x] = ' ';
            }
            switch (paddingCount)
            {
                case 0:
                    break;
                case 1:
                    result[blockCount * 4 - 1] = '=';
                    break;
                case 2:
                    result[blockCount * 4 - 1] = '=';
                    result[blockCount * 4 - 2] = '=';
                    break;
                default:
                    break;
            }
            return new string(result);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encryptedInput"></param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string Base64Decrypt(this string encryptedInput, Encoding encoding)
        {
            char[] source;
            int temp, length1, length2, ength3, blockCount, paddingCount;
            byte b1, b2, b3, b4, temp1, temp2, temp3, temp4;
            byte[] buffer1, buffer2;
            temp = 0;
            source = encryptedInput.ToCharArray();
            length1 = source.Length;
            for (int i = 0; i < 2; i++)
            {
                if (encryptedInput[length1 - i - 1] == '=')
                    temp++;
            }
            paddingCount = temp;
            blockCount = length1 / 4;
            length2 = blockCount * 3;
            buffer1 = new byte[length1];
            buffer2 = new byte[length2];
            for (int i = 0; i < length1; i++)
            {
                char c = source[i];
                if (c == '=')
                    buffer1[i] = 0;
                else
                {
                    bool find = false;
                    for (int j = 0; j < 64; j++)
                    {
                        if (_Table[j] == c)
                        {
                            buffer1[i] = (byte)j;
                            find = true;
                            break;
                        }
                    }
                    if (!find)
                        buffer1[i] = 0;
                }
            }
            for (int i = 0; i < blockCount; i++)
            {
                temp1 = buffer1[i * 4];
                temp2 = buffer1[i * 4 + 1];
                temp3 = buffer1[i * 4 + 2];
                temp4 = buffer1[i * 4 + 3];
                b1 = (byte)(temp1 << 2);
                b2 = (byte)((temp2 & 48) >> 4);
                b2 += b1;
                b1 = (byte)((temp2 & 15) << 4);
                b3 = (byte)((temp3 & 60) >> 2);
                b3 += b1;
                b1 = (byte)((temp3 & 3) << 6);
                b4 = temp4;
                b4 += b1;
                buffer2[i * 3] = b2;
                buffer2[i * 3 + 1] = b3;
                buffer2[i * 3 + 2] = b4;
            }
            ength3 = length2 - paddingCount;
            byte[] result = new byte[ength3];
            for (int i = 0; i < ength3; i++)
                result[i] = buffer2[i];
            return encoding.GetString(result);
        }

        #endregion

        #region Simplified/Complex

        private static string _S = "皑蔼碍爱袄奥坝罢摆败颁办绊帮绑镑谤剥饱宝报鲍辈贝钡狈备惫绷笔毕毙币闭边编贬变辩辫标鳖别瘪濒滨宾摈饼并拨钵铂驳卜补财参蚕残惭惨灿苍舱仓沧厕侧册测层诧搀掺蝉馋谗缠铲产阐颤场尝长偿肠厂畅钞车彻尘陈衬撑称惩诚骋痴迟驰耻齿炽冲虫宠畴踌筹绸丑橱厨锄雏础储触处传疮闯创锤纯绰辞词赐聪葱囱从丛凑蹿窜错达带贷担单郸掸胆惮诞弹当挡党荡档捣岛祷导盗灯邓敌涤递缔颠点垫电淀钓调谍叠钉顶锭订丢东动栋冻斗犊独读赌镀锻断缎兑队对吨顿钝夺堕鹅额讹恶饿儿尔饵贰发罚阀珐矾钒烦范贩饭访纺飞诽废费纷坟奋愤粪丰枫锋风疯冯缝讽凤肤辐抚辅赋复负讣妇缚该钙盖干赶秆赣冈刚钢纲岗镐搁鸽阁铬个给龚宫巩贡钩沟构购够蛊顾剐挂关观馆惯贯广规硅归龟闺轨诡柜贵刽辊滚锅国过骇韩汉号阂鹤贺横轰鸿红后壶护沪户哗华画划话怀坏欢环还缓换唤痪焕涣黄谎挥辉毁贿秽会烩汇讳诲绘荤浑伙获货祸击机积饥迹讥鸡绩缉极辑级挤几蓟剂济计记际继纪夹荚颊贾钾价驾歼监坚笺间艰缄茧检碱硷拣捡简俭减荐槛鉴践贱见键舰剑饯渐溅涧将浆蒋桨奖讲酱胶浇骄娇搅铰矫侥脚饺缴绞轿较阶节杰洁结诫届紧锦仅谨进晋烬尽劲荆茎鲸惊经颈静镜径痉竞净纠厩旧驹举据锯惧剧鹃绢觉决诀绝钧军骏开凯颗壳课垦恳抠库裤夸块侩宽矿旷况亏岿窥馈溃扩阔蜡腊莱来赖蓝栏拦篮阑兰澜谰揽览懒缆烂滥捞劳涝乐镭垒类泪篱离里鲤礼丽厉励砾历沥隶俩联莲连镰怜涟帘敛脸链恋炼练粮凉两辆谅疗辽镣猎临邻鳞凛赁龄铃凌灵岭领馏刘龙聋咙笼垄拢陇楼娄搂篓芦卢颅庐炉掳卤虏鲁赂禄录陆驴吕铝侣屡缕虑滤绿峦挛孪滦乱抡轮伦仑沦纶论萝罗逻锣箩骡骆络妈玛码蚂马骂吗买麦卖迈脉瞒馒蛮满谩猫锚铆贸么霉没镁门闷们锰梦谜弥觅幂绵缅庙灭悯闽鸣铭谬谋亩钠纳难挠脑恼闹馁内拟腻撵捻酿鸟聂啮镊镍柠狞宁拧泞钮纽脓浓农疟诺欧鸥殴呕沤盘庞抛赔喷鹏骗飘频贫苹凭评泼颇扑铺仆朴谱栖凄脐齐骑岂启气弃讫牵扦钎铅迁签谦钱钳潜浅谴堑枪呛墙蔷强抢锹桥乔侨翘窍窃钦亲寝轻氢倾顷请庆琼穷趋区躯驱龋颧权劝却鹊确让饶扰绕热韧认纫荣绒软锐闰润洒萨鳃赛叁伞丧骚扫涩杀纱筛晒删闪陕赡缮伤赏烧绍赊摄慑设绅审婶肾渗声绳胜圣师狮湿诗尸时蚀实识驶势适释饰视试寿兽枢输书赎属术树竖数帅双谁税顺说硕烁丝饲耸怂颂讼诵擞苏诉肃虽随绥岁孙损笋缩琐锁獭挞态摊贪瘫滩坛谭谈叹汤烫涛绦讨腾誊锑题体屉条贴铁厅听烃铜统头秃图涂团颓蜕脱鸵驮驼椭洼袜弯湾顽万网韦违围为潍维苇伟伪纬谓卫温闻纹稳问瓮挝蜗涡窝卧呜钨乌污诬无芜吴坞雾务误锡牺袭习铣戏细虾辖峡侠狭厦吓锨鲜纤咸贤衔闲显险现献县馅羡宪线厢镶乡详响项萧嚣销晓啸蝎协挟携胁谐写泻谢锌衅兴汹锈绣虚嘘须许叙绪续轩悬选癣绚学勋询寻驯训讯逊压鸦鸭哑亚讶阉烟盐严颜阎艳厌砚彦谚验鸯杨扬疡阳痒养样瑶摇尧遥窑谣药爷页业叶医铱颐遗仪蚁艺亿忆义诣议谊译异绎荫阴银饮隐樱婴鹰应缨莹萤营荧蝇赢颖哟拥佣痈踊咏涌优忧邮铀犹诱舆鱼渔娱与屿语吁御狱誉预驭鸳渊辕园员圆缘远愿约跃钥岳粤悦阅云郧匀陨运蕴酝晕韵杂灾载攒暂赞赃脏凿枣责择则泽贼赠扎札轧铡闸栅诈斋债毡盏斩辗崭栈战绽张涨帐账胀赵蛰辙锗这贞针侦诊镇阵挣睁狰争帧郑证织职执纸挚掷帜质滞钟终种肿众诌轴皱昼骤猪诸诛烛瞩嘱贮铸筑驻专砖转赚桩庄装妆壮状锥赘坠缀谆着浊兹资渍踪综总纵邹诅组钻亘鼗芈啬厍厣厮靥赝匦匮赜刭刿剀伛伥伧伫侪侬俦俨俪俣偾偬偻傥傧傩佥籴黉冁凫兖衮亵脔禀冢讠讦讧讪讴讵讷诂诃诋诏诎诒诓诔诖诘诙诜诟诠诤诨诩诮诰诳诶诹诼诿谀谂谄谇谌谏谑谒谔谕谖谙谛谘谝谟谠谡谥谧谪谫谮谯谲谳谵谶卺陉陧邝邬邺郏郐郄郓郦刍奂劢勐凼巯垩圹坜坂垅垆垭垲垧垴埘埚埙埯埝塬艹芗苈苋苌苁苎茏苘茑茔茕荛荜荞荟荠荦荥荨荩荬荪荭荮莳莴莜莅莶莸莺莼萦蒇蒉蒌蓦蓠蓥蓣蔹蔺蕲薮藓藁奁尴扪抟挢掴掼揸揿摅撄撷撸撺叽呒呓呖呗呙吣咔咛咝咴哒哓哔哕哌哙哜咤哝唛唠唢唣啧啭唿喽喾嗫嗬嗳辔嘤噜噼嚯囵帏帱帻帼幞岖岘岙岚岽峄峤峥崂崃嵘嵛嵝嵴巅徕犭犷犸狍狯狲猃猡猕猬饧饨饩饪饫饬饴饷饽馀馄馇馊馍馐馑馓馔馕庑赓廪忏怃怄忾怅怆怿恸恹恻恺恽悭惬愠愦憷懔闩闫闱闳闵闶闼闾阃阄阆阈阊阋阌阍阏阒阕阖阗阙阚丬沣沩泷泸泺泾浃浈浍浏浒浔涞涠渎渑渖渌溆滟滠滢滗潆潇漤潋潴濑灏骞迩迳逦屦弪妩妪妫姗娅娆娈娲娴婵媪嫒嫔嫱嬷驵驷驸驺驿驽骀骁骅骈骊骐骒骓骖骘骛骜骝骟骠骢骣骥骧纟纡纣纥纨纩纭纰纾绀绁绂绉绋绌绐绔绗绛绠绡绨绫绮绯绱绲缍绶绺绻绾缁缂缃缇缈缋缌缏缑缒缗缙缜缛缟缡缢缣缤缥缦缧缪缫缬缭缯缰缱缲缳缵幺玑玮珏珑珉顼玺珲琏瑷璎璇瓒韪韫韬杩枥枧枨枞枭栉栊栌栀栎柽桠桡桢桤桦桧栾桊棂椟椠椤椁榄榇榈榉槟槠樯橥橹橼檐檩殁殇殒殓殚殡轫轭轱轲轳轵轶轸轷轹轺轼轾辁辂辄辇辋辍辎辏辘辚戋戗戬瓯昙晔晖暧贲贳贶贻贽赀赅赆赈赉赇赍赕赙觇觊觋觌觎觏觐觑犟牦毵氇氩氲牍肷胧胨胪胫脍脶腌腼腽腭膑欤飑飒飓飕飙飚毂齑斓炀炜炖炝烨焖煳煅煺熘焘祢祯禅怼悫愍懑戆沓泶矶砀砗砜砺砻硖硗碛碜碹磙龛眍眦睐睑畲罴羁钆钇钋钊钌钍钏钐钔钗钕钚钛钣钤钫钪钭钬钯钰钲钴钶钷钸钹钺钼钽钿铄铈铉铊铋铌铍铎铐铑铒铕铖铗铙铘铛铞铟铠铢铤铥铧铨铪铩铫铮铯铳铴铵铷铹铼铽铿锃锂锆锇锉锊锍锎锏锒锓锔锕锖锘锛锝锞锟锢锪锫锩锬锱锲锴锶锷锸锼锾锿镂锵镄镅镆镉镌镎镏镒镓镔镖镗镘镙镛镞镟镝镡镢镤镥镦镧镨镩镪镫镬镯镱镲镳锺稆穑鸠鸢鸨鸩鸪鸫鸬鸲鸱鸶鸸鸷鸹鸺鸾鹁鹂鹄鹆鹇鹈鹉鹋鹌鹎鹑鹕鹗鹚鹛鹜鹞鹣鹦鹧鹨鹩鹪鹫鹬鹱鹭鹳疖疠疬疴疱痖痨痫瘅瘗瘘瘿瘾癞癫癯窦窭裆裢裣裥褛褴襁皲耢耧聍聩顸颀颃颉颌颍颏颔颚颛颞颟颡颢颥颦虬虮虿蚬蚝蛎蛏蛱蛲蛳蛴蝈蝾蝼螨罂笃笕笾筚筝箦箧箨箪箫篑簖籁舣舻袅羟籼粝粜糁糇絷麸趱酰酽酾鹾趸跄跖跞跷跸跹跻踬踯蹑蹒蹰躏躜觞觯靓雳霁霭龀龃龅龆龇龈龉龊龌黾鼋鼍隽雠銮錾鱿鲂鲅鲆鲇鲈稣鲋鲎鲐鲑鲒鲔鲕鲚鲛鲞鲟鲠鲡鲢鲣鲥鲦鲧鲨鲩鲫鲭鲮鲰鲱鲲鲳鲴鲵鲶鲷鲺鲻鲼鲽鳄鳅鳆鳇鳊鳋鳌鳍鳎鳏鳐鳓鳔鳕鳗鳘鳙鳜鳝鳟鳢鞑鞒鞯鞴鹘髅髋髌魇魉飨餍鬓黩黪鼹齄签叹原啰里呼于汇搜注复";
        private static string _C = "皚藹礙愛襖奧壩罷擺敗頒辦絆幫綁鎊謗剝飽寶報鮑輩貝鋇狽備憊繃筆畢斃幣閉邊編貶變辯辮標鱉別癟瀕濱賓擯餅並撥缽鉑駁蔔補財參蠶殘慚慘燦蒼艙倉滄廁側冊測層詫攙摻蟬饞讒纏鏟產闡顫場嘗長償腸廠暢鈔車徹塵陳襯撐稱懲誠騁癡遲馳恥齒熾沖蟲寵疇躊籌綢醜櫥廚鋤雛礎儲觸處傳瘡闖創錘純綽辭詞賜聰蔥囪從叢湊躥竄錯達帶貸擔單鄲撣膽憚誕彈當擋黨蕩檔搗島禱導盜燈鄧敵滌遞締顛點墊電澱釣調諜疊釘頂錠訂丟東動棟凍鬥犢獨讀賭鍍鍛斷緞兌隊對噸頓鈍奪墮鵝額訛惡餓兒爾餌貳發罰閥琺礬釩煩範販飯訪紡飛誹廢費紛墳奮憤糞豐楓鋒風瘋馮縫諷鳳膚輻撫輔賦複負訃婦縛該鈣蓋幹趕稈贛岡剛鋼綱崗鎬擱鴿閣鉻個給龔宮鞏貢鉤溝構購夠蠱顧剮掛關觀館慣貫廣規矽歸龜閨軌詭櫃貴劊輥滾鍋國過駭韓漢號閡鶴賀橫轟鴻紅後壺護滬戶嘩華畫劃話懷壞歡環還緩換喚瘓煥渙黃謊揮輝毀賄穢會燴匯諱誨繪葷渾夥獲貨禍擊機積饑跡譏雞績緝極輯級擠幾薊劑濟計記際繼紀夾莢頰賈鉀價駕殲監堅箋間艱緘繭檢堿鹼揀撿簡儉減薦檻鑒踐賤見鍵艦劍餞漸濺澗將漿蔣槳獎講醬膠澆驕嬌攪鉸矯僥腳餃繳絞轎較階節傑潔結誡屆緊錦僅謹進晉燼盡勁荊莖鯨驚經頸靜鏡徑痙競淨糾廄舊駒舉據鋸懼劇鵑絹覺決訣絕鈞軍駿開凱顆殼課墾懇摳庫褲誇塊儈寬礦曠況虧巋窺饋潰擴闊蠟臘萊來賴藍欄攔籃闌蘭瀾讕攬覽懶纜爛濫撈勞澇樂鐳壘類淚籬離裏鯉禮麗厲勵礫曆瀝隸倆聯蓮連鐮憐漣簾斂臉鏈戀煉練糧涼兩輛諒療遼鐐獵臨鄰鱗凜賃齡鈴淩靈嶺領餾劉龍聾嚨籠壟攏隴樓婁摟簍蘆盧顱廬爐擄鹵虜魯賂祿錄陸驢呂鋁侶屢縷慮濾綠巒攣孿灤亂掄輪倫侖淪綸論蘿羅邏鑼籮騾駱絡媽瑪碼螞馬罵嗎買麥賣邁脈瞞饅蠻滿謾貓錨鉚貿麼黴沒鎂門悶們錳夢謎彌覓冪綿緬廟滅憫閩鳴銘謬謀畝鈉納難撓腦惱鬧餒內擬膩攆撚釀鳥聶齧鑷鎳檸獰甯擰濘鈕紐膿濃農瘧諾歐鷗毆嘔漚盤龐拋賠噴鵬騙飄頻貧蘋憑評潑頗撲鋪僕樸譜棲淒臍齊騎豈啟氣棄訖牽扡釺鉛遷簽謙錢鉗潛淺譴塹槍嗆牆薔強搶鍬橋喬僑翹竅竊欽親寢輕氫傾頃請慶瓊窮趨區軀驅齲顴權勸卻鵲確讓饒擾繞熱韌認紉榮絨軟銳閏潤灑薩鰓賽三傘喪騷掃澀殺紗篩曬刪閃陝贍繕傷賞燒紹賒攝懾設紳審嬸腎滲聲繩勝聖師獅濕詩屍時蝕實識駛勢適釋飾視試壽獸樞輸書贖屬術樹豎數帥雙誰稅順說碩爍絲飼聳慫頌訟誦擻蘇訴肅雖隨綏歲孫損筍縮瑣鎖獺撻態攤貪癱灘壇譚談歎湯燙濤絛討騰謄銻題體屜條貼鐵廳聽烴銅統頭禿圖塗團頹蛻脫鴕馱駝橢窪襪彎灣頑萬網韋違圍為濰維葦偉偽緯謂衛溫聞紋穩問甕撾蝸渦窩臥嗚鎢烏汙誣無蕪吳塢霧務誤錫犧襲習銑戲細蝦轄峽俠狹廈嚇鍁鮮纖鹹賢銜閑顯險現獻縣餡羨憲線廂鑲鄉詳響項蕭囂銷曉嘯蠍協挾攜脅諧寫瀉謝鋅釁興洶鏽繡虛噓須許敘緒續軒懸選癬絢學勳詢尋馴訓訊遜壓鴉鴨啞亞訝閹煙鹽嚴顏閻豔厭硯彥諺驗鴦楊揚瘍陽癢養樣瑤搖堯遙窯謠藥爺頁業葉醫銥頤遺儀蟻藝億憶義詣議誼譯異繹蔭陰銀飲隱櫻嬰鷹應纓瑩螢營熒蠅贏穎喲擁傭癰踴詠湧優憂郵鈾猶誘輿魚漁娛與嶼語籲禦獄譽預馭鴛淵轅園員圓緣遠願約躍鑰嶽粵悅閱雲鄖勻隕運蘊醞暈韻雜災載攢暫贊贓髒鑿棗責擇則澤賊贈紮劄軋鍘閘柵詐齋債氈盞斬輾嶄棧戰綻張漲帳賬脹趙蟄轍鍺這貞針偵診鎮陣掙睜猙爭幀鄭證織職執紙摯擲幟質滯鐘終種腫眾謅軸皺晝驟豬諸誅燭矚囑貯鑄築駐專磚轉賺樁莊裝妝壯狀錐贅墜綴諄著濁茲資漬蹤綜總縱鄒詛組鑽亙鞀羋嗇厙厴廝靨贗匭匱賾剄劌剴傴倀傖佇儕儂儔儼儷俁僨傯僂儻儐儺僉糴黌囅鳧兗袞褻臠稟塚訁訐訌訕謳詎訥詁訶詆詔詘詒誆誄詿詰詼詵詬詮諍諢詡誚誥誑誒諏諑諉諛諗諂誶諶諫謔謁諤諭諼諳諦諮諞謨讜謖諡謐謫譾譖譙譎讞譫讖巹陘隉鄺鄔鄴郟鄶郤鄆酈芻奐勱猛氹巰堊壙壢阪壟壚埡塏坰堖塒堝塤垵墊原艸薌藶莧萇蓯苧蘢檾蔦塋煢蕘蓽蕎薈薺犖滎蕁藎蕒蓀葒葤蒔萵蓧蒞薟蕕鶯蓴縈蕆蕢蔞驀蘺鎣蕷蘞藺蘄藪蘚槁奩尷捫摶撟摑摜摣撳攄攖擷擼攛嘰嘸囈嚦唄咼唚哢嚀噝噅噠嘵嗶噦呱噲嚌吒噥嘜嘮嗩唕嘖囀呼嘍嚳囁呵噯轡嚶嚕劈謔圇幃幬幘幗襆嶇峴嶴嵐崠嶧嶠崢嶗崍嶸崳嶁脊巔徠犬獷獁麅獪猻獫玀獼蝟餳飩餼飪飫飭飴餉餑餘餛餷餿饃饈饉饊饌饢廡賡廩懺憮慪愾悵愴懌慟懨惻愷惲慳愜慍憒怵懍閂閆闈閎閔閌闥閭閫鬮閬閾閶鬩閿閽閼闃闋闔闐闕闞爿灃溈瀧瀘濼涇浹湞澮瀏滸潯淶潿瀆澠瀋淥漵灩灄瀅潷瀠瀟濫瀲瀦瀨灝騫邇逕邐屨弳嫵嫗媯姍婭嬈孌媧嫻嬋媼嬡嬪嬙嬤駔駟駙騶驛駑駘驍驊駢驪騏騍騅驂騭騖驁騮騸驃驄驏驥驤糸紆紂紇紈纊紜紕紓紺絏紱縐紼絀紿絝絎絳綆綃綈綾綺緋緔緄綞綬綹綣綰緇緙緗緹緲繢緦緶緱縋緡縉縝縟縞縭縊縑繽縹縵縲繆繅纈繚繒韁繾繰繯纘么璣瑋玨瓏瑉頊璽琿璉璦瓔璿瓚韙韞韜榪櫪梘棖樅梟櫛櫳櫨梔櫟檉椏橈楨榿樺檜欒棬欞櫝槧欏槨欖櫬櫚櫸檳櫧檣櫫櫓櫞簷檁歿殤殞殮殫殯軔軛軲軻轤軹軼軫軤轢軺軾輊輇輅輒輦輞輟輜輳轆轔戔戧戩甌曇曄暉曖賁貰貺貽贄貲賅贐賑賚賕齎賧賻覘覬覡覿覦覯覲覷強犛毿氌氬氳牘膁朧腖臚脛膾腡醃靦膃齶臏歟颮颯颶颼飆飆轂齏斕煬煒燉熗燁燜糊煆退溜燾禰禎禪懟愨湣懣戇遝澩磯碭硨碸礪礱硤磽磧磣镟滾龕瞘眥睞瞼佘羆羈釓釔釙釗釕釷釧釤鍆釵釹鈈鈦鈑鈐鈁鈧鈄鈥鈀鈺鉦鈷鈳鉕鈽鈸鉞鉬鉭鈿鑠鈰鉉鉈鉍鈮鈹鐸銬銠鉺銪鋮鋏鐃鋣鐺銱銦鎧銖鋌銩鏵銓鉿鎩銚錚銫銃鐋銨銣鐒錸鋱鏗鋥鋰鋯鋨銼鋝鋶鉲鐧鋃鋟鋦錒錆鍩錛鍀錁錕錮鍃錇錈錟錙鍥鍇鍶鍔鍤鎪鍰鎄鏤鏘鐨鋂鏌鎘鐫錼鎦鎰鎵鑌鏢鏜鏝鏍鏞鏃鏇鏑鐔钁鏷鑥鐓鑭鐠鑹鏹鐙鑊鐲鐿鑔鑣鍾穭穡鳩鳶鴇鴆鴣鶇鸕鴝鴟鷥鴯鷙鴰鵂鸞鵓鸝鵠鵒鷳鵜鵡鶓鵪鵯鶉鶘鶚鶿鶥鶩鷂鶼鸚鷓鷚鷯鷦鷲鷸鸌鷺鸛癤癘鬁屙皰瘂癆癇癉瘞瘺癭癮癩癲臒竇窶襠褳襝襇褸襤繈皸耮耬聹聵頇頎頏頡頜潁頦頷顎顓顳顢顙顥顬顰虯蟣蠆蜆蠔蠣蟶蛺蟯螄蠐蟈蠑螻蟎罌篤筧籩篳箏簀篋籜簞簫簣籪籟艤艫嫋羥秈糲糶糝餱縶麩趲醯釅釃鹺躉蹌蹠躒蹺蹕躚躋躓躑躡蹣躕躪躦觴觶靚靂霽靄齔齟齙齠齜齦齬齪齷黽黿鼉雋讎鑾鏨魷魴鮁鮃鯰鱸穌鮒鱟鮐鮭鮚鮪鮞鱭鮫鯗鱘鯁鱺鰱鰹鰣鰷鯀鯊鯇鯽鯖鯪鯫鯡鯤鯧鯝鯢鯰鯛鯴鯔鱝鰈鱷鰍鰒鰉鯿鰠鼇鰭鰨鰥鰩鰳鰾鱈鰻鰵鱅鱖鱔鱒鱧韃鞽韉韝鶻髏髖髕魘魎饗饜鬢黷黲鼴齇籤嘆塬囉裡唿於彙蒐註復";

        /// <summary>
        /// 转换为简体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSimplified(this string input)
        {
            StringBuilder result = new StringBuilder();
            foreach (char c in input)
            {
                int index = _C.IndexOf(c);
                result.Append(index < 0 ? c.ToString() : _S.Substring(index, 1));
            }
            return result.ToString();
        }

        /// <summary>
        /// 转换为繁体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToComplex(this string input)
        {
            StringBuilder result = new StringBuilder();
            foreach (char c in input)
            {
                int index = _S.IndexOf(c);
                result.Append(index < 0 ? c.ToString() : _C.Substring(index, 1));
            }
            return result.ToString();
        }

        #endregion

        #region Create

        /// <summary>
        /// 创建目录(如果不存在)
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(this string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        #endregion
    }
}