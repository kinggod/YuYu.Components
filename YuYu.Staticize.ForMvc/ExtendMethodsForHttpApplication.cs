using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtendMethodsForHttpApplication
    {

        /// <summary>
        /// 注册RouteForStatic
        /// </summary>
        /// <param name="httpApplication"></param>
        /// <param name="directoryParameterName">文件夹参数名称</param>
        /// <param name="cycleMode">静态文件生成周期（MM：每月更新；WW：每周更新；DD：每日更新）</param>
        public static void RegisterStaticizeRoutes(this HttpApplication httpApplication, string directoryParameterName = "staticizeDirectory", string cycleMode = "DD")
        {
            YuYuMvcStaticizeConfigurationManager.RegisterStaticizeRoutes(directoryParameterName, cycleMode);
        }

        /// <summary>
        /// 在 HttpApplication 或其派生类的 Application_Error 中调用此方法以生成静态页
        /// </summary>
        /// <param name="httpApplication"></param>
        /// <param name="htmlFileDirectoryName">用于存放生成的html文件的文件夹</param>
        /// <param name="cycleMode"></param>
        public static void Staticize(this HttpApplication httpApplication, string htmlFileDirectoryName = "html", string cycleMode = "DD")
        {
            HttpContext context = httpApplication.Context;
            HttpServerUtility server = httpApplication.Server;
            HttpRequest request = httpApplication.Request;
            HttpResponse response = httpApplication.Response;
            if (!"GET".Equals(request.HttpMethod.ToUpper()))//非GET请求则退出
                return;
            if (YuYuMvcStaticizeConfigurationManager.StaticizeRoutes.Count == 0)//没有配置静态化路由则退出
                return;
            HttpException httpException = server.GetLastError() as HttpException;
            if (httpException == null)//异常为空则退出
                return;
            if (httpException.GetHttpCode() != 404)//非404错误则退出
                return;
            string url = request.Url.ToString(),
                pathAndQuery = request.Url.PathAndQuery,
                path = request.Path,
                dateFolderName = DateTime.Now.GetDirectoryName();
            UrlHelper urlHelper = new UrlHelper(request.RequestContext, RouteTable.Routes);
            context.RewritePath(pathAndQuery.Replace("/" + htmlFileDirectoryName + "/" + dateFolderName, string.Empty));
            RouteData data = YuYuMvcStaticizeConfigurationManager.StaticizeRoutes.GetRouteData(new HttpContextWrapper(context));
            if (data == null)//路由数据为空则退出
                return;
            string actionName = data.Values["action"] as string, controllerName = data.Values["controller"] as string;
            if (string.IsNullOrWhiteSpace(actionName) || string.IsNullOrWhiteSpace(controllerName))//处理映射程序不存在则退出
                return;
            string urlstring = urlHelper.Action(actionName, controllerName, data.Values),//映射到程序的Url
                filePath = server.MapPath("~" + path),//请求的文件路径
                content = string.Empty;
            string directoryPath = filePath.Remove(filePath.LastIndexOf("\\"));
            if (!Directory.Exists(directoryPath))
            {
                string htmlFolderPath = server.MapPath("~/" + htmlFileDirectoryName + "/");
                try
                {
                    if (Directory.Exists(htmlFolderPath))
                    {
                        string[] directories = Directory.GetDirectories(htmlFolderPath);
                        new Thread(() =>
                        {
                            if (directories == null || directories.Length == 0)
                                return;
                            for (int i = 0; i < directories.Length; i++)
                            {
                                Directory.Delete(directories[i], true);
                            }
                        }).Start();
                    }
                    Directory.CreateDirectory(directoryPath);
                }
                catch { }
            }
            if (Directory.Exists(directoryPath))
                try
                {
                    using (Stream responseSteam = WebRequest.Create(url.Replace(pathAndQuery, string.Empty) + urlstring).GetResponse().GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(responseSteam);
                        content = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                        responseSteam.Close();
                    }
                    if (content == null || content.Length == 0)
                        return;
                    using (FileStream fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        StreamWriter sw = new StreamWriter(fileStream);
                        sw.Write(content);
                        sw.Close();
                        sw.Dispose();
                        fileStream.Close();
                        fileStream.Dispose();
                    }
                }
                catch (Exception)
                {
                    return;
                }
            if (File.Exists(filePath))
                response.Redirect(request.Url.ToString());
        }
    }
}
