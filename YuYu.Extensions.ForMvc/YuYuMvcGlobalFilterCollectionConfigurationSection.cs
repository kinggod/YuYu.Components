using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 过滤器配置节
    /// </summary>
    public class YuYuMvcGlobalFilterCollectionConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// 过滤器配置节
        /// </summary>
        public const string FiltersKey = "filters";

        /// <summary>
        /// 过滤器集合
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(FiltersKey, IsRequired = true)]
        public virtual FilterCollection Filters
        {
            get { return (FilterCollection)this[FiltersKey]; }
            set { this[FiltersKey] = value; }
        }
    }
}
