using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class WebOptimizationFileElement : ConfigurationElement
    {
        /// <summary>
        /// 虚拟路径键
        /// </summary>
        public const string VirtualPathKey = "virtualPath";

        /// <summary>
        /// 虚拟路径
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(VirtualPathKey, IsRequired = true)]
        public string VirtualPath
        {
            get { return (string)this[VirtualPathKey]; }
            set { this[VirtualPathKey] = value; }
        }
    }
}
