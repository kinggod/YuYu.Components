using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// WebApi集合类
    /// </summary>
    public class WebApiRouteCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 元素名称
        /// </summary>
        public const string RouteKey = "route";

        /// <summary>
        /// 获取下标 index 的WebApi元素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public WebApiRouteElement this[int index]
        {
            get
            {
                return this.RouteElements[index];
            }
        }

        /// <summary>
        /// 获取 System.Configuration.ConfigurationElementCollection 的类型。
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        /// <summary>
        /// 元素名称
        /// </summary>
        protected override string ElementName
        {
            get { return RouteKey; }
        }

        /// <summary>
        /// WebApi元素组
        /// </summary>
        public virtual WebApiRouteElement[] RouteElements
        {
            get
            {
                return this.Cast<WebApiRouteElement>().ToArray();
            }
        }

        /// <summary>
        /// 添加WebApi元素
        /// </summary>
        /// <param name="element"></param>
        public void Add(WebApiRouteElement element)
        {
            base.BaseAdd(element);
        }

        /// <summary>
        /// 移除WebApi元素
        /// </summary>
        /// <param name="element"></param>
        public void Remove(WebApiRouteElement element)
        {
            base.BaseRemove(GetElementKey(element));
        }

        /// <summary>
        /// 新建元素
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new WebApiRouteElement();
        }

        /// <summary>
        /// 获取指定配置元素的元素键。
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return element;
        }
    }
}
