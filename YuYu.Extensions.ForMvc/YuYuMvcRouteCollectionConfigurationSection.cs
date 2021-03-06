﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 路由配置节
    /// </summary>
    public class YuYuMvcRouteCollectionConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// 路由配置节
        /// </summary>
        public const string RoutesKey = "routes";

        /// <summary>
        /// 路由集合
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(RoutesKey, IsRequired = true)]
        public virtual MvcRouteCollection Routes
        {
            get { return (MvcRouteCollection)this[RoutesKey]; }
            set { this[RoutesKey] = value; }
        }
    }
}
