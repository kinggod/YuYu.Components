using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 被占用的关键字
    /// </summary>
    public class Keywords
    {
        /// <summary>
        /// 根节点名称
        /// </summary>
        public const string ROOTNODENAME = "yuyu.xmlToEntity";

        /// <summary>
        /// 实体集节点名称
        /// </summary>
        public const string ENTITIESNODENAME = "entities";

        /// <summary>
        /// 实体对象节点名称
        /// </summary>
        public const string ENTITYNODENAME = "entity";

        /// <summary>
        /// 用于定义实体类型的字符串特性名称
        /// </summary>
        public const string ENTITYTYPEATTRIBUTENAME = "type";

        /// <summary>
        /// 用于定义实体类型的字符串特性名称
        /// </summary>
        public const string ENTITYELEMENTTYPEATTRIBUTENAME = "elementType";

        /// <summary>
        /// 用于定义实体数据时间戳的字符串特性名称
        /// </summary>
        public const string ENTITYTIMESTAMPATTRIBUTENAME = "timestamp";
    }
}
