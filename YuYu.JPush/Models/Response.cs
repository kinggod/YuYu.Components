using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YuYu.Components
{
    /// <summary>
    /// PushResponse
    /// </summary>
    [DataContract]
    [KnownType(typeof(ResponseCode))]
    public class Response
    {
        /// <summary>
        /// Gets or sets the send identity.
        /// </summary>
        /// <value>The send identity.</value>
        [DataMember]
        public string SendIdentity { get; set; }

        /// <summary>
        /// Gets or sets the message unique identifier.
        /// </summary>
        /// <value>The message unique identifier.</value>
        [DataMember]
        public string MessageID { get; set; }

        /// <summary>
        /// Gets or sets the response code.
        /// </summary>
        /// <value>The response code.</value>
        [DataMember]
        public ResponseCode ResponseCode { get; set; }

        /// <summary>
        /// Gets or sets the response message.
        /// </summary>
        /// <value>The response message.</value>
        [DataMember]
        public string ResponseMessage { get; set; }
    }
}
