using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// YuYu.Mvc配置块
    /// </summary>
    public class YuYuMvcConfigurationSectionGroup : ConfigurationSectionGroup
    {
        /// <summary>
        /// 路由配置节
        /// </summary>
        public const string RoutesSectionKey = "routeCollection";

        /// <summary>
        /// 过滤器配置节
        /// </summary>
        public const string FiltersSectionKey = "globalFilterCollection";

        /// <summary>
        /// 路由配置节
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(RoutesSectionKey, IsRequired = true)]
        public virtual YuYuMvcRouteCollectionConfigurationSection RoutesSection
        {
            get { return (YuYuMvcRouteCollectionConfigurationSection)this.Sections[RoutesSectionKey]; }
        }

        /// <summary>
        /// 过滤器配置节
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(FiltersSectionKey)]
        public virtual YuYuMvcGlobalFilterCollectionConfigurationSection FiltersSection
        {
            get { return (YuYuMvcGlobalFilterCollectionConfigurationSection)this.Sections[FiltersSectionKey]; }
        }
    }
}
