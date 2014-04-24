using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 过滤器集合类
    /// </summary>
    public class MvcGlobalFilterCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 元素名称
        /// </summary>
        public const string FilterKey = "globalFilter";

        /// <summary>
        /// 获取下标 index 的过滤器元素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MvcGlobalFilterElement this[int index]
        {
            get
            {
                return this.GlobalFilterElements[index];
            }
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
            get { return FilterKey; }
        }

        /// <summary>
        /// 过滤器元素组
        /// </summary>
        public virtual MvcGlobalFilterElement[] GlobalFilterElements
        {
            get
            {
                return this.Cast<MvcGlobalFilterElement>().OrderBy(e => e.Order).ToArray();
            }
        }

        /// <summary>
        /// 添加过滤器元素
        /// </summary>
        /// <param name="element"></param>
        public void Add(MvcGlobalFilterElement element)
        {
            base.BaseAdd(element);
        }

        /// <summary>
        /// 移除过滤器元素
        /// </summary>
        /// <param name="element"></param>
        public void Remove(MvcGlobalFilterElement element)
        {
            base.BaseRemove(GetElementKey(element));
        }

        /// <summary>
        /// 新建元素
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new MvcGlobalFilterElement();
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
