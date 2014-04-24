using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class RouteValueDictionaryHelper
    {
        /// <summary>
        /// 获取RouteValueDictionary
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static RouteValueDictionary CreateRouteValueDictionary(object values)
        {
            IDictionary<string, object> dictionary = values as IDictionary<string, object>;
            if (dictionary != null)
                return new RouteValueDictionary(dictionary);
            return new RouteValueDictionary(values);
        }

    }
}
