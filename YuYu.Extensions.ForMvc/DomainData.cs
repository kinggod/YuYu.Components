using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 域名信息
    /// </summary>
    public class DomainData
    {
        /// <summary>
        /// 协议
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// 主机名
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// 其它
        /// </summary>
        public int Port { get; set; }
    }
}
