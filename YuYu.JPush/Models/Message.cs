using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YuYu.Components
{
    /// <summary>
    /// 推送消息
    /// </summary>
    [DataContract]
    public class Message
    {
        /// <summary>
        /// 通知标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 通知内容
        /// </summary>
        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// 通知附加参数。JSON格式。客户端可取得全部内容。
        /// </summary>
        [DataMember]
        public Dictionary<string, object> CustomizedValue { get; set; }

        /// <summary>
        /// 1-1000的数值，不填则默认为 0，使用 极光Push SDK 的默认通知样式。只有 Android 支持这个参数。
        /// 进一步了解请参考文档通知栏样式定制API:http://docs.jpush.cn/pages/viewpage.action?pageId=557243
        /// </summary>
        [DataMember]
        public int Android_BuilderId { get; set; }

        /// <summary>
        /// 显示到应用程序图标上的数字
        /// </summary>
        [DataMember]
        public int? iOS_Badge { get; set; }

        /// <summary>
        /// 提示声音
        /// </summary>
        [DataMember]
        public string iOS_Sound { get; set; }

        /// <summary>
        /// 获取通知的Json格式字符串
        /// <example>
        /// 格式：{"n_builder_id":"通知样式","n_title":"通知标题","n_content":"通知内容", "n_extras":{"ios":{"badge":8, "sound":"happy"}, "user_param_1":"value1", "user_param_2":"value2"}}
        /// </example>
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <returns></returns>
        public string GetJsonString(Platform platform = Platform.Android|Platform.iOS)
        {
            IDictionary<string, object> data = new Dictionary<string, object> { { "n_title", this.Title ?? string.Empty }, { "n_content", this.Content ?? string.Empty } };
            IDictionary<string, object> extra = new Dictionary<string, object>();
            if (this.CustomizedValue != null)
                foreach (string key in this.CustomizedValue.Keys)
                {
                    extra[key] = this.CustomizedValue[key];
                }
            if (platform.Contains(Platform.Android))
            {
                if (this.Android_BuilderId > 0 && this.Android_BuilderId <= 1000)
                    data["n_builder_id"] = this.Android_BuilderId;
            }
            if (platform.Contains(Platform.iOS))
            {
                IDictionary<string, object> iOSExtra = new Dictionary<string, object>();

                iOSExtra["badge"] = this.iOS_Badge;

                if (!string.IsNullOrWhiteSpace(this.iOS_Sound))
                    iOSExtra["sound"] = this.iOS_Sound;
                extra["ios"] = iOSExtra;
            }
            data.Add("n_extras", extra);
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// 重写ToString()=>this..GetJSONString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.GetJsonString();
        }
    }
}
