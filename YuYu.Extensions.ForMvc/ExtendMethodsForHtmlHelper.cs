using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Collections;
using System.Globalization;
using System.Web.WebPages;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtendMethodsForHtmlHelper
    {
        /// <summary>
        /// 获取ID
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string ID<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            return htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);
        }

        /// <summary>
        /// 创建 Meta-Charset 标签
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static MvcHtmlString Meta(this HtmlHelper htmlHelper, Encoding encoding)
        {
            TagBuilder tag = new TagBuilder("meta");
            tag.MergeAttribute("charset", encoding.WebName);
            return tag._ToMvcHtmlString(TagRenderMode.SelfClosing);
        }

        /// <summary>
        /// 创建 Meta-Name 标签
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public static MvcHtmlString Meta(this HtmlHelper htmlHelper, MetaName name, string content, string scheme = null)
        {
            return _Meta(htmlHelper, name, null, content, scheme);
        }

        /// <summary>
        /// 创建 Meta-HttpEquiv 标签
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="httpEquiv"></param>
        /// <param name="content"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public static MvcHtmlString Meta(this HtmlHelper htmlHelper, MetaHttpEquiv httpEquiv, string content, string scheme = null)
        {
            return _Meta(htmlHelper, null, httpEquiv, content, scheme);
        }

        /// <summary>
        /// 文件上传控件
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <param name="className"></param>
        /// <param name="selectText"></param>
        /// <returns></returns>
        public static MvcHtmlString FileBox(this HtmlHelper htmlHelper, string expression, string value, string className, string selectText)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");
            var sb = new StringBuilder();
            sb.Append(htmlHelper.TextBox(expression, value, new { @class = className, @readonly = "readonly" }).ToString());
            _AppendSpan(className, expression, selectText, sb);
            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// 文件上传控件
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="className"></param>
        /// <param name="selectText"></param>
        /// <returns></returns>
        public static MvcHtmlString FileBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, string className, string selectText)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var id = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);
            var sb = new StringBuilder();
            sb.Append(htmlHelper.TextBoxFor(expression, new { @class = className, @readonly = "readonly" }).ToString());
            _AppendSpan(className, id, selectText, sb);
            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// 创建一个具有分组下拉选项的下拉框
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="selectList"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, IDictionary<string, IEnumerable<SelectListItem>> selectList, object htmlAttributes = null)
        {
            return htmlHelper.DropDownListFor(expression, selectList.ToDictionary(m => m.Key, m => m.Value.ToDictionary(mm => mm, mm => null as object) as IDictionary<SelectListItem, object>), htmlAttributes);
        }

        /// <summary>
        /// 创建一个具有分组下拉选项的下拉框
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="selectList"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, IDictionary<string, IDictionary<SelectListItem, object>> selectList, object htmlAttributes = null)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");
            bool allowMultiple = false;
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText(expression);
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (string.IsNullOrEmpty(fullName))
                throw new ArgumentException("未能获取控件name值！", "name");
            bool usedViewData = false;
            // If we got a null selectList, try to use ViewData to get the list of items.
            if (selectList == null)
            {
                object o = null;
                if (htmlHelper.ViewData != null)
                    o = htmlHelper.ViewData.Eval(name);
                if (o == null)
                    throw new InvalidOperationException("无效的下拉项集合！");
                selectList = o as IDictionary<string, IDictionary<SelectListItem, object>>;
                if (selectList == null)
                    throw new InvalidOperationException("无效的下拉项集合！");
                usedViewData = true;
            }
            object defaultValue = allowMultiple ? _GetModelStateValue(htmlHelper, fullName, typeof(string[])) : _GetModelStateValue(htmlHelper, fullName, typeof(string));
            // If we haven't already used ViewData to get the entire list of items then we need to
            // use the ViewData-supplied value before using the parameter-supplied value.
            if (defaultValue == null && !String.IsNullOrEmpty(name))
            {
                if (!usedViewData)
                    defaultValue = htmlHelper.ViewData.Eval(name);
                else if (metadata != null)
                    defaultValue = metadata.Model;
            }
            if (defaultValue != null)
            {
                IEnumerable defaultValues;
                if (allowMultiple)
                {
                    defaultValues = defaultValue as IEnumerable;
                    if (defaultValues == null || defaultValues is string)
                        throw new InvalidOperationException("无效的下拉项集合！");
                }
                else
                    defaultValues = new[] { defaultValue };
                var values = from object value in defaultValues select Convert.ToString(value, CultureInfo.CurrentCulture);
                var enumValues = from Enum value in defaultValues.OfType<Enum>() select value.ToString("d");
                values = values.Concat(enumValues);
                var selectedValues = new HashSet<string>(values, StringComparer.OrdinalIgnoreCase);
                foreach (var keyValuePair in selectList)
                    foreach (var item in keyValuePair.Value)
                        item.Key.Selected = (item.Key.Value != null) ? selectedValues.Contains(item.Key.Value) : selectedValues.Contains(item.Key.Text);
            }
            var listItemGroupBuilder = new StringBuilder();
            foreach (var keyValuePair in selectList)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in keyValuePair.Value)
                {
                    TagBuilder optionTagBuilder = new TagBuilder("option") { InnerHtml = HttpUtility.HtmlEncode(item.Key.Text) };
                    if (item.Key.Value != null)
                        optionTagBuilder.Attributes["value"] = item.Key.Value;
                    if (item.Key.Selected)
                        optionTagBuilder.Attributes["selected"] = "selected";
                    if (item.Value != null)
                        optionTagBuilder.MergeAttributes(_GetAttributes(item.Value));
                    stringBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
                }
                TagBuilder optgroupTagBuilder = new TagBuilder("optgroup") { InnerHtml = stringBuilder.ToString() };
                optgroupTagBuilder.Attributes["label"] = keyValuePair.Key;
                listItemGroupBuilder.AppendLine(optgroupTagBuilder.ToString(TagRenderMode.Normal));
            }
            TagBuilder selectTagBuilder = new TagBuilder("select") { InnerHtml = listItemGroupBuilder.ToString() };
            selectTagBuilder.MergeAttributes(_GetAttributes(htmlAttributes));
            selectTagBuilder.MergeAttribute("name", fullName, true);
            selectTagBuilder.GenerateId(fullName);
            if (allowMultiple)
                selectTagBuilder.MergeAttribute("multiple", "multiple");
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState))
                if (modelState.Errors.Count > 0)
                    selectTagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            selectTagBuilder.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata));
            return selectTagBuilder._ToMvcHtmlString(TagRenderMode.Normal);
        }

        /// <summary>
        /// 创建一个具有丰富特性的按钮
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="button">按钮的类型</param>
        /// <param name="value">显示的文本</param>
        /// <param name="htmlAttributes">丰富的特性</param>
        /// <returns></returns>
        public static MvcHtmlString Button(this HtmlHelper htmlHelper, Button button, string value, object htmlAttributes = null)
        {
            var tag = new TagBuilder("input");
            tag.MergeAttribute("type", button.ToString().ToLower(), true);
            tag.MergeAttribute("value", value, true);
            tag.MergeAttributes(_GetAttributes(htmlAttributes), true);
            return tag._ToMvcHtmlString(TagRenderMode.SelfClosing);
        }

        /// <summary>
        /// 创建一个Image标签对象
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="src">源位置</param>
        /// <param name="alt">提示</param>
        /// <param name="title">鼠标提示</param>
        /// <returns></returns>
        public static MvcHtmlString Img(this HtmlHelper htmlHelper, string src, string alt, string title = null)
        {
            return Img(htmlHelper, src, alt, title, null);
        }

        /// <summary>
        /// 创建一个具有丰富特性的Image标签对象
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="src">源位置</param>
        /// <param name="alt">提示</param>
        /// <param name="title">鼠标提示</param>
        /// <param name="htmlAttributes">丰富的特性</param>
        /// <returns></returns>
        public static MvcHtmlString Img(this HtmlHelper htmlHelper, string src, string alt, string title, object htmlAttributes = null)
        {
            UrlHelper urlHelper = ((WebViewPage)WebPageContext.Current.Page).Url;
            TagBuilder tagBuilder = new TagBuilder("img");
            tagBuilder.MergeAttribute("src", urlHelper.Content(src), true);
            tagBuilder.MergeAttribute("alt", alt, true);
            tagBuilder.MergeAttribute("title", title, true);
            tagBuilder.MergeAttributes(_GetAttributes(htmlAttributes), true);
            return tagBuilder._ToMvcHtmlString(TagRenderMode.SelfClosing);
        }

        /// <summary>
        /// 创建一个普通分页控件
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="actionName">行为</param>
        /// <param name="controllerName">控制器</param>
        /// <param name="p">分页数据封装类实例</param>
        /// <param name="morePN">当前页左右侧各显示更多页码</param>
        /// <param name="currPNAttributes">当前页码html特性</param>
        /// <param name="noLinkAttributes">无链接页码html特性</param>
        /// <param name="htmlAttributes">链接页码html特性</param>
        /// <param name="firstPT">首页链接文本</param>
        /// <param name="lastPT">末页链接文本</param>
        /// <param name="prevPT">上一页链接文本</param>
        /// <param name="nextPT">下一页链接文本</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper htmlHelper, string actionName, string controllerName, Pager p, int morePN, object currPNAttributes, object noLinkAttributes, object htmlAttributes, string firstPT, string lastPT, string prevPT, string nextPT)
        {
            return Pager(htmlHelper, actionName, controllerName, p, morePN, null, currPNAttributes, noLinkAttributes, htmlAttributes, firstPT, lastPT, prevPT, nextPT);
        }

        /// <summary>
        /// 创建一个带路由参数的分页控件
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="actionName">行为</param>
        /// <param name="controllerName">控制器</param>
        /// <param name="pager">分页数据封装类实例</param>
        /// <param name="morePN">当前页左右侧各显示更多页码</param>
        /// <param name="parameters">路由参数</param>
        /// <param name="currPNAttributes">当前页码html特性</param>
        /// <param name="noLinkAttributes">无链接页码html特性</param>
        /// <param name="htmlAttributes">链接页码html特性</param>
        /// <param name="firstPT">首页链接文本</param>
        /// <param name="lastPT">末页链接文本</param>
        /// <param name="prevPT">上一页链接文本</param>
        /// <param name="nextPT">下一页链接文本</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper htmlHelper, string actionName, string controllerName, Pager pager, int morePN, object parameters, object currPNAttributes, object noLinkAttributes, object htmlAttributes, string firstPT, string lastPT, string prevPT, string nextPT)
        {
            UrlHelper urlHelper = ((WebViewPage)WebPageContext.Current.Page).Url;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<ul>");
            if (pager.CurrNo > 1)
            {
                if (!string.IsNullOrEmpty(firstPT))
                    _AddItem(firstPT, urlHelper, actionName, controllerName, new { s = pager.PageSize, n = 1 }, parameters, htmlAttributes, sb, false);
                if (!string.IsNullOrEmpty(prevPT))
                    _AddItem(prevPT, urlHelper, actionName, controllerName, new { s = pager.PageSize, n = pager.CurrNo - 1 }, parameters, htmlAttributes, sb, false);
            }
            else
            {
                if (!string.IsNullOrEmpty(firstPT))
                    _AddItem(firstPT, noLinkAttributes, sb);
                if (!string.IsNullOrEmpty(prevPT))
                    _AddItem(prevPT, noLinkAttributes, sb);
            }
            if (morePN < 1)
                morePN = 1;
            if (pager.CurrNo - morePN > 1)
                _AddItem("1", urlHelper, actionName, controllerName, new { s = pager.PageSize, n = 1 }, parameters, htmlAttributes, sb, false);
            if (pager.CurrNo - morePN > 2)
                _AddItem("...", noLinkAttributes, sb);
            for (int i = pager.CurrNo - morePN; i <= pager.CurrNo + morePN; i++)
            {
                if (i < 1 || i > pager.PageCount)
                    continue;
                if (i == pager.CurrNo)
                {
                    _AddItem(i.ToString(), urlHelper, actionName, controllerName, new { s = pager.PageSize, n = i }, parameters, currPNAttributes, sb, false);
                    continue;
                }
                _AddItem(i.ToString(), urlHelper, actionName, controllerName, new { s = pager.PageSize, n = i }, parameters, htmlAttributes, sb, false);
            }
            if (pager.CurrNo + morePN + 1 < pager.PageCount)
                _AddItem("...", noLinkAttributes, sb);
            if (pager.CurrNo + morePN < pager.PageCount)
                _AddItem(pager.PageCount.ToString(), urlHelper, actionName, controllerName, new { s = pager.PageSize, n = pager.PageCount }, parameters, htmlAttributes, sb, false);
            if (pager.CurrNo < pager.PageCount)
            {
                if (!string.IsNullOrEmpty(nextPT))
                    _AddItem(nextPT, urlHelper, actionName, controllerName, new { s = pager.PageSize, n = pager.CurrNo + 1 }, parameters, htmlAttributes, sb, false);
                if (!string.IsNullOrEmpty(lastPT))
                    _AddItem(lastPT, urlHelper, actionName, controllerName, new { s = pager.PageSize, n = pager.PageCount }, parameters, htmlAttributes, sb, false);
            }
            else
            {
                if (!string.IsNullOrEmpty(nextPT))
                    _AddItem(nextPT, noLinkAttributes, sb);
                if (!string.IsNullOrEmpty(lastPT))
                    _AddItem(lastPT, noLinkAttributes, sb);
            }
            sb.AppendLine("</ul>");
            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// 创建一个外链样式
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="href">文件路径（ep."~/styles/style.css"）</param>
        /// <param name="noCache">是否缓存</param>
        /// <returns></returns>
        public static MvcHtmlString Link(this HtmlHelper htmlHelper, string href, bool noCache = false)
        {
            UrlHelper urlHelper = ((WebViewPage)WebPageContext.Current.Page).Url;
            TagBuilder tagBuilder = new TagBuilder("link");
            tagBuilder.MergeAttribute("type", "text/css", true);
            tagBuilder.MergeAttribute("rel", "stylesheet", true);
            tagBuilder.MergeAttribute("href", urlHelper.Content(href) + (noCache ? "?" + DateTime.Now.Ticks : string.Empty), true);
            return tagBuilder._ToMvcHtmlString(TagRenderMode.SelfClosing);
        }

        /// <summary>
        /// 创建一个外链脚本
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="src">文件路径（ep."~/scripts/script.js"）</param>
        /// <param name="charset">文件字符编码</param>
        /// <param name="noCache">是否缓存</param>
        /// <returns></returns>
        public static MvcHtmlString Script(this HtmlHelper htmlHelper, string src, string charset, bool noCache = false)
        {
            UrlHelper urlHelper = ((WebViewPage)WebPageContext.Current.Page).Url;
            TagBuilder tagBuilder = new TagBuilder("script");
            tagBuilder.MergeAttribute("type", "text/javascript", true);
            if (!string.IsNullOrWhiteSpace(charset))
                tagBuilder.MergeAttribute("charset", charset, true);
            tagBuilder.MergeAttribute("src", urlHelper.Content(src) + (noCache ? "?" + DateTime.Now.Ticks : string.Empty), true);
            return tagBuilder._ToMvcHtmlString(TagRenderMode.Normal);
        }

        #region ActionLink

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="requireAbsoluteUrl"></param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, null, new RouteValueDictionary(), new RouteValueDictionary(), requireAbsoluteUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="routeValues"></param>
        /// <param name="requireAbsoluteUrl"></param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, null, new RouteValueDictionary(routeValues), new RouteValueDictionary(), requireAbsoluteUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="requireAbsoluteUrl"></param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary(), requireAbsoluteUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="routeValues"></param>
        /// <param name="requireAbsoluteUrl"></param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, null, routeValues, new RouteValueDictionary(), requireAbsoluteUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="requireAbsoluteUrl"></param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, null, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes), requireAbsoluteUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="requireAbsoluteUrl"></param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, null, routeValues, htmlAttributes, requireAbsoluteUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="requireAbsoluteUrl"></param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, controllerName, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes), requireAbsoluteUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="requireAbsoluteUrl"></param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, bool requireAbsoluteUrl)
        {
            if (requireAbsoluteUrl)
            {
                HttpContextBase currentContext = new HttpContextWrapper(HttpContext.Current);
                RouteData routeData = RouteTable.Routes.GetRouteData(currentContext);
                routeData.Values["controller"] = controllerName;
                routeData.Values["action"] = actionName;
                DomainRoute domainRoute = routeData.Route as DomainRoute;
                if (domainRoute != null)
                {
                    DomainData domain = domainRoute.GetDomainData(new RequestContext(currentContext, routeData), routeData.Values);
                    return htmlHelper.ActionLink(linkText, actionName, controllerName, domain.Protocol, domain.HostName, string.Empty, routeData.Values, null);
                }
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        }

        #endregion

        #region

        private static MvcHtmlString _Meta(HtmlHelper htmlHelper, MetaName? name, MetaHttpEquiv? httpEquiv, string content, string scheme = null)
        {
            TagBuilder tagBuilder = new TagBuilder("meta");
            if (name.HasValue)
                tagBuilder.MergeAttribute("name", name.Value.ToString().Replace('_', '-'));
            if (httpEquiv.HasValue)
                tagBuilder.MergeAttribute("http-equiv", httpEquiv.Value.ToString().Replace('_', '-'));
            tagBuilder.MergeAttribute("content", content);
            if (!string.IsNullOrWhiteSpace(scheme))
                tagBuilder.MergeAttribute("scheme", scheme);
            return tagBuilder._ToMvcHtmlString(TagRenderMode.SelfClosing);
        }

        private static object _GetModelStateValue<TModel>(HtmlHelper<TModel> htmlHelper, string key, Type destinationType)
        {
            object result = null;
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(key, out modelState))
                result = modelState.Value.ConvertTo(destinationType, null);
            return result;
        }

        private static void _AppendSpan(string className, string id, string selectText, StringBuilder stringBuilder)
        {
            TagBuilder tagBuilder = new TagBuilder("span");
            tagBuilder.AddCssClass(className);
            TagBuilder fileTagBuilder = new TagBuilder("input");
            fileTagBuilder.MergeAttribute("type", "file", true);
            fileTagBuilder.MergeAttribute("name", id + "_FileInput", true);
            fileTagBuilder.MergeAttribute("id", id + "_FileInput", true);
            tagBuilder.InnerHtml = selectText + fileTagBuilder.ToString(TagRenderMode.SelfClosing);
            stringBuilder.Append(tagBuilder.ToString(TagRenderMode.Normal));
        }

        private static void _AddItem(string innerText, UrlHelper urlHelper, string actionName, string controllerName, object pagerParams, object parameters, object htmlAttributes, StringBuilder stringBuilder, bool ajax)
        {
            RouteValueDictionary routValues = urlHelper.RequestContext.RouteData.Values;
            if (pagerParams != null)
                foreach (PropertyInfo propertyInfo in pagerParams.GetType().GetProperties())
                {
                    if (routValues.ContainsKey(propertyInfo.Name))
                        routValues.Remove(propertyInfo.Name);
                    routValues.Add(propertyInfo.Name, propertyInfo.GetValue(pagerParams, null));
                }
            if (parameters != null)
                foreach (PropertyInfo propertyInfo in parameters.GetType().GetProperties())
                {
                    if (routValues.ContainsKey(propertyInfo.Name))
                        routValues.Remove(propertyInfo.Name);
                    routValues.Add(propertyInfo.Name, propertyInfo.GetValue(parameters, null));
                }
            string href = urlHelper.Action(actionName, controllerName, routValues);
            TagBuilder liTagBuilder = new TagBuilder("li");
            TagBuilder aTagBuilder = new TagBuilder("a");
            aTagBuilder.MergeAttribute("href", href, true);
            aTagBuilder.SetInnerText(innerText);
            liTagBuilder.InnerHtml = aTagBuilder.ToString(TagRenderMode.Normal);
            liTagBuilder.MergeAttributes(_GetAttributes(htmlAttributes), true);
            stringBuilder.AppendLine(liTagBuilder.ToString(TagRenderMode.Normal));
        }

        private static void _AddItem(string innerText, object htmlAttributes, StringBuilder stringBuilder)
        {
            TagBuilder tagBuilder = new TagBuilder("li");
            tagBuilder.MergeAttributes(_GetAttributes(htmlAttributes), true);
            tagBuilder.SetInnerText(innerText);
            stringBuilder.Append(tagBuilder.ToString(TagRenderMode.Normal));
        }

        private static IDictionary<string, object> _GetAttributes(object htmlAttributes)
        {
            IDictionary<string, object> attributes = new Dictionary<string, object>();
            if (htmlAttributes != null)
                foreach (PropertyInfo propertyInfo in htmlAttributes.GetType().GetProperties())
                {
                    if (attributes.ContainsKey(propertyInfo.Name))
                        attributes.Remove(propertyInfo.Name);
                    object value = propertyInfo.GetValue(htmlAttributes, null);
                    if (value != null)
                        attributes.Add(propertyInfo.Name, value);
                }
            return attributes;
        }

        private static MvcHtmlString _ToMvcHtmlString(this TagBuilder tagBuilder, TagRenderMode tagRenderMode)
        {
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }

        #endregion
    }
}
