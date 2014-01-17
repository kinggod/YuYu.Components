using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class YuYuMvcConfigurationManager
    {
        /// <summary>
        /// 配置节名称
        /// </summary>
        public const string SectionGroupName = "yuyu.mvc";

        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(System.Web.Routing.RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            foreach (var route in YuYuMvcConfigurationSectionGroup.RoutesSection.Routes.RouteElements)
            {
                RouteValueDictionary defaults = Helper.CreateRouteValueDictionary(route.Defaults.CreateObject());
                RouteValueDictionary constraints = Helper.CreateRouteValueDictionary(route.Constraints.CreateObject());
                Route item = null;
                if (!string.IsNullOrWhiteSpace(route.Domain))
                    item = new DomainRoute(route.Domain, route.Url, route.CreateRouteHandlerInstance())
                    {
                        Defaults = defaults,
                        Constraints = constraints,
                        DataTokens = new RouteValueDictionary(),
                        Port = route.Port,
                        Protocol = route.Protocol,
                    };
                else
                    item = new Route(route.Url, route.CreateRouteHandlerInstance())
                    {
                        Defaults = defaults,
                        Constraints = constraints,
                        DataTokens = new RouteValueDictionary()
                    };
                if (!string.IsNullOrWhiteSpace(route.Namespaces))
                    item.DataTokens["Namespaces"] = route.Namespaces.Split(',');
                routes.Add(route.Name, item);
            }
        }

        /// <summary>
        /// 注册过滤器
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            foreach (var filter in YuYuMvcConfigurationSectionGroup.FiltersSection.Filters.FilterElements)
            {
                filters.Add(filter.CreateFilterInstance(), filter.Order);
            }
        }

        /// <summary>
        /// YuYu.Mvc配置节组
        /// </summary>
        public static YuYuMvcConfigurationSectionGroup YuYuMvcConfigurationSectionGroup = (YuYuMvcConfigurationSectionGroup)WebConfigurationManager.OpenWebConfiguration("~/web.config").GetSectionGroup(SectionGroupName);
    }
}
