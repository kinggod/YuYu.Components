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
        /// <param name="pushRequest"></param>
        /// <returns></returns>
        public Response Push(Request pushRequest)
        {
            Response pushResponse = new Response();
            WebResponse response = null;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(this.ApiBaseUrl);
                SetCredential(httpWebRequest);
                IDictionary<string, string> dictionary = pushRequest.GetDictionary();
                dictionary["app_key"] = this.AppKey;
                dictionary["verification_code"] = pushRequest.GetVerificationCode(this.MasterSecret);
                httpWebRequest.SetRequestData(dictionary, Encoding.UTF8);
                response = httpWebRequest.GetResponse();
                string responseContent = response.GetOutputData();
                JToken root = JToken.Parse(responseContent);
                pushResponse.ResponseCode = (ResponseCode)root.SelectToken("errcode").Value<Int32>();
                pushResponse.ResponseMessage = root.SelectToken("errmsg").Value<string>();
                if (pushResponse.ResponseCode == ResponseCode.Succeed)
                {
                    pushResponse.MessageID = root.SelectToken("msg_id").Value<string>();
                    pushResponse.SendIdentity = root.SelectToken("sendno").Value<string>();
                }
                return pushResponse;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed to send push message.", e);
            }
            finally
            {
                if (response != null)
                    response.Close();
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
                WebResponse response = null;
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(REPORTBASEURLFORMAT + "?msg_ids=" + ids);
                    httpWebRequest.Method = "GET";
                    httpWebRequest.Headers[HttpRequestHeader.Authorization] = (this.AppKey + ":" + this.MasterSecret).ToBase64String();
                    SetCredential(httpWebRequest);
                    response = httpWebRequest.GetResponse();
                    string responseContent = response.GetOutputData();
                    result = JsonConvert.DeserializeObject<List<Result>>(responseContent);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Failed to QueryPushMessageStatus.", e);
                }
                finally
                {
                    if (response != null)
                        response.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// 设置Credentials
        /// </summary>
        /// <param name="httpWebRequest"></param>
        protected void SetCredential(HttpWebRequest httpWebRequest)
        {
            if (httpWebRequest != null)
                httpWebRequest.Credentials = new NetworkCredential(this.AppKey, this.MasterSecret);
        }
    }
}
