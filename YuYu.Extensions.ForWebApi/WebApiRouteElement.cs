using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// WebApi元素类
    /// </summary>
    public class WebApiRouteElement : ConfigurationElement
    {
        /// <summary>
        /// WebApi名称键
        /// </summary>
        public const string NameKey = "name";

        /// <summary>
        /// WebApi路由模板键
        /// </summary>
        public const string RouteTemplateKey = "routeTemplate";

        /// <summary>
        /// WebApi默认值集合键
        /// </summary>
        public const string DefaultsKey = "defaults";

        /// <summary>
        /// WebApi路由约束集合键
        /// </summary>
        public const string ConstraintsKey = "constraints";

        /// <summary>
        /// WebApi处理程序类型键
        /// </summary>
        public const string HandlerTypeKey = "handlerType";

        /// <summary>
        /// WebApi名称
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(NameKey, IsRequired = true)]
        public string Name
        {
            get { return (string)this[NameKey]; }
            set { this[NameKey] = value; }
        }

        /// <summary>
        /// WebApi路由模板
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(RouteTemplateKey, IsRequired = true)]
        public string RouteTemplate
        {
            get { return (string)this[RouteTemplateKey]; }
            set { this[RouteTemplateKey] = value; }
        }

        /// <summary>
        /// WebApi默认值集合
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(DefaultsKey, IsRequired = true)]
        public PropertyCollection Defaults
        {
            get { return (PropertyCollection)this[DefaultsKey]; }
            set { this[DefaultsKey] = value; }
        }

        /// <summary>
        /// WebApi路由约束集合
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(ConstraintsKey)]
        public PropertyCollection Constraints
        {
            get { return (PropertyCollection)this[ConstraintsKey]; }
            set { this[ConstraintsKey] = value; }
        }

        /// <summary>
        /// 表示WebApi处理程序类的字符串
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(HandlerTypeKey)]
        public string HandlerType
        {
            get { return (string)this[HandlerTypeKey]; }
            set { this[HandlerTypeKey] = value; }
        }

        /// <summary>
        /// 创建WebApi处理程序实例对象
        /// </summary>
        /// <returns></returns>
        internal HttpMessageHandler Handler
        {
            get
            {
                if (string.IsNullOrWhiteSpace(HandlerType))
                    return null;
                return Activator.CreateInstance(System.Type.GetType(HandlerType)) as HttpMessageHandler;
            }
        }
    }
}
