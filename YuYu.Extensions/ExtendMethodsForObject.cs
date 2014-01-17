using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtendMethodsForObject
    {
        
        /// <summary>
        /// 深度克隆
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="obj">待克隆对象</param>
        /// <returns></returns>
        public static T DeepClone<T>(this T obj) where T : class
        {
            using (Stream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(ms) as T;
            }
        }
    }
}
