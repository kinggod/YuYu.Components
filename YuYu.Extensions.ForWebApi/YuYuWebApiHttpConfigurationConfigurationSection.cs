using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// YuYu配置块
    /// </summary>
    public class YuYuWebApiHttpConfigurationConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// webApi配置节
        /// </summary>
        public const string WebApisKey = "webApis";

        /// <summary>
        /// WebApi集合
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(WebApisKey)]
        public virtual WebApiCollection WebApis
        {
            get { return (WebApiCollection)this[WebApisKey]; }
            set { this[WebApisKey] = value; }
        }
    }
}
