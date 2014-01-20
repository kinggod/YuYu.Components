using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace YuYu.Components
{
    /// <summary>
    /// 对 HttpRequest 类型的扩展
    /// </summary>
    public static class ExtendMethodsForHttpRequest
    {
        /// <summary>
        /// 获取客户端IPv4地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string ClientIP(this HttpRequest request)
        {
            string result = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrWhiteSpace(result))
            {
                //可能有代理 
                if (result.IndexOf('.') == -1)    //没有“.”肯定是非IPv4格式 
                    result = null;
                else
                {
                    if (result.IndexOf(',') != -1)
                    {
                        //有“,”，估计多个代理。取第一个不是内网的IP。 
                        result = result.Replace(" ", string.Empty).Replace("'", string.Empty);
                        string[] temparyip = result.Split(",;".ToCharArray());
                        for (int i = 0; i < temparyip.Length; i++)
                        {
                            if (temparyip[i].IsIPv4() && temparyip[i].Substring(0, 3) != "10." && temparyip[i].Substring(0, 7) != "192.168" && temparyip[i].Substring(0, 7) != "172.16.")
                                return temparyip[i];    //找到不是内网的地址 
                        }
                    }
                    else if (result.IsIPv4()) //代理即是IP格式 
                        return result;
                    else
                        result = null;    //代理中的内容 非IP，取IP 
                }
            }
            if (string.IsNullOrWhiteSpace(result))
                result = request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrWhiteSpace(result))
                result = request.UserHostAddress;
            return result;
        }

        /// <summary>
        /// 客户端浏览器支持的压缩类型
        /// </summary>
        /// <param name="request">客户端请求</param>
        /// <returns></returns>
        public static CompressionType SupportCompression(this HttpRequest request)
        {
            string acceptEncoding = request.Headers["Accept-Encoding"];
            if (string.IsNullOrEmpty(acceptEncoding))
                return CompressionType.None;
            acceptEncoding = acceptEncoding.ToUpper();
            if (acceptEncoding.Contains("GZIP"))
                return CompressionType.GZip;
            else if (acceptEncoding.Contains("DEFLATE"))
                return CompressionType.Deflate;
            else
                return CompressionType.None;
        }
    }
}
