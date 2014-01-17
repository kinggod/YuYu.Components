using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace YuYu.Components
{
    /// <summary>
    /// Xml上下文
    /// </summary>
    public abstract class XmlContext : IDisposable
    {
        /// <summary>
        /// Xml提供程序
        /// </summary>
        public XmlProvider XmlProvider { get; private set; }

        /// <summary>
        /// Xml上下文构造函数
        /// </summary>
        /// <param name="xmlFilePath">Xml文档路径</param>
        public XmlContext(string xmlFilePath) : this(xmlFilePath, null) { }
        
        /// <summary>
        /// 获取实体集
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="elements"></param>
        /// <returns></returns>
        public EntitySet<TEntity> GetEntitySet<TEntity>(IEnumerable<XElement> elements = null) where TEntity : class
        {
            EntitySet<TEntity> entitySet = new EntitySet<TEntity>();
            Type entityType = typeof(TEntity);
            ArrayList entities = this._GetEntitySet(entityType);
            foreach (object entity in entities)
                entitySet.Add(Convert.ChangeType(entity, entityType) as TEntity);
            return entitySet;
        }

        /// <summary>
        /// 保存更改
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            try
            {
                IEnumerable<PropertyInfo> propertyInfos = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(m => m.PropertyType.Name == "EntitySet`1");
                IList<XElement> elements = new List<XElement>();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    Type entityType = propertyInfo.PropertyType.GetGenericArguments()[0];
                    object entitySet = propertyInfo.GetValue(this, null);
                    if (entitySet != null)
                    {
                        XElement xe = new XElement(Keywords.ENTITIESNODENAME);
                        if (entityType != null)
                            xe.SetAttributeValue(Keywords.ENTITYTYPEATTRIBUTENAME, entityType.FullName);
                        IEnumerator e = (entitySet as IEnumerable).GetEnumerator();
                        while (e.MoveNext())
                            xe.Add(this._GeneralXml(e.Current, entityType));
                        elements.Add(xe);
                    }
                }
                this.XmlProvider.WriteElementsToFile(elements);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            XmlProvider = null;
            IEnumerable<PropertyInfo> propertyInfos = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo propertyInfo in propertyInfos)
                propertyInfo.SetValue(this, null, null);
        }

        #region

        private XmlContext(string xmlFilePath, XmlProvider xmlProvider)
        {
            this.XmlProvider = new XmlProvider(xmlFilePath);
            IEnumerable<PropertyInfo> propertyInfos = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(m => m.PropertyType.Name == "EntitySet`1");
            if (propertyInfos != null)
                foreach (PropertyInfo propertyInfo in propertyInfos)
                    propertyInfo.SetValue(this, typeof(XmlContext).GetMethod("GetEntitySet").MakeGenericMethod(propertyInfo.PropertyType.GetGenericArguments()[0]).Invoke(this, new object[] { null }), null);
        }

        private ArrayList _GetEntitySet(Type entityType, IEnumerable<XElement> elements = null)
        {
            ArrayList set = new ArrayList();
            elements = elements ?? this.XmlProvider.GetElements(entityType);
            if (elements != null)
                foreach (XElement item in elements)
                {
                    object entity = this._CreateInstance(entityType, item);
                    if (entity != null)
                        set.Add(entity);
                }
            return set;
        }

        private object _CreateInstance(Type type, XElement element)
        {
            if (element.Attribute(Keywords.ENTITYTYPEATTRIBUTENAME) != null)
            {
                Type objType = Type.GetType(element.Attribute(Keywords.ENTITYTYPEATTRIBUTENAME).Value, false);
                if (objType != null && objType.IsSubclassOf(type))
                    type = objType;
                throw new Exception(Keywords.ENTITYTYPEATTRIBUTENAME + "所指向的类型非目标类型或其派生类型！");
            }
            object entity = Activator.CreateInstance(type);
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfos != null)
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    XElement propertyElement = element.Element(propertyInfo.Name);
                    if (propertyElement != null)
                    {
                        Type propertyType = propertyInfo.PropertyType;
                        if (propertyType.IsPrimitive || propertyType.IsEnum || propertyType == typeof(string))
                            try
                            {
                                object value = propertyType.IsEnum ? Enum.Parse(propertyType, propertyElement.Value) : Convert.ChangeType(propertyElement.Value, propertyType);
                                propertyInfo.SetValue(entity, value, null);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        else
                            if (propertyType.GetInterface("IEnumerable") != null)
                            {
                                Type elementType = null;
                                if (propertyType.IsArray)//属性是多维数组跳过
                                {
                                    elementType = propertyType.GetElementType();
                                    if (propertyType.GetArrayRank() > 1)
                                        continue;
                                }
                                else if (propertyType.IsGenericType)
                                {
                                    Type[] genericTypes = propertyType.GetGenericArguments();
                                    if (genericTypes.Length > 1)//非单泛型跳过
                                        continue;
                                    elementType = genericTypes[0];
                                }
                                if (elementType.IsPrimitive || elementType.IsEnum || elementType == typeof(string))
                                {
                                    string[] values = propertyElement.Value.Split(';');
                                    ArrayList list = new ArrayList(values.Length);
                                    try
                                    {
                                        foreach (string v in values)
                                        {
                                            object value = elementType.IsEnum ? Enum.Parse(elementType, v) : Convert.ChangeType(v, elementType);
                                            list.Add(value);
                                        }
                                    }
                                    catch (Exception) { }
                                    propertyInfo.SetValue(entity, list.ToArray(elementType), null);
                                    continue;
                                }
                                else
                                {
                                    ArrayList entities = this._GetEntitySet(elementType, propertyElement.Elements(Keywords.ENTITYNODENAME));
                                    if (propertyType.IsArray)
                                        propertyInfo.SetValue(entity, entities.ToArray(elementType), null);
                                    else
                                    {
                                        string typeString = propertyType.FullName
                                            .Replace("IList`1", "List`1")
                                            .Replace("ICollection`1", "List`1")
                                            .Replace("IEnumerable`1", "List`1")
                                            .Replace("IDictionary`1", "Dictionary`1");
                                        IList list = Activator.CreateInstance(Type.GetType(typeString)) as IList;
                                        foreach (var item in entities)
                                            list.Add(item);
                                        propertyInfo.SetValue(entity, list, null);
                                    }
                                }
                            }
                            else
                                propertyInfo.SetValue(entity, this._CreateInstance(propertyType, propertyElement), null);
                    }
                }
            return entity;
        }

        private XElement _GeneralXml(object entity, Type type, XName elementName = null)
        {
            if (entity == null)
                return null;
            XElement xe = new XElement(elementName ?? Keywords.ENTITYNODENAME, new XAttribute(Keywords.ENTITYTIMESTAMPATTRIBUTENAME, DateTime.Now.Ticks));
            Type objType = entity.GetType();
            if (objType != type && !"<>f__AnonymousType0".Equals(objType.FullName))
                xe.SetAttributeValue(Keywords.ENTITYTYPEATTRIBUTENAME, objType.FullName);
            PropertyInfo[] propertyInfos = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfos != null)
            {
                IEnumerable<PropertyInfo> tmp = propertyInfos.Where(m => (m.PropertyType.IsPrimitive || m.PropertyType.IsEnum || m.PropertyType == typeof(string)));
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    Type propertyType = propertyInfo.PropertyType;
                    if (propertyType.IsPrimitive || propertyType.IsEnum || propertyType == typeof(string))
                    {
                        XElement propertyElement = new XElement(propertyInfo.Name, propertyInfo.GetValue(entity, null));
                        xe.Add(propertyElement);
                    }
                    else if (propertyType.GetInterface("IEnumerable") != null)
                    {
                        Type elementType = null;
                        if (propertyType.IsArray)//属性是多维数组跳过
                        {
                            if (propertyType.GetArrayRank() > 1)
                                continue;
                            elementType = propertyType.GetElementType();
                        }
                        else if (propertyType.IsGenericType)
                        {
                            Type[] genericTypes = propertyType.GetGenericArguments();
                            if (genericTypes.Length > 1)//非单泛型跳过
                                continue;
                            elementType = genericTypes[0];
                        }
                        if (elementType.IsPrimitive || elementType.IsEnum || elementType == typeof(string))
                        {
                            IEnumerable values = propertyInfo.GetValue(entity, null) as IEnumerable;
                            if (values != null)
                            {
                                StringBuilder sb = new StringBuilder();
                                IEnumerator rator = values.GetEnumerator();
                                while (rator.MoveNext())
                                {
                                    sb.Append(rator.Current);
                                    sb.Append(";");
                                }
                                string content = sb.ToString();
                                content = content.Remove(content.Length - 1);
                                XElement propertyElement = new XElement(propertyInfo.Name, content);
                                propertyElement.SetAttributeValue(Keywords.ENTITYTYPEATTRIBUTENAME, propertyType.FullName);
                                xe.Add(propertyElement);
                            }
                            continue;
                        }
                        else
                        {
                            XElement propertyElement = new XElement(propertyInfo.Name);
                            propertyElement.SetAttributeValue(Keywords.ENTITYTYPEATTRIBUTENAME, propertyType.FullName);
                            propertyElement.SetAttributeValue(Keywords.ENTITYELEMENTTYPEATTRIBUTENAME, elementType.FullName);
                            IEnumerator e = (propertyInfo.GetValue(entity, null) as IEnumerable).GetEnumerator();
                            while (e.MoveNext())
                                propertyElement.Add(this._GeneralXml(e.Current, elementType));
                            xe.Add(propertyElement);
                        }
                    }
                    else
                        xe.Add(this._GeneralXml(propertyInfo.GetValue(entity, null), propertyType, propertyInfo.Name));
                }
            }
            return xe;
        }

        #endregion
    }
}
