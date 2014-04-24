using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class WebOptimizationFileCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 元素名称
        /// </summary>
        public const string IncludeKey = "file";

        /// <summary>
        /// 获取下标 index 的路由元素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public WebOptimizationFileElement this[int index]
        {
            get
            {
                return this.FileElements[index];
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
            get { return IncludeKey; }
        }

        /// <summary>
        /// 路由元素组
        /// </summary>
        public virtual WebOptimizationFileElement[] FileElements
        {
            get
            {
                return this.Cast<WebOptimizationFileElement>().ToArray();
            }
        }

        /// <summary>
        /// 添加路由元素
        /// </summary>
        /// <param name="element"></param>
        public void Add(WebOptimizationFileElement element)
        {
            base.BaseAdd(element);
        }

        /// <summary>
        /// 移除路由元素
        /// </summary>
        /// <param name="element"></param>
        public void Remove(WebOptimizationFileElement element)
        {
            base.BaseRemove(GetElementKey(element));
        }

        /// <summary>
        /// 新建元素
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new WebOptimizationFileElement();
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
