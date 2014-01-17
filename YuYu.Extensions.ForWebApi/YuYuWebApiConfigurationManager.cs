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
    public class YuYuWebApiConfigurationManager
    {
        /// <summary>
        /// 配置节名称
        /// </summary>
        public const string SectionGroupName = "yuyu.webApi";

        /// <summary>
        /// 注册WebApi
        /// </summary>
        /// <param name="config"></param>
        public static void RegisterWebApis(HttpConfiguration config)
        {
            _RegisterWebApis(config);
        }

        /// <summary>
        /// YuYu.WebApi配置节组
        /// </summary>
        public static YuYuWebApiConfigurationSectionGroup YuYuWebApiConfigurationSectionGroup = (YuYuWebApiConfigurationSectionGroup)WebConfigurationManager.OpenWebConfiguration("~/web.config").GetSectionGroup(SectionGroupName);

        #region

        private static void _RegisterWebApis(HttpConfiguration config)
        {
            foreach (var webApi in YuYuWebApiConfigurationSectionGroup.WebApisSection.WebApis.WebApiElements)
            {
                config.Routes.MapHttpRoute(
                    name: webApi.Name,
                    routeTemplate: webApi.RouteTemplate,
                    defaults: webApi.Defaults.CreateObject(),
                    constraints: webApi.Constraints.CreateObject(),
                    handler: webApi.CreateHandlerInstance()
                );
            }
        }

        #endregion
    }
}
