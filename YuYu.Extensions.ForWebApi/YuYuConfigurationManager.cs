using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace YuYu.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class YuYuConfigurationManager
    {
        /// <summary>
        /// 配置节名称
        /// </summary>
        public const string SectionName = "yuyu";

        /// <summary>
        /// 注册WebApi
        /// </summary>
        /// <param name="config"></param>
        public static void RegisterWebApis(HttpConfiguration config)
        {
            _RegisterWebApis(config);
        }

        #region

        private static YuYuConfigurationSection _YuYu = (YuYuConfigurationSection)ConfigurationManager.GetSection(SectionName);

        private static void _RegisterWebApis(HttpConfiguration config)
        {
            foreach (var webApi in _YuYu.WebApis.WebApiElements)
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
