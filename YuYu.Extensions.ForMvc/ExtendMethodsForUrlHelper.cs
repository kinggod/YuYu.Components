using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtendMethodsForUrlHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="requireAbsoluteUrl"></param>
        /// <returns></returns>
        public static string Action(this UrlHelper urlHelper, string actionName, bool requireAbsoluteUrl)
        {
            return urlHelper.Action(actionName, null, new RouteValueDictionary(null), requireAbsoluteUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="routeValues"></param>
        /// <param name="requireAbsoluteUrl"></param>
        /// <returns></returns>
        public static string Action(this UrlHelper urlHelper, string actionName, object routeValues, bool requireAbsoluteUrl)
        {
            return urlHelper.Action(actionName, null, new RouteValueDictionary(routeValues), requireAbsoluteUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="routeValues"></param>
        /// <param name="requireAbsoluteUrl"></param>
        /// <returns></returns>
        public static string Action(this UrlHelper urlHelper, string actionName, RouteValueDictionary routeValues, bool requireAbsoluteUrl)
        {
            return urlHelper.Action(actionName, null, routeValues, requireAbsoluteUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="requireAbsoluteUrl"></param>
        /// <returns></returns>
        public static string Action(this UrlHelper urlHelper, string actionName, string controllerName, bool requireAbsoluteUrl)
        {
            return urlHelper.Action(actionName, controllerName, new RouteValueDictionary(), requireAbsoluteUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="requireAbsoluteUrl"></param>
        /// <returns></returns>
        public static string Action(this UrlHelper urlHelper, string actionName, string controllerName, object routeValues, bool requireAbsoluteUrl)
        {
            return urlHelper.Action(actionName, controllerName, new RouteValueDictionary(routeValues), requireAbsoluteUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="requireAbsoluteUrl"></param>
        /// <returns></returns>
        public static string Action(this UrlHelper urlHelper, string actionName, string controllerName, RouteValueDictionary routeValues, bool requireAbsoluteUrl)
        {
            if (requireAbsoluteUrl)
            {
                RouteBase route = RouteTable.Routes.GetVirtualPath(_GetRequestContext(urlHelper, controllerName, actionName), routeValues).Route;
                DomainRoute domainRoute = route as DomainRoute;
                if (domainRoute != null)
                {
                    DomainData domain = domainRoute.GetDomainData(urlHelper.RequestContext, routeValues);
                    string urlString = urlHelper.Action(actionName, controllerName, routeValues, domain.Protocol, domain.Host);
                    return Regex.Replace(urlString, @"\:\d+", domain.Port > 0 ? ":" + domain.Port : string.Empty);
                }
            }
            return urlHelper.Action(actionName, controllerName, routeValues);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="protocol"></param>
        /// <param name="requireAbsoluteUrl"></param>
        /// <returns></returns>
        public static string Action(this UrlHelper urlHelper, string actionName, string controllerName, object routeValues, string protocol, bool requireAbsoluteUrl)
        {
            if (requireAbsoluteUrl)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(routeValues);
                RouteBase route = RouteTable.Routes.GetVirtualPath(_GetRequestContext(urlHelper, controllerName, actionName), routeValueDictionary).Route;
                DomainRoute domainRoute = route as DomainRoute;
                if (domainRoute != null)
                {
                    DomainData domain = domainRoute.GetDomainData(urlHelper.RequestContext, routeValueDictionary);
                    string urlString = urlHelper.Action(actionName, controllerName, new RouteValueDictionary(routeValues), protocol, domain.Host);
                    return Regex.Replace(urlString, @"\:\d+", domain.Port > 0 ? ":" + domain.Port : string.Empty);
                }
            }
            return urlHelper.Action(actionName, controllerName, routeValues, protocol);
        }

        private static RequestContext _GetRequestContext(UrlHelper urlHelper, string controllerName, string actionName)
        {
            HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);
            RouteData routeData = RouteTable.Routes.GetRouteData(httpContext);
            routeData.Values["controller"] = controllerName;
            routeData.Values["action"] = actionName;
            return new RequestContext(httpContext, routeData);
        }
    }
}