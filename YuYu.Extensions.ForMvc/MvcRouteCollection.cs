using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 路由集合类
    /// </summary>
    public class MvcRouteCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 元素名称
        /// </summary>
        public const string RouteKey = "route";

        /// <summary>
        /// 获取下标 index 的路由元素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MvcRouteElement this[int index]
        {
            get { return this.RouteElements[index]; }
        }

        /// <summary>
        /// 获取 System.Configuration.ConfigurationElementCollection 的类型。
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        /// <summary>
        /// 元素名称
        /// </summary>
        protected override string ElementName
        {
            get { return RouteKey; }
        }

        /// <summary>
        /// 路由元素组
        /// </summary>
        public virtual MvcRouteElement[] RouteElements
        {
            get { return this.Cast<MvcRouteElement>().ToArray(); }
        }

        /// <summary>
        /// 添加路由元素
        /// </summary>
        /// <param name="element"></param>
        public void Add(MvcRouteElement element)
        {
            base.BaseAdd(element);
        }

        /// <summary>
        /// 移除路由元素
        /// </summary>
        /// <param name="element"></param>
        public void Remove(MvcRouteElement element)
        {
            base.BaseRemove(GetElementKey(element));
        }

        /// <summary>
        /// 新建元素
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new MvcRouteElement();
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
