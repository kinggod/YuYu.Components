using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Web.Http;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class YuYuWebApiConfigurationManager : YuYuWebConfigurationManager
    {
        /// <summary>
        /// 配置节名称
        /// </summary>
        public const string WebApiSectionGroupName = "webApi";

        /// <summary>
        /// 注册WebApi
        /// </summary>
        /// <param name="config"></param>
        public static void RegisterWebApis(HttpConfiguration config)
        {
            foreach (WebApiRouteElement route in YuYuWebApiConfigurationSectionGroup.HttpConfiguration.Routes.RouteElements)
            {
                config.Routes.MapHttpRoute(
                    name: route.Name,
                    routeTemplate: route.RouteTemplate,
                    defaults: route.Defaults.CreateObject(),
                    constraints: route.Constraints.CreateObject(),
                    handler: route.Handler
                );
            }
        }

        /// <summary>
        /// WebApi配置节组
        /// </summary>
        public static YuYuWebApiConfigurationSectionGroup YuYuWebApiConfigurationSectionGroup = (YuYuWebApiConfigurationSectionGroup)YuYuWebConfigurationSectionGroup.SectionGroups[WebApiSectionGroupName];
    }
}
