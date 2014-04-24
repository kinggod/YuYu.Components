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
    public class YuYuWebOptimizationBundleCollectionConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// 捆绑配置节
        /// </summary>
        public const string BundlesKey = "bundles";

        /// <summary>
        /// 捆绑集合
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(BundlesKey, IsRequired = true)]
        public virtual WebOptimizationBundleCollection Bundles
        {
            get { return (WebOptimizationBundleCollection)this[BundlesKey]; }
            set { this[BundlesKey] = value; }
        }
    }
}
