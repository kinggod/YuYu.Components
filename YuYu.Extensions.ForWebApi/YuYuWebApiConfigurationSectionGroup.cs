using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using YuYu.Components;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class YuYuWebApiConfigurationSectionGroup : ConfigurationSectionGroup
    {
        /// <summary>
        /// HttpConfiguration配置节KEY
        /// </summary>
        public const string HttpConfigurationSectionKey = "httpConfiguration";

        /// <summary>
        /// HttpConfiguration配置节
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(HttpConfigurationSectionKey)]
        public virtual YuYuWebApiHttpConfigurationConfigurationSection HttpConfiguration
        {
            get { return (YuYuWebApiHttpConfigurationConfigurationSection)this.Sections[HttpConfigurationSectionKey]; }
        }
    }
}
