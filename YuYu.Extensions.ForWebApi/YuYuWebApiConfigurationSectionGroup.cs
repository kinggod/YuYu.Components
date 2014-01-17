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
        /// webApi配置节
        /// </summary>
        public const string WebApisSectionKey = "httpConfiguration";

        /// <summary>
        /// WebApi集合
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [ConfigurationProperty(WebApisSectionKey)]
        public virtual YuYuWebApiHttpConfigurationConfigurationSection WebApisSection
        {
            get { return (YuYuWebApiHttpConfigurationConfigurationSection)this.Sections[WebApisSectionKey]; }
        }
    }
}
