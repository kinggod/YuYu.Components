using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace YuYu.Components
{
    /// <summary>
    /// 权限验证
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "Unsealed so that subclassed types can set properties in the default constructor or override our behavior.")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class YuYuAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// Http响应状态码
        /// </summary>
        protected HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="controllerNamespace"></param>
        /// <returns></returns>
        protected virtual bool AuthorizeCore(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");
            HttpContextBase httpContext = filterContext.HttpContext;
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Count() > 0)
            {
                this.HttpStatusCode = HttpStatusCode.OK;
                return true;
            }

            #region 验证登录状态

            if (!httpContext.User.Identity.IsAuthenticated)
            {
                this.HttpStatusCode = HttpStatusCode.Unauthorized;
                FormsAuthentication.SignOut();
                return false;
            }

            #endregion

            #region 验证权限状态

            Type controllerType = filterContext.Controller.GetType();
            string @namespace = controllerType.Namespace,
                controllerName = controllerType.Name,
                actionName = filterContext.ActionDescriptor.ActionName;
            //权限验证不通过
            if (!YuYuMembership.HasAuthority(httpContext.User, actionName, controllerName, @namespace))
            {
                this.HttpStatusCode = HttpStatusCode.Forbidden;
                return false;
            }

            #endregion

            this.HttpStatusCode = HttpStatusCode.OK;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");
            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
                // If a child action cache block is active, we need to fail immediately, even if authorization
                // would have succeeded. The reason is that there's no way to hook a callback to rerun
                // authorization before the fragment is served from the cache, so we can't guarantee that this
                // filter will be re-run on subsequent requests.
                throw new InvalidOperationException("Can not use within child action cache!");
            //bool skipAuthorization
            //    = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
            //    || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);
            //if (skipAuthorization)
            //    return;
            string controllerNamespace = filterContext.Controller.GetType().Namespace;
            if (AuthorizeCore(filterContext))
            {
                // ** IMPORTANT **
                // Since we're performing authorization at the action level, the authorization code runs
                // after the output caching module. In the worst case this could allow an authorized user
                // to cause the page to be cached, then an unauthorized user would later be served the
                // cached page. We work around this by telling proxies not to cache the sensitive page,
                // then we hook our custom authorization code into the caching mechanism so that we have
                // the final say on whether a page should be served from the cache.
                HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
                cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                cachePolicy.AddValidationCallback(_CacheValidateHandler, filterContext /* data */);
            }
            else
                HandleUnauthorizedRequest(filterContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpStatusCodeResult((int)this.HttpStatusCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext, AuthorizationContext filterContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");
            return AuthorizeCore(filterContext) ? HttpValidationStatus.Valid : HttpValidationStatus.IgnoreThisRequest;
        }

        #region

        private void _CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context), data as AuthorizationContext);
        }

        #endregion
    }
}
