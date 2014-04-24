using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Configuration;
using System.Web.Routing;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class YuYuWebConfigurationManager
    {
        /// <summary>
        /// 配置节名称
        /// </summary>
        public const string SectionGroupName = "yuyu.web";

        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="routes">RouteCollection</param>
        public static void RegisterPageRoutes(System.Web.Routing.RouteCollection routes)
        {
            foreach (PageRouteElement route in YuYuWebConfigurationSectionGroup.RoutesSection.Routes.RouteElements)
            {
                RouteValueDictionary defaults = RouteValueDictionaryHelper.CreateRouteValueDictionary(route.Defaults.CreateObject());
                RouteValueDictionary constraints = RouteValueDictionaryHelper.CreateRouteValueDictionary(route.Constraints.CreateObject());
                if (string.IsNullOrWhiteSpace(route.Domain))
                    routes.MapPageRoute(route.Name, route.Url, route.PhysicalFile, route.CheckPhysicalUrlAccess, defaults, constraints);
                else
                    routes.Add(route.Name, new DomainRoute(route.Domain, route.Url, new PageRouteHandler(route.PhysicalFile, route.CheckPhysicalUrlAccess))
                    {
                        Defaults = defaults,
                        Constraints = constraints,
                        DataTokens = new RouteValueDictionary(),
                        Port = route.Port,
                        Protocol = route.Protocol,
                    });
            }
        }

        /// <summary>
        /// YuYu.Web配置节组
        /// </summary>
        public static YuYuWebConfigurationSectionGroup YuYuWebConfigurationSectionGroup = (YuYuWebConfigurationSectionGroup)WebConfigurationManager.OpenWebConfiguration("~/web.config").GetSectionGroup(SectionGroupName);
    }
}
