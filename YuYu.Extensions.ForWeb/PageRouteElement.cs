using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace YuYu.Components
{
    /// <summary>
    /// 路由元素类
    /// </summary>
    public class PageRouteElement : ConfigurationElement
    {
        /// <summary>
        /// 路由名称属性键
        /// </summary>
        public const string NameKey = "name";

        /// <summary>
        /// 请求协议属性键
        /// </summary>
        public const string ProtocolKey = "protocol";

        /// <summary>
        /// 域属性键
        /// </summary>
        public const string DomainKey = "domain";

        /// <summary>
        /// 端口号属性键
        /// </summary>
        public const string PortKey = "port";

        /// <summary>
        /// 路由模板属性键
        /// </summary>
        public const string UrlKey = "url";

        /// <summary>
        /// 路由处理程序类属性键
        /// </summary>
        public const string PhysicalFileKey = "physicalFile";

        /// <summary>
        /// 路由处理程序类属性键
        /// </summary>
        public const string CheckPhysicalUrlAccessKey = "checkPhysicalUrlAccess";

        /// <summary>
        /// 默认值集合键
        /// </summary>
        public const string DefaultsKey = "defaults";

        /// <summary>
        /// 路由约束集合键
        /// </summary>
        public const string ConstraintsKey = "constraints";

        /// <summary>
        /// 路由名称
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(NameKey, IsRequired = true)]
        public string Name
        {
            get { return (string)this[NameKey]; }
            set { this[NameKey] = value; }
        }

        /// <summary>
        /// 请求协议
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(ProtocolKey)]
        public string Protocol
        {
            get { return (string)this[ProtocolKey]; }
            set { this[ProtocolKey] = value; }
        }

        /// <summary>
        /// 域
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(DomainKey)]
        public string Domain
        {
            get { return (string)this[DomainKey]; }
            set { this[DomainKey] = value; }
        }

        /// <summary>
        /// 端口号
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(PortKey)]
        public int Port
        {
            get { return (int)this[PortKey]; }
            set { this[PortKey] = value; }
        }

        /// <summary>
        /// 路由模板
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(UrlKey, IsRequired = true)]
        public string Url
        {
            get { return (string)this[UrlKey]; }
            set { this[UrlKey] = value; }
        }

        /// <summary>
        /// 表示路由控制程序类型的字符串
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(PhysicalFileKey, IsRequired = true)]
        public string PhysicalFile
        {
            get { return (string)this[PhysicalFileKey]; }
            set { this[PhysicalFileKey] = value; }
        }

        /// <summary>
        /// 表示路由控制程序类型的字符串
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(CheckPhysicalUrlAccessKey)]
        public bool CheckPhysicalUrlAccess
        {
            get { return (bool)this[CheckPhysicalUrlAccessKey]; }
            set { this[CheckPhysicalUrlAccessKey] = value; }
        }

        /// <summary>
        /// 默认值集合
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(DefaultsKey)]
        public PropertyCollection Defaults
        {
            get { return (PropertyCollection)this[DefaultsKey]; }
            set { this[DefaultsKey] = value; }
        }

        /// <summary>
        /// 路由约束
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(ConstraintsKey)]
        public PropertyCollection Constraints
        {
            get { return (PropertyCollection)this[ConstraintsKey]; }
            set { this[ConstraintsKey] = value; }
        }
    }
}
