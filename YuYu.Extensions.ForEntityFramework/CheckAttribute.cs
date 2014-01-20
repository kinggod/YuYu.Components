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
    public class CheckAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        public CheckAttribute(string expression)
        {
            if (expression.IndexOf("{0}") >= 0)
                this.Expression = expression;
            else
                throw new ArgumentException("检查约束表达式没有提供字段列占位符！");
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public string Expression { get; set; }
    }
}
