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
    /// ExtendMethodsForHttp
    /// </summary>
    internal static class ExtendMethodsForHttp
    {
        /// <summary>
        /// 设置 “WebRequest” 的请求数据
        /// </summary>
        /// <param name="webRequest"></param>
        /// <param name="parameters"></param>
        /// <param name="encoding"></param>
        /// <param name="httpMethod"></param>
        public static void SetData(this WebRequest webRequest, IDictionary<string, string> parameters, Encoding encoding = null, string httpMethod = "POST")
        {
            if (webRequest != null)
            {
                encoding = encoding ?? Encoding.ASCII;
                StringBuilder stringBuilder = new StringBuilder();
                if (parameters != null)
                    foreach (string key in parameters.Keys)
                    {
                        stringBuilder.AppendFormat("&{0}={1}", key, parameters[key] ?? string.Empty);
                    }
                byte[] data = encoding.GetBytes(stringBuilder.ToString());
                webRequest.SetData(data, "application/x-www-form-urlencoded", httpMethod);
            }
        }

        /// <summary>
        /// 设置 “WebRequest” 的请求数据
        /// </summary>
        /// <param name="webRequest"></param>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="httpMethod"></param>
        public static void SetData(this WebRequest webRequest, byte[] data, string contentType, string httpMethod = "POST")
        {
            if (webRequest != null && data != null)
            {
                webRequest.Method = httpMethod;
                webRequest.ContentType = contentType;
                webRequest.ContentLength = data.Length;
                using (Stream stream = webRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// 从 “WebResponse” 中读取输出数据！
        /// </summary>
        /// <param name="webResponse"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetData(this WebResponse webResponse, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            string data = string.Empty;
            if (webResponse != null)
                try
                {
                    using (Stream stream = webResponse.GetResponseStream())
                    {
                        StreamReader streamReader = new StreamReader(stream, encoding);
                        data = streamReader.ReadToEnd();
                        streamReader.Close();
                        streamReader.Dispose();
                        stream.Close();
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("从 “WebResponse” 中读取输出数据失败！", e);
                }
                finally
                {
                    webResponse.Close();
                }
            return data;
        }
    }
}
