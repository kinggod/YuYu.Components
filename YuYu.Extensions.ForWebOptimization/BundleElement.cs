using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 捆绑配置节
    /// </summary>
    public class BundleElement : ConfigurationElement
    {
        /// <summary>
        /// 类型键
        /// </summary>
        public const string TypeKey = "type";

        /// <summary>
        /// 虚拟路径键
        /// </summary>
        public const string VirtualPathKey = "virtualPath";

        /// <summary>
        /// 虚拟路径键
        /// </summary>
        public const string FilesKey = "files";

        /// <summary>
        /// 虚拟文件夹路径键
        /// </summary>
        public const string DirectoriesKey = "directories";

        /// <summary>
        /// 类型
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(TypeKey, IsRequired = true)]
        public string Type
        {
            get { return (string)this[TypeKey]; }
            set { this[TypeKey] = value; }
        }

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
        /// 虚拟路径集合
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(FilesKey)]
        public FileCollection Files
        {
            get { return (FileCollection)this[FilesKey]; }
            set { this[FilesKey] = value; }
        }

        /// <summary>
        /// 虚拟文件夹路径集合
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(DirectoriesKey)]
        public DirectoryCollection Directories
        {
            get { return (DirectoryCollection)this[DirectoriesKey]; }
            set { this[DirectoriesKey] = value; }
        }
    }
}
