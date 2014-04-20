using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YuYu.Components
{
    /// <summary>
    /// PushRequest
    /// </summary>
    [DataContract]
    [KnownType(typeof(Message))]
    [KnownType(typeof(MessageType))]
    [KnownType(typeof(Platform))]
    [KnownType(typeof(ReceiverType))]
    public class PushRequest
    {
        /// <summary>
        /// 发送编号（最大支持32位正整数(即 4294967295 )）。由开发者自己维护，用于开发者自己标识一次发送请求。
        /// </summary>
        [DataMember]
        public int SendNo
        {
            get
            {
                return (int)(((DateTime.UtcNow - new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds) % Int32.MaxValue);
            }
        }

        /// <summary>
        /// 接收者类型
        /// </summary>
        /// <value>The type.</value>
        [DataMember]
        public ReceiverType ReceiverType { get; set; }

        /// <summary>
        /// 发送范围值，与 receiver_type 相对应。
        /// </summary>
        [DataMember]
        public string ReceiverValue { get; set; }

        /// <summary>
        /// 通知主体
        /// </summary>
        [DataMember]
        public Message Message { get; set; }

        /// <summary>
        /// 通知类型
        /// </summary>
        [DataMember]
        public MessageType MessageType { get; set; }

        /// <summary>
        /// 接收通知的设备平台
        /// </summary>
        [DataMember]
        public Platform Platform { get; set; }

        /// <summary>
        /// 通知描述
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// 服务器端保存时长（秒）
        /// 最大: 864000 Seconds (10天)
        /// 默认: 86400 Seconds (1天)
        /// </summary>
        [DataMember]
        public int LifeTime { get; set; }

        /// <summary>
        /// 待覆盖的通知ID
        /// </summary>
        [DataMember]
        public string OverrideMessageID { get; set; }

        /// <summary>
        /// 是否测试状态
        /// </summary>
        [DataMember]
        public bool IsTest { get; set; }

        /// <summary>
        /// 获取推送请求参数字典
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> GetDictionary()
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("sendno", ((int)this.SendNo).ToString());
            dictionary.Add("receiver_type", ((int)this.ReceiverType).ToString());
            dictionary.Add("receiver_value", (this.ReceiverValue ?? string.Empty).UrlEncode());
            dictionary.Add("msg_type", ((int)this.MessageType).ToString());
            dictionary.Add("msg_content", this.Message.GetJsonString(this.Platform).UrlEncode());
            dictionary.Add("send_description", (this.Description ?? string.Empty).UrlEncode());
            dictionary.Add("platform", this.Platform.GetString().UrlEncode());
            if (this.Platform.Contains(Platform.iOS))
                dictionary.Add("apns_production", this.IsTest ? "0" : "1");
            dictionary.Add("time_to_live", this.LifeTime.ToString());
            if (!string.IsNullOrWhiteSpace(this.OverrideMessageID))
                dictionary.Add("override_msg_id", this.OverrideMessageID);
            return dictionary;
        }

        /// <summary>
        /// 验证串，用于校验发送的合法性。
        /// </summary>
        public string GetVerificationCode(string master_secret)
        {
            return (string.Empty + this.SendNo + (int)this.ReceiverType + this.ReceiverValue + master_secret).MD5Encode();
        }
    }
}
