using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtendMethodsForRouteCollection
    {
        /// <summary>
        /// 注册Route
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterPageRoutes(this System.Web.Routing.RouteCollection routes)
        {
            YuYuWebConfigurationManager.RegisterPageRoutes(routes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="protocol"></param>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="physicalFile"></param>
        /// <param name="checkPhysicalUrlAccess"></param>
        /// <returns></returns>
        public static Route MapDomainPageRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, string url, string physicalFile, bool checkPhysicalUrlAccess=false)
        {
            return routes.MapDomainPageRoute(name, protocol, domain, url, physicalFile, checkPhysicalUrlAccess, null, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="protocol"></param>
        /// <param name="domain"></param>
        /// <param name="port"></param>
        /// <param name="url"></param>
        /// <param name="physicalFile"></param>
        /// <param name="checkPhysicalUrlAccess"></param>
        /// <returns></returns>
        public static Route MapDomainPageRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, int port, string url, string physicalFile, bool checkPhysicalUrlAccess = false)
        {
            return routes.MapDomainPageRoute(name, protocol, domain, port, url, physicalFile, checkPhysicalUrlAccess, null, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="protocol"></param>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="physicalFile"></param>
        /// <param name="checkPhysicalUrlAccess"></param>
        /// <param name="defaults"></param>
        /// <returns></returns>
        public static Route MapDomainPageRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, string url, string physicalFile, bool checkPhysicalUrlAccess , object defaults )
        {
            return routes.MapDomainPageRoute(name, protocol, domain, url, physicalFile, checkPhysicalUrlAccess, defaults, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="protocol"></param>
        /// <param name="domain"></param>
        /// <param name="port"></param>
        /// <param name="url"></param>
        /// <param name="physicalFile"></param>
        /// <param name="checkPhysicalUrlAccess"></param>
        /// <param name="defaults"></param>
        /// <returns></returns>
        public static Route MapDomainPageRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, int port, string url, string physicalFile, bool checkPhysicalUrlAccess , object defaults )
        {
            return routes.MapDomainPageRoute(name, protocol, domain, port, url, physicalFile, checkPhysicalUrlAccess, defaults, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="protocol"></param>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="physicalFile"></param>
        /// <param name="checkPhysicalUrlAccess"></param>
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <returns></returns>
        public static Route MapDomainPageRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, string url, string physicalFile, bool checkPhysicalUrlAccess, object defaults, object constraints)
        {
            return routes.MapDomainPageRoute(name, protocol, domain, url, physicalFile, checkPhysicalUrlAccess, defaults, constraints, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="protocol"></param>
        /// <param name="domain"></param>
        /// <param name="port"></param>
        /// <param name="url"></param>
        /// <param name="physicalFile"></param>
        /// <param name="checkPhysicalUrlAccess"></param>
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <returns></returns>
        public static Route MapDomainPageRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, int port, string url, string physicalFile, bool checkPhysicalUrlAccess, object defaults, object constraints)
        {
            return routes.MapDomainPageRoute(name, protocol, domain, port, url, physicalFile, checkPhysicalUrlAccess, defaults, constraints, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="protocol"></param>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="physicalFile"></param>
        /// <param name="checkPhysicalUrlAccess"></param>
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <param name="dataTokens"></param>
        /// <returns></returns>
        public static Route MapDomainPageRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, string url, string physicalFile, bool checkPhysicalUrlAccess, object defaults, object constraints, object dataTokens)
        {
            return routes.MapDomainPageRoute(name, protocol, domain, 80, url, physicalFile, checkPhysicalUrlAccess, defaults, constraints, dataTokens);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="protocol"></param>
        /// <param name="domain"></param>
        /// <param name="port"></param>
        /// <param name="url"></param>
        /// <param name="physicalFile"></param>
        /// <param name="checkPhysicalUrlAccess"></param>
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <param name="dataTokens"></param>
        /// <returns></returns>
        public static Route MapDomainPageRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, int port, string url, string physicalFile, bool checkPhysicalUrlAccess, object defaults, object constraints, object dataTokens)
        {
            if (routes == null)
                throw new ArgumentNullException("routes");
            if (domain == null)
                throw new ArgumentNullException("domain");
            if (url == null)
                throw new ArgumentNullException("url");
            Route route = new DomainRoute(domain, url, new PageRouteHandler(physicalFile, checkPhysicalUrlAccess))
            {
                Defaults = RouteValueDictionaryHelper.CreateRouteValueDictionary(defaults),
                Constraints = RouteValueDictionaryHelper.CreateRouteValueDictionary(constraints),
                DataTokens = RouteValueDictionaryHelper.CreateRouteValueDictionary(dataTokens),
                Protocol = protocol,
                Port = port,
            };
            routes.Add(name, route);
            return route;
        }
    }
}
