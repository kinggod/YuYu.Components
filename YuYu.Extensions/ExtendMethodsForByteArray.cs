using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// Byte[]的扩展方法
    /// </summary>
    public static class ExtendMethodsForByteArray
    {
        /// <summary>
        /// 将byte[]数组转换为字符串输出
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="format">输出格式</param>
        /// <returns></returns>
        public static string ToString(this byte[] bytes, string format = "X2")
        {
            StringBuilder output = new StringBuilder(bytes.Length);
            for (int i = 0; i < bytes.Length - 1; i++)
                output.Append(bytes[i].ToString(format));
            return output.ToString();
        }

        /// <summary>
        /// MD5编码
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="toUpper">全部大写</param>
        /// <returns></returns>
        public static string MD5Encode(this byte[] bytes, bool toUpper = false)
        {
            using (MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider())
            {
                string encoded = BitConverter.ToString(md5CryptoServiceProvider.ComputeHash(bytes)).Replace("-", string.Empty);
                return toUpper ? encoded.ToUpperInvariant() : encoded;
            }
        }
    }
}
