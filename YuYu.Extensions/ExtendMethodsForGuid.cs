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
    public static class ExtendMethodsForGuid
    {
        /// <summary>
        /// 等同于 ToString("N")
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string N(this Guid guid)
        {
            return guid.ToString("N");
        }

        /// <summary>
        /// 判断可空类型 Guid? 为null或Guid.Empty
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this Guid? guid)
        {
            if (!guid.HasValue || guid.Value == Guid.Empty)
                return true;
            return false;
        }

        /// <summary>
        /// 判断可空类型 Guid? 不为null或Guid.Empty
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this Guid? guid)
        {
            return !guid.IsNullOrEmpty();
        }
    }
}
