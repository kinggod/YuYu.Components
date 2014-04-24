using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 过滤器元素类
    /// </summary>
    public class MvcGlobalFilterElement : ConfigurationElement
    {
        /// <summary>
        /// 类型属性键
        /// </summary>
        public const string TypeKey = "type";

        /// <summary>
        /// 排序属性键
        /// </summary>
        public const string OrderKey = "order";

        /// <summary>
        /// 表示类型属性的字符串
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(TypeKey, IsRequired = true)]
        public string Type
        {
            get { return (string)this[TypeKey]; }
            set { this[TypeKey] = value; }
        }

        /// <summary>
        /// 排序属性
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(OrderKey, DefaultValue = 0)]
        public int Order
        {
            get { return (int)this[OrderKey]; }
            set { this[OrderKey] = value; }
        }

        /// <summary>
        /// 创建过滤器类实例对象
        /// </summary>
        /// <returns></returns>
        internal object CreateFilterInstance()
        {
            return Activator.CreateInstance(System.Type.GetType(Type));
        }
    }
}
