using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YuYu.Components
{
    /// <summary>
    /// JPushClient
    /// </summary>
    public class JPushClient
    {
        /// <summary>
        /// The remote base URL format
        /// </summary>
        public const string APIBASEURLFORMAT = "{0}api.jpush.cn:{1}/v2/push";

        /// <summary>
        /// The report base URL format
        /// </summary>
        public const string REPORTBASEURLFORMAT = "https://report.jpush.cn/v2/received";

        /// <summary>
        /// 协议
        /// </summary>
        public string Protocol { get; protected set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; protected set; }

        /// <summary>
        /// AppKey
        /// </summary>
        public string AppKey { get; protected set; }

        /// <summary>
        /// MasterSecret
        /// </summary>
        public string MasterSecret { get; protected set; }

        /// <summary>
        /// ApiBaseUrl
        /// </summary>
        public string ApiBaseUrl
        {
            get
            {
                return string.Format(APIBASEURLFORMAT, this.Protocol, this.Port);
            }
        }

        /// <summary>
        /// 初始化JPushClient
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="masterSecret"></param>
        /// <param name="useSSL"></param>
        public JPushClient(string appKey, string masterSecret, bool useSSL = true)
        {
            this.AppKey = appKey;
            this.MasterSecret = masterSecret;
            this.Protocol = useSSL ? "https://" : "http://";
            this.Port = useSSL ? 443 : 8800;
        }

        /// <summary>
        /// 发送推送
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response Push(Request request)
        {
            Response response = new Response();
            WebResponse webResponse = null;
            try
            {
                WebRequest webRequest = WebRequest.Create(this.ApiBaseUrl);
                SetCredential(webRequest);
                IDictionary<string, string> dictionary = request.GetDictionary();
                dictionary["app_key"] = this.AppKey;
                dictionary["verification_code"] = request.GetVerificationCode(this.MasterSecret);
                webRequest.SetData(dictionary, Encoding.UTF8);
                webResponse = webRequest.GetResponse();
                string responseContent = webResponse.GetData();
                JToken root = JToken.Parse(responseContent);
                response.ResponseCode = (ResponseCode)root.SelectToken("errcode").Value<Int32>();
                response.ResponseMessage = root.SelectToken("errmsg").Value<string>();
                if (response.ResponseCode == ResponseCode.Succeed)
                {
                    response.MessageID = root.SelectToken("msg_id").Value<string>();
                    response.SendNo = root.SelectToken("sendno").Value<string>();
                }
                return response;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed to send push message.", e);
            }
            finally
            {
                if (webResponse != null)
                    webResponse.Close();
            }
        }

        /// <summary>
        /// 检索推送结果
        /// </summary>
        /// <param name="messageIDs"></param>
        /// <returns></returns>
        public IList<Result> QueryResult(List<string> messageIDs)
        {
            // JPush has limitation officially. One query support no more than 100 IDs.
            int limitation = 100;
            List<Result> result = new List<Result>();
            string ids = string.Empty;
            if (messageIDs != null && messageIDs.Count > 0)
            {
                if (messageIDs.Count > limitation)
                    messageIDs.RemoveRange(limitation, messageIDs.Count - limitation);
                ids = string.Join(",", messageIDs);
            }
            if (!string.IsNullOrWhiteSpace(ids))
            {
                WebResponse webResponse = null;
                try
                {
                    WebRequest webRequest = (WebRequest)WebRequest.Create(REPORTBASEURLFORMAT + "?msg_ids=" + ids);
                    webRequest.Method = "GET";
                    webRequest.Headers[HttpRequestHeader.Authorization] = Convert.ToBase64String(Encoding.UTF8.GetBytes(this.AppKey + ":" + this.MasterSecret));
                    SetCredential(webRequest);
                    webResponse = webRequest.GetResponse();
                    string responseContent = webResponse.GetData();
                    result = JsonConvert.DeserializeObject<List<Result>>(responseContent);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Failed to QueryPushMessageStatus.", e);
                }
                finally
                {
                    if (webResponse != null)
                        webResponse.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// 设置Credentials
        /// </summary>
        /// <param name="webRequest"></param>
        protected void SetCredential(WebRequest webRequest)
        {
            if (webRequest != null)
                webRequest.Credentials = new NetworkCredential(this.AppKey, this.MasterSecret);
        }
    }
}
