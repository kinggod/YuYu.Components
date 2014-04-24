using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 属性集合类
    /// </summary>
    public class PropertyCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 元素名称
        /// </summary>
        public const string PropertyKey = "property";

        /// <summary>
        /// 取得下标 index 的属性元素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PropertyElement this[int index]
        {
            get
            {
                return this.PropertyElements[index];
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
            get { return PropertyKey; }
        }

        /// <summary>
        /// 属性元素组
        /// </summary>
        public virtual PropertyElement[] PropertyElements
        {
            get
            {
                return this.Cast<PropertyElement>().ToArray();
            }
        }

        /// <summary>
        /// 创建匿名对象
        /// </summary>
        /// <returns></returns>
        public virtual object CreateObject()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("namespace __temp");
            builder.AppendLine("{");
            builder.AppendLine("public class __temp");
            builder.AppendLine("{");
            foreach (var item in this.PropertyElements)
            {
                builder.AppendLine(item.PropertyString);
            }
            builder.AppendLine("}");
            builder.AppendLine("}");
            return HelperBase.CreateObject(builder.ToString(), new string[] { "System.dll", AppDomain.CurrentDomain.BaseDirectory + "bin\\System.Web.Mvc.dll" });
        }

        /// <summary>
        /// 添加属性元素
        /// </summary>
        /// <param name="element"></param>
        public void Add(PropertyElement element)
        {
            base.BaseAdd(element);
        }

        /// <summary>
        /// 移除属性元素
        /// </summary>
        /// <param name="element"></param>
        public void Remove(PropertyElement element)
        {
            base.BaseRemove(GetElementKey(element));
        }

        /// <summary>
        /// 新建元素
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new PropertyElement();
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
