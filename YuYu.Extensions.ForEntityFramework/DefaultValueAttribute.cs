using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DefaultValueAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public DefaultValueAttribute(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public string Value { get; set; }
    }
}
