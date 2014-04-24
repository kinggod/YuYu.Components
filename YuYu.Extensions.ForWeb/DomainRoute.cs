using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace YuYu.Components
{
    /// <summary>
    /// 域名路由
    /// </summary>
    public class DomainRoute : Route
    {
        /// <summary>
        /// 请求协议
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// 域
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="routeHandler"></param>
        public DomainRoute(string domain, string url, IRouteHandler routeHandler) : this(domain, url, null, null, null, routeHandler) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <param name="routeHandler"></param>
        public DomainRoute(string domain, string url, RouteValueDictionary defaults, IRouteHandler routeHandler) : this(domain, url, defaults, null, null, routeHandler) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <param name="routeHandler"></param>
        public DomainRoute(string domain, string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler) : this(domain, url, defaults, constraints, null, routeHandler) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <param name="dataTokens"></param>
        /// <param name="routeHandler"></param>
        public DomainRoute(string domain, string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
            this.Domain = domain;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            Regex domainRegex = new Regex("^" + this.Domain.Replace("/", @"\/?").Replace(".", @"\.?").Replace("-", @"\-?").Replace("{", @"(?<").Replace("}", @">(\w*))") + "$");
            string requestDomain = httpContext.Request.Headers["host"];
            if (string.IsNullOrEmpty(requestDomain))
                requestDomain = httpContext.Request.Url.Host;
            else if (requestDomain.IndexOf(":") > 0)
                requestDomain = requestDomain.Substring(0, requestDomain.IndexOf(":"));
            Match domainMatch = domainRegex.Match(requestDomain);
            RouteData data = base.GetRouteData(httpContext);
            if (domainMatch.Success)
            {
                data = base.GetRouteData(httpContext);
                if (data != null)
                    for (int i = 1; i < domainMatch.Groups.Count; i++)
                    {
                        Group group = domainMatch.Groups[i];
                        if (group.Success)
                        {
                            string key = domainRegex.GroupNameFromNumber(i);
                            if (!string.IsNullOrEmpty(key) && !char.IsNumber(key, 0) && !string.IsNullOrEmpty(group.Value))
                                data.Values[key] = group.Value;
                        }
                    }
            }
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return base.GetVirtualPath(requestContext, _RemoveDomainTokens(values));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public DomainData GetDomainData(RequestContext requestContext, RouteValueDictionary values)
        {
            string hostname = this.Domain;
            foreach (KeyValuePair<string, object> pair in values)
                hostname = hostname.Replace("{" + pair.Key + "}", pair.Value.ToString());
            return new DomainData
            {
                Protocol = string.IsNullOrWhiteSpace(this.Protocol) ? requestContext.HttpContext.Request.Url.Scheme : this.Protocol,
                Host = hostname,
                Port = this.Port > 0 ? requestContext.HttpContext.Request.Url.Port : this.Port,
            };
        }

        private RouteValueDictionary _RemoveDomainTokens(RouteValueDictionary values)
        {
            Regex tokenRegex = new Regex(@"({\w*})*-?\.?\/?({\w*})*-?\.?\/?({\w*})*-?\.?\/?({\w*})*-?\.?\/?({\w*})*-?\.?\/?({\w*})*-?\.?\/?({\w*})*-?\.?\/?({\w*})*-?\.?\/?({\w*})*-?\.?\/?({\w*})*-?\.?\/?({\w*})*-?\.?\/?({\w*})*-?\.?\/?");
            Match tokenMatch = tokenRegex.Match(Domain);
            for (int i = 0; i < tokenMatch.Groups.Count; i++)
            {
                Group group = tokenMatch.Groups[i];
                if (group.Success)
                {
                    string key = group.Value.Replace("{", string.Empty).Replace("}", string.Empty);
                    if (values.ContainsKey(key))
                        values.Remove(key);
                }
            }
            return values;
        }
    }
}