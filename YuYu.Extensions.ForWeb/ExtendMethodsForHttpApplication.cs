using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Routing;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtendMethodsForHttpApplication
    {

        /// <summary>
        /// 在 HttpApplication 或其派生类的 Application_BeginRequest 中调用此方法以压缩输出到客户端的 Response 流
        /// </summary>
        /// <param name="httpApplication"></param>
        /// <param name="compressFilePathExtension">可压缩输出流的请求后缀，默认“html|htm|css|js”</param>
        /// <param name="filterWhiteSpace">是否过滤空白、换行、制表符等字符串，默认为否</param>
        public static void CompressResponse(this HttpApplication httpApplication, string compressFilePathExtension = "html|htm|css|js", bool filterWhiteSpace = false)
        {
            HttpRequest request = httpApplication.Request;
            HttpResponse response = httpApplication.Response;
            string currentExecutionFilePathExtension = httpApplication.Request.CurrentExecutionFilePathExtension;
            if (string.IsNullOrWhiteSpace(currentExecutionFilePathExtension) || (!string.IsNullOrWhiteSpace(compressFilePathExtension) && Regex.IsMatch(currentExecutionFilePathExtension, @"\.(" + compressFilePathExtension + ")", RegexOptions.IgnoreCase)))
                try
                {
                    CompressionType compressionType = request.SupportCompression();
                    if (compressionType != CompressionType.None)
                    {
                        response.AppendHeader("Content-Encoding", compressionType.ToString().ToLower());
                        switch (compressionType)
                        {
                            case CompressionType.GZip:
                                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress, true);
                                break;
                            case CompressionType.Deflate:
                                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress, true);
                                break;
                            case CompressionType.None:
                            default:
                                break;
                        }
                    }
                    if (filterWhiteSpace)
                        response.Filter = new YuYuFilter(response.Filter, request.CurrentExecutionFilePathExtension);
                }
                catch { }
        }

        /// <summary>
        /// 多语言支持（在 HttpApplication 或其派生类的 Application_BeginRequest 中调用此方法检索当前请求的区域语言的名称或值，并重置当前线程的区域语言）
        /// 检索顺序：1、路由参数，2、查询字符串，3、Cookie
        /// </summary>
        /// <param name="httpApplication"></param>
        /// <param name="cultureParameterName">用于索引区域语言的名称或值的KEY</param>
        /// <param name="defaultCulture">未检索到当前请求的区域语言时呈现的默认区域语言，默认为null则自动感知请求所支持的语言</param>
        public static void MultipleLanguage(this HttpApplication httpApplication, string cultureParameterName, CultureInfo defaultCulture = null)
        {
            HttpRequest request = httpApplication.Request;
            RouteData routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpApplication.Context));
            string culture = routeData.Values[cultureParameterName] as string;
            if (culture == null)
                culture = request.QueryString[cultureParameterName];
            if (culture == null)
            {
                HttpCookie cultureCookie = request.Cookies[cultureParameterName];
                if (cultureCookie != null)
                    culture = cultureCookie.Value;
            }
            CultureInfo cultureInfo = null;
            if (string.IsNullOrWhiteSpace(culture))
            {
                if (defaultCulture == null)
                    if (request.UserLanguages.Length > 0)
                        defaultCulture = new CultureInfo(request.UserLanguages[0]);
                cultureInfo = defaultCulture;
            }
            else
                cultureInfo = Regex.IsMatch(culture, @"\d+") ? new CultureInfo(int.Parse(culture)) : cultureInfo = new CultureInfo(culture);
            if (cultureInfo != null)
            {
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }
        }
    }
}
