using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtendMethodsForRandom
    {
        /// <summary>
        /// 获取随机的Boolean类型值
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        public static bool NextBool(this Random random)
        {
            return random.NextDouble() > 0.5D;
        }

        /// <summary>
        /// 获取随机的枚举值
        /// </summary>
        /// <typeparam name="T">enum type</typeparam>
        /// <param name="random"></param>
        /// <returns></returns>
        public static T NextEnum<T>(this Random random) where T : struct
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                Array array = Enum.GetValues(type);
                int index = random.Next(array.GetLowerBound(0), array.GetUpperBound(0) + 1);
                return (T)array.GetValue(index);
            }
            else
                throw new InvalidOperationException("T must be enum type!");
        }
    }
}