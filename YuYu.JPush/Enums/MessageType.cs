using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YuYu.Components
{
    /// <summary>
    /// MessageType
    /// </summary>
    [DataContract]
    [Flags]
    public enum MessageType
    {
        /// <summary>
        /// 空
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// 通知
        /// </summary>
        [EnumMember]
        Notification = 1,

        /// <summary>
        /// 自定义消息
        /// </summary>
        [EnumMember]
        CustomizedMessage = 2
    }
}
