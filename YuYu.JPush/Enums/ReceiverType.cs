using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YuYu.Components
{
    /// <summary>
    /// 接收者类型
    /// </summary>
    [DataContract]
    public enum ReceiverType
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// 指定的 tag
        /// </summary>
        [EnumMember]
        ByTag = 2,

        /// <summary>
        /// 指定的 alias
        /// </summary>
        [EnumMember]
        ByAlias = 3,

        /// <summary>
        /// 广播：对 app_key 下的所有用户推送消息
        /// </summary>
        [EnumMember]
        Broadcast = 4,

        /// <summary>
        /// 根据 RegistrationID 进行推送。当前只是 Android SDK r1.6.0 版本支持。相关文档：http://blog.jpush.cn/registrationid_pusn_launch/
        /// </summary>
        [EnumMember]
        ByRegistrationId = 5
    }
}
