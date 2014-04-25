using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace YuYu.Components
{
    /// <summary>
    /// 路由配置节
    /// </summary>
    public class YuYuFileRouteCollectionConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// 路由配置节
        /// </summary>
        public const string RoutesKey = "routes";

        /// <summary>
        /// DefaultRouteHandlerType键
        /// </summary>
        public const string DefaultRouteHandlerTypeKey = "defaultRouteHandlerType";

        /// <summary>
        /// 路由集合
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(RoutesKey, IsRequired = true)]
        public virtual FileRouteCollection Routes
        {
            get { return (FileRouteCollection)this[RoutesKey]; }
            set { this[RoutesKey] = value; }
        }

        /// <summary>
        /// 表示路由控制程序类型的字符串
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(DefaultRouteHandlerTypeKey)]
        public string DefaultRouteHandlerType
        {
            get { return (string)this[DefaultRouteHandlerTypeKey]; }
            set { this[DefaultRouteHandlerTypeKey] = value; }
        }

        /// <summary>
        /// RouteHandler
        /// </summary>
        public IRouteHandler CreateDefaultRouteHandler(string physicalFile, bool checkPhysicalUrlAccess)
        {
            IRouteHandler routeHandler = null;
            if (!string.IsNullOrWhiteSpace(DefaultRouteHandlerType))
                routeHandler = Activator.CreateInstance(System.Type.GetType(DefaultRouteHandlerType), physicalFile, checkPhysicalUrlAccess) as IRouteHandler;
            return routeHandler ?? new PageRouteHandler(physicalFile, checkPhysicalUrlAccess);
        }
    }
}
