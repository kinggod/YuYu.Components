using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace YuYu.Components
{
    internal class Helper
    {
        internal static RouteValueDictionary CreateRouteValueDictionary(object values)
        {
            IDictionary<string, object> dictionary = values as IDictionary<string, object>;
            if (dictionary != null)
                return new RouteValueDictionary(dictionary);
            return new RouteValueDictionary(values);
        }
    }
}
