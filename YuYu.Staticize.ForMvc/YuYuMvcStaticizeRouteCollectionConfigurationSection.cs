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
    public class YuYuMvcStaticizeRouteCollectionConfigurationSection : ConfigurationSection
    {

        /// <summary>
        /// 用于生成静态页的路由配置节
        /// </summary>
        public const string RoutesKey = "routes";

        /// <summary>
        /// 用于生成静态页的路由集合
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(RoutesKey)]
        public virtual RouteCollection Routes
        {
            get { return (RouteCollection)this[RoutesKey]; }
            set { this[RoutesKey] = value; }
        }

    }
}
