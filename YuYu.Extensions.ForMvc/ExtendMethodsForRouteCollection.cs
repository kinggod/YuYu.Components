using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
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
        public static void RegisterRoutes(this System.Web.Routing.RouteCollection routes)
        {
            YuYuMvcConfigurationManager.RegisterRoutes(routes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="protocol"></param>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Route MapDomainRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, string url)
        {
            return routes.MapDomainRoute(name, protocol, domain, url, null, null, null);
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
        /// <returns></returns>
        public static Route MapDomainRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, int port, string url)
        {
            return routes.MapDomainRoute(name, protocol, domain, port, url, null, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="protocol"></param>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <returns></returns>
        public static Route MapDomainRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, string url, object defaults = null)
        {
            return routes.MapDomainRoute(name, protocol, domain, url, defaults, null, null);
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
        /// <param name="defaults"></param>
        /// <returns></returns>
        public static Route MapDomainRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, int port, string url, object defaults = null)
        {
            return routes.MapDomainRoute(name, protocol, domain, port, url, defaults, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="protocol"></param>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="namespaces"></param>
        /// <returns></returns>
        public static Route MapDomainRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, string url, string[] namespaces)
        {
            return routes.MapDomainRoute(name, protocol, domain, url, null, null, namespaces);
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
        /// <param name="namespaces"></param>
        /// <returns></returns>
        public static Route MapDomainRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, int port, string url, string[] namespaces)
        {
            return routes.MapDomainRoute(name, protocol, domain, port, url, null, null, namespaces);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="protocol"></param>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <returns></returns>
        public static Route MapDomainRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, string url, object defaults, object constraints)
        {
            return routes.MapDomainRoute(name, protocol, domain, url, defaults, constraints, null);
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
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <returns></returns>
        public static Route MapDomainRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, int port, string url, object defaults, object constraints)
        {
            return routes.MapDomainRoute(name, protocol, domain, port, url, defaults, constraints, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="protocol"></param>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <param name="namespaces"></param>
        /// <returns></returns>
        public static Route MapDomainRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, string url, object defaults, string[] namespaces)
        {
            return routes.MapDomainRoute(name, protocol, domain, url, defaults, null, namespaces);
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
        /// <param name="defaults"></param>
        /// <param name="namespaces"></param>
        /// <returns></returns>
        public static Route MapDomainRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, int port, string url, object defaults, string[] namespaces)
        {
            return routes.MapDomainRoute(name, protocol, domain, port, url, defaults, null, namespaces);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="protocol"></param>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <param name="namespaces"></param>
        /// <returns></returns>
        public static Route MapDomainRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, string url, object defaults, object constraints, string[] namespaces)
        {
            return routes.MapDomainRoute(name, protocol, domain, 80, url, defaults, constraints, namespaces);
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
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <param name="namespaces"></param>
        /// <returns></returns>
        public static Route MapDomainRoute(this System.Web.Routing.RouteCollection routes, string name, string protocol, string domain, int port, string url, object defaults, object constraints, string[] namespaces)
        {
            if (routes == null)
                throw new ArgumentNullException("routes");
            if (domain == null)
                throw new ArgumentNullException("domain");
            if (url == null)
                throw new ArgumentNullException("url");
            Route route = new DomainRoute(domain, url, new MvcRouteHandler())
            {
                Defaults = RouteValueDictionaryHelper.CreateRouteValueDictionary(defaults),
                Constraints = RouteValueDictionaryHelper.CreateRouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary(),
                Protocol = protocol,
                Port = port,
            };
            if (namespaces.IsNotNullOrEmpty())
                route.DataTokens["Namespaces"] = namespaces;
            routes.Add(name, route);
            return route;
        }
    }
}
