using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// YuYu配置块
    /// </summary>
    public class YuYuWebApiHttpConfigurationConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// webApi集合配置节
        /// </summary>
        public const string RoutesKey = "routes";

        /// <summary>
        /// WebApi集合
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(RoutesKey)]
        public virtual WebApiRouteCollection Routes
        {
            get { return (WebApiRouteCollection)this[RoutesKey]; }
            set { this[RoutesKey] = value; }
        }
    }
}
