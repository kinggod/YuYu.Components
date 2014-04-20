using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YuYu.Components
{
    /// <summary>
    /// Platform
    /// </summary>
    [DataContract]
    [Flags]
    public enum Platform
    {
        /// <summary>
        /// None
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// Android
        /// </summary>
        [EnumMember]
        Android = 1,

        /// <summary>
        /// iOS
        /// </summary>
        [EnumMember]
        iOS = 2,

        /// <summary>
        /// WindowPhone
        /// </summary>
        [EnumMember]
        WindowPhone = 4,
    }
}
