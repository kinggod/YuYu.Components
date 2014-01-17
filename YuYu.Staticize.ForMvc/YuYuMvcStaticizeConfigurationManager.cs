using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class YuYuMvcStaticizeConfigurationManager : YuYuMvcConfigurationManager
    {
        /// <summary>
        /// 配置节名称
        /// </summary>
        public const string SectionName = "staticizeRouteCollection";

        /// <summary>
        /// 用以配置需要生成静态页的路由集合
        /// </summary>
        public static System.Web.Routing.RouteCollection StaticizeRoutes = new System.Web.Routing.RouteCollection();

        /// <summary>
        /// 注册用于生成静态页的路由
        /// </summary>
        /// <param name="directoryParameterName">文件夹参数名称</param>
        /// <param name="cycleMode">配置此项（MM：每月更新；WW：每周更新；DD：每日更新）用于生成静态路由</param>
        public static void RegisterStaticizeRoutes(string directoryParameterName = "staticizeDirectory", string cycleMode = "DD")
        {
            string folderName = string.Empty;
            if (!string.IsNullOrWhiteSpace(directoryParameterName))
                folderName = DateTime.Now.GetDirectoryName(cycleMode);
            foreach (var route in _Section.Routes.RouteElements)
            {
                RouteValueDictionary defaults = Helper.CreateRouteValueDictionary(route.Defaults.CreateObject());
                RouteValueDictionary constraints = Helper.CreateRouteValueDictionary(route.Constraints.CreateObject());
                if (!string.IsNullOrWhiteSpace(directoryParameterName))
                    defaults.Add(directoryParameterName, folderName);
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
                StaticizeRoutes.Add(route.Name, item);
            }
        }

        #region

        private static YuYuMvcStaticizeRouteCollectionConfigurationSection _Section = (YuYuMvcStaticizeRouteCollectionConfigurationSection)YuYuMvcConfigurationSectionGroup.Sections[SectionName];

        private static string _WeekOfYear(DateTime dt)
        {
            DateTime firstDayOfYear = new DateTime(dt.Year, 1, 1);
            int skipWeek = firstDayOfYear.DayOfWeek > 0 ? 1 : 0;
            int days = dt.DayOfYear - (firstDayOfYear.DayOfWeek > 0 ? 7 - (int)firstDayOfYear.DayOfWeek : 0);
            int weekOfYear = days / 7 + 1;
            if (days % 7 > 0)
                weekOfYear += 1;
            if (weekOfYear > 9)
                return weekOfYear.ToString();
            else
                return "0" + weekOfYear;
        }

        #endregion
    }
}
