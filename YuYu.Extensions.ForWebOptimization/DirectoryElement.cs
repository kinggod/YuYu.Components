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
    public class DirectoryElement : ConfigurationElement
    {
        /// <summary>
        /// 虚拟路径键
        /// </summary>
        public const string VirtualPathKey = "virtualPath";

        /// <summary>
        /// 检索表达式键
        /// </summary>
        public const string SearchPatternKey = "searchPattern";

        /// <summary>
        /// 是否检索子目录键
        /// </summary>
        public const string SearchSubdirectoriesKey = "searchSubdirectories";

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

        /// <summary>
        /// 检索表达式
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(SearchPatternKey, IsRequired = true)]
        public string SearchPattern
        {
            get { return (string)this[SearchPatternKey]; }
            set { this[SearchPatternKey] = value; }
        }

        /// <summary>
        /// 检索表达式
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(SearchSubdirectoriesKey, DefaultValue = false)]
        public bool SearchSubdirectories
        {
            get { return (bool)this[SearchSubdirectoriesKey]; }
            set { this[SearchSubdirectoriesKey] = value; }
        }
    }
}
