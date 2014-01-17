using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 枚举类型的扩展方法
    /// </summary>
    public static class ExtendMethodsForEnum
    {
        /// <summary>
        /// 获取枚举类型的值的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumType">typeof(枚举类型)</param>
        /// <returns></returns>
        public static IList<T> ToList<T>(this Type enumType) where T : struct
        {
            if (enumType.IsEnum)
                try
                {
                    return enumType.GetEnumValues().Cast<T>().ToList();
                }
                catch (Exception)
                {
                }
            throw new ArgumentException("enumType must be Enum type!");
        }

        /// <summary>
        /// 将枚举值拼接成字符串
        /// </summary>
        /// <param name="enumType">typeof(枚举类型)</param>
        /// <param name="separater">分隔符</param>
        /// <returns></returns>
        public static string Join(this Type enumType, char separater = ',')
        {
            return Join(enumType, separater.ToString());
        }

        /// <summary>
        /// 将枚举值拼接成字符串
        /// </summary>
        /// <param name="enumType">typeof(枚举类型)</param>
        /// <param name="separater">分隔符</param>
        /// <returns></returns>
        public static string Join(this Type enumType, string separater)
        {
            if (enumType.IsEnum)
            {
                StringBuilder s = new StringBuilder();
                try
                {
                    foreach (object item in enumType.GetEnumNames())
                        s.AppendFormat("{0}{1}", item, separater);
                }
                catch (Exception) { }
                return s.ToString();
            }
            throw new ArgumentException("enumType must be Enum type!");
        }

        /// <summary>
        /// 获取枚举实例对应的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static T Value<T>(this Enum e) where T : struct
        {
            Type underlyingType = Enum.GetUnderlyingType(e.GetType());
            if (underlyingType == typeof(sbyte))
                return (T)Convert.ChangeType(Convert.ToSByte(e), typeof(T));
            else if (underlyingType == typeof(byte))
                return (T)Convert.ChangeType(Convert.ToByte(e), typeof(T));
            else if (underlyingType == typeof(short))
                return (T)Convert.ChangeType(Convert.ToInt16(e), typeof(T));
            else if (underlyingType == typeof(ushort))
                return (T)Convert.ChangeType(Convert.ToUInt16(e), typeof(T));
            else if (underlyingType == typeof(int))
                return (T)Convert.ChangeType(Convert.ToInt32(e), typeof(T));
            else if (underlyingType == typeof(uint))
                return (T)Convert.ChangeType(Convert.ToUInt32(e), typeof(T));
            else if (underlyingType == typeof(long))
                return (T)Convert.ChangeType(Convert.ToInt64(e), typeof(T));
            else if (underlyingType == typeof(ulong))
                return (T)Convert.ChangeType(Convert.ToUInt64(e), typeof(T));
            throw new ArgumentException("UnderlyingType of enum not in(sbyte, byte, short, ushort, int, uint, long, ulong)");
        }
    }
}
