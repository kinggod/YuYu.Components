using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YuYu.Components
{
    /// <summary>
    /// PushTracking
    /// </summary>
    [DataContract]
    public class Tracking
    {
        /// <summary>
        /// 通知ID
        /// </summary>
        [DataMember]
        public string MessageID { get; set; }

        /// <summary>
        /// UTC时间戳
        /// </summary>
        [DataMember]
        public DateTime UtcTimestamp { get; set; }

        /// <summary>
        /// 重写ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.MessageID ?? string.Empty;
        }
    }
}
