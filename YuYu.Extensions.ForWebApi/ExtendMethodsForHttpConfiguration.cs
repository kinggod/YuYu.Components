using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtendMethodsForHttpConfiguration
    {
        /// <summary>
        /// 注册WebApi
        /// </summary>
        /// <param name="configuration"></param>
        public static void Register(this HttpConfiguration configuration)
        {
            YuYuWebApiConfigurationManager.RegisterWebApis(configuration);
        }
    }
}
