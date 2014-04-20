using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace YuYu.Components
{
    /// <summary>
    /// ExtendMethods
    /// </summary>
    internal static class ExtendMethods
    {
        /// <summary>
        /// 取平台的字符串值
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        public static string GetString(this Platform platform)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (platform.Contains(Platform.Android))
                stringBuilder.AppendFormat("{0},", Platform.Android.ToString().ToLowerInvariant());
            if (platform.Contains(Platform.iOS))
                stringBuilder.AppendFormat("{0},", Platform.iOS.ToString().ToLowerInvariant());
            return stringBuilder.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 从 “HttpWebRequest” 中读取“WebResponse”输出数据！
        /// </summary>
        /// <param name="httpWebRequest"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetResponseData(this HttpWebRequest httpWebRequest, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            if (httpWebRequest != null)
            {
                WebResponse webResponse = httpWebRequest.GetResponse();
                return webResponse.GetOutputData(encoding);
            }
            return string.Empty;
        }

        /// <summary>
        /// 从 “WebResponse” 中读取输出数据！
        /// </summary>
        /// <param name="webResponse"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetOutputData(this WebResponse webResponse, Encoding encoding = null)
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

        /// <summary>
        /// 获取传入的 HTTP 实体主体的内容
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public static byte[] GetInputData(this HttpRequest httpRequest)
        {
            if (httpRequest != null)
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    httpRequest.InputStream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            return null;
        }

        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="originalInput"></param>
        /// <returns></returns>
        public static string UrlEncode(this string originalInput)
        {
            return HttpUtility.UrlEncode(originalInput, Encoding.UTF8);
        }

        /// <summary>
        /// MD5编码
        /// </summary>
        /// <param name="originalInput"></param>
        /// <returns></returns>
        public static string MD5Encode(this string originalInput)
        {
            return MD5Encode(Encoding.Default.GetBytes(originalInput)).ToUpperInvariant();
        }


        /// <summary>
        /// MD5编码
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string MD5Encode(this byte[] bytes)
        {
            using (MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider())
            {
                return BitConverter.ToString(md5CryptoServiceProvider.ComputeHash(bytes)).Replace("-", string.Empty).ToUpperInvariant();
            }
        }

        /// <summary>
        /// Checks the null reference.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <exception cref="System.NullReferenceException">Object [ + objectIdentity.GetStringValue() + ] is null.</exception>
        public static void CheckNullReference(this object anyObject, string objectIdentity = null)
        {
            if (anyObject == null)
            {
                throw new NullReferenceException("Object [" + (objectIdentity ?? string.Empty) + "] is null.");
            }
        }

        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="originalInput"></param>
        /// <param name="encoding">默认为 Encoding.UTF8</param>
        /// <returns></returns>
        public static string ToBase64String(this string originalInput, Encoding encoding = null)
        {
            if (originalInput != null)
                return Convert.ToBase64String((encoding ?? Encoding.UTF8).GetBytes(originalInput));
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool Contains(this Platform platform, Platform other)
        {
            return other == (platform & other);
        }
    }
}
