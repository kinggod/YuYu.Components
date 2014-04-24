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
    public class YuYuMvcConfigurationManager : YuYuWebConfigurationManager
    {
        /// <summary>
        /// 配置节名称
        /// </summary>
        public const string MvcSectionGroupName = "mvc";

        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="routes">RouteCollection</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            foreach (MvcRouteElement route in YuYuMvcConfigurationSectionGroup.RoutesSection.Routes.RouteElements)
            {
                RouteValueDictionary defaults = RouteValueDictionaryHelper.CreateRouteValueDictionary(route.Defaults.CreateObject());
                RouteValueDictionary constraints = RouteValueDictionaryHelper.CreateRouteValueDictionary(route.Constraints.CreateObject());
                Route item = null;
                if (route.Domain.IsNullOrWhiteSpace())
                    item = new Route(route.Url, route.RouteHandler)
                    {
                        Defaults = defaults,
                        Constraints = constraints,
                        DataTokens = new RouteValueDictionary()
                    };
                else
                    item = new DomainRoute(route.Domain, route.Url, route.RouteHandler)
                    {
                        Defaults = defaults,
                        Constraints = constraints,
                        DataTokens = new RouteValueDictionary(),
                        Port = route.Port,
                        Protocol = route.Protocol,
                    };
                if (route.Namespaces.IsNotNullOrWhiteSpace())
                    item.DataTokens["Namespaces"] = route.Namespaces.Split(',');
                routes.Add(route.Name, item);
            }
        }

        /// <summary>
        /// 注册过滤器
        /// </summary>
        /// <param name="filters">GlobalFilterCollection</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            foreach (MvcGlobalFilterElement filter in YuYuMvcConfigurationSectionGroup.FiltersSection.GlobalFilters.GlobalFilterElements)
            {
                filters.Add(filter.CreateFilterInstance(), filter.Order);
            }
        }

        /// <summary>
        /// Mvc配置节组
        /// </summary>
        public static YuYuMvcConfigurationSectionGroup YuYuMvcConfigurationSectionGroup = (YuYuMvcConfigurationSectionGroup)YuYuWebConfigurationSectionGroup.SectionGroups[MvcSectionGroupName];
    }
}
