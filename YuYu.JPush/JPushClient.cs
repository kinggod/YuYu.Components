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
        public const string APIBASEURLFORMAT = "{0}api.jpush.cn:{1}/v2/";

        /// <summary>
        /// The report base URL format
        /// </summary>
        public const string REPORTBASEURLFORMAT = "https://report.jpush.cn/v2/";

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
            this.Port = useSSL ? 443 : 80;
        }

        /// <summary>
        /// 发送推送
        /// </summary>
        /// <param name="pushRequest"></param>
        /// <returns></returns>
        public PushResponse SendPush(PushRequest pushRequest)
        {
            PushResponse result = new PushResponse();
            WebResponse response = null;

            try
            {
                var httpRequest = CreateSendPushRequest(pushRequest);
                response = httpRequest.GetResponse();

                var responseContent = response.GetOutputData();

                JToken root = JToken.Parse(responseContent);

                result.ResponseCode = (ResponseCode)root.SelectToken("errcode").Value<Int32>();
                result.ResponseMessage = root.SelectToken("errmsg").Value<string>();

                if (result.ResponseCode == ResponseCode.Succeed)
                {
                    result.MessageID = root.SelectToken("msg_id").Value<string>();
                    result.SendIdentity = root.SelectToken("sendno").Value<string>();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to send push message.", ex);
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        /// <summary>
        /// 检索推送结果
        /// </summary>
        /// <param name="messageIDs"></param>
        /// <returns></returns>
        public IList<PushResult> QueryPushResult(List<string> messageIDs)
        {
            // JPush has limitation officially. One query support no more than 100 IDs.
            int limitation = 100;
            List<PushResult> result = new List<PushResult>();
            string ids = string.Empty;
            if (messageIDs != null && messageIDs.Count > 0)
            {
                if (messageIDs.Count > limitation)
                    messageIDs.RemoveRange(limitation, messageIDs.Count - limitation);
                ids = string.Join(",", messageIDs);
            }
            if (!string.IsNullOrWhiteSpace(ids))
            {
                HttpWebRequest httpWebRequest = this.CreateQueryPushResultRequest(ids);
                WebResponse response = null;
                try
                {
                    response = httpWebRequest.GetResponse();
                    string responseContent = response.GetOutputData();
                    result = JsonConvert.DeserializeObject<List<PushResult>>(responseContent);
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
        /// 创建发送推送的HttpWeb请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected HttpWebRequest CreateSendPushRequest(PushRequest request)
        {
            HttpWebRequest httpWebRequest = null;
            if (request != null)
            {
                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(this.ApiBaseUrl + "push");
                SetCredential(httpWebRequest);
                httpWebRequest.SetRequestData(GetDataDictionary(request), Encoding.UTF8);
            }
            return httpWebRequest;
        }

        /// <summary>
        /// 创建检索推送结果的HttpWeb请求
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        protected HttpWebRequest CreateQueryPushResultRequest(string ids)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(REPORTBASEURLFORMAT + "received?msg_ids=" + ids);
            httpRequest.Method = "GET";
            httpRequest.Headers[HttpRequestHeader.Authorization] = GetQueryToken(this.AppKey, this.MasterSecret);
            SetCredential(httpRequest);
            return httpRequest;
        }

        /// <summary>
        /// 获取数据字典
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected IDictionary<string, string> GetDataDictionary(PushRequest request)
        {
            if (request != null)
            {
                IDictionary<string, string> dictionary = request.GetDictionary();
                dictionary["app_key"] = this.AppKey;
                dictionary["verification_code"] = request.GetVerificationCode(this.MasterSecret);
            }
            return null;
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

        /// <summary>
        /// 获取QueryToken
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="masterSecret"></param>
        /// <returns></returns>
        protected string GetQueryToken(string appKey, string masterSecret)
        {
            return (this.AppKey + ":" + this.MasterSecret).ToBase64String();
        }
    }
}
