using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// YuYu配置块
    /// </summary>
    public class YuYuWebOptimizationConfigurationSectionGroup : ConfigurationSectionGroup
    {
        /// <summary>
        /// 绑定集合配置节KEY
        /// </summary>
        public const string BundleCollectionKey = "bundleCollection";

        /// <summary>
        /// 绑定集合配置节
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(BundleCollectionKey, IsRequired = true)]
        public virtual YuYuWebOptimizationBundleCollectionConfigurationSection BundleCollection
        {
            get { return (YuYuWebOptimizationBundleCollectionConfigurationSection)this.Sections[BundleCollectionKey]; }
        }
    }
}
