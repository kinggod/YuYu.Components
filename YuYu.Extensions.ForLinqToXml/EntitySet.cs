using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 实体集类型
    /// 在XmlContext派生类中定义此类型属性方可从Xml文档中加载或保存数据集
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntitySet<TEntity> : List<TEntity>
    {

    }
}
