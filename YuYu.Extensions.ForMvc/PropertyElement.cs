using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 属性元素类
    /// </summary>
    public class PropertyElement : ConfigurationElement
    {
        /// <summary>
        /// 名称属性键
        /// </summary>
        public const string NameKey = "name";

        /// <summary>
        /// 值属性键
        /// </summary>
        public const string ValueKey = "value";

        /// <summary>
        /// 名称
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(NameKey, IsRequired = true)]
        public string Name
        {
            get { return (string)this[NameKey]; }
            set { this[NameKey] = value; }
        }

        /// <summary>
        /// 值
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(ValueKey, IsRequired = true)]
        public string Value
        {
            get { return (string)this[ValueKey]; }
            set { this[ValueKey] = value; }
        }

        /// <summary>
        /// 表示匿名类中属性的字符串
        /// </summary>
        internal string PropertyString
        {
            get
            {
                string prop = string.Empty;
                if ("UrlParameter.Optional".Equals(Value))
                    return "public System.Web.Mvc.UrlParameter {0}{get{return System.Web.Mvc.UrlParameter.Optional;}}".Replace("{0}", Name);
                else
                    return "public string {0}{get{return @\"{1}\";}}".Replace("{0}", Name).Replace("{1}", Value);
            }
        }
    }
}
