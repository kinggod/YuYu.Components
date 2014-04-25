using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// YuYu配置块组
    /// </summary>
    public class YuYuWebConfigurationSectionGroup : ConfigurationSectionGroup
    {
        /// <summary>
        /// 路由配置节
        /// </summary>
        public const string RoutesSectionKey = "routeCollection";

        /// <summary>
        /// 路由配置节
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(RoutesSectionKey, IsRequired = true)]
        public virtual YuYuFileRouteCollectionConfigurationSection YuYuFileRouteCollectionConfigurationSection
        {
            get { return (YuYuFileRouteCollectionConfigurationSection)this.Sections[RoutesSectionKey]; }
        }
    }
}
