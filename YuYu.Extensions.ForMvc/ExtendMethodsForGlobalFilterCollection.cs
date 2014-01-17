using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtendMethodsForGlobalFilterCollection
    {
        /// <summary>
        /// 注册Filter
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(this GlobalFilterCollection filters)
        {
            YuYuMvcConfigurationManager.RegisterGlobalFilters(filters);
        }
    }
}
