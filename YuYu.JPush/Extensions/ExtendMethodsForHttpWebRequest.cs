using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace YuYu.Components
{
    /// <summary>
    /// ExtendMethodsForHttpWebRequest
    /// </summary>
    internal static class ExtendMethodsForHttpWebRequest
    {
        /// <summary>
        /// 设置 “HttpWebRequest” 的请求数据
        /// </summary>
        /// <param name="httpWebRequest"></param>
        /// <param name="parameters"></param>
        /// <param name="encoding"></param>
        /// <param name="httpMethod"></param>
        public static void SetRequestData(this HttpWebRequest httpWebRequest, IDictionary<string, string> parameters, Encoding encoding = null, string httpMethod = "POST")
        {
            if (httpWebRequest != null)
            {
                encoding = encoding ?? Encoding.ASCII;
                StringBuilder stringBuilder = new StringBuilder();
                if (parameters != null)
                    foreach (string key in parameters.Keys)
                    {
                        stringBuilder.AppendFormat("&{0}={1}", key, parameters[key] ?? string.Empty);
                    }
                byte[] data = encoding.GetBytes(stringBuilder.ToString());
                httpWebRequest.SetRequestData(data, "application/x-www-form-urlencoded", httpMethod);
            }
        }

        /// <summary>
        /// 设置 “HttpWebRequest” 的请求数据
        /// </summary>
        /// <param name="httpWebRequest"></param>
        /// <param name="parameters"></param>
        /// <param name="encoding"></param>
        /// <param name="httpMethod"></param>
        public static void SetRequestData(this HttpWebRequest httpWebRequest, string parameters, Encoding encoding = null, string httpMethod = "POST")
        {
            byte[] data = null;
            if (!string.IsNullOrWhiteSpace(parameters))
            {
                encoding = encoding ?? Encoding.UTF8;
                data = encoding.GetBytes(parameters);
            }
            httpWebRequest.SetRequestData(data, "text/xml; charset=utf-8", httpMethod);
        }

        /// <summary>
        /// 设置 “HttpWebRequest” 的请求数据
        /// </summary>
        /// <param name="httpWebRequest"></param>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="httpMethod"></param>
        public static void SetRequestData(this HttpWebRequest httpWebRequest, byte[] data, string contentType, string httpMethod = "POST")
        {
            if (httpWebRequest != null && data != null)
            {
                httpWebRequest.Method = httpMethod;
                httpWebRequest.ContentType = contentType;
                httpWebRequest.ContentLength = data.Length;
                using (Stream stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }
            }
        }
    }
}
