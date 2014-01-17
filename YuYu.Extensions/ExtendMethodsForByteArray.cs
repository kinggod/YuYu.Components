using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
