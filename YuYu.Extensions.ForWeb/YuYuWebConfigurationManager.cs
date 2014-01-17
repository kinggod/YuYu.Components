using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Configuration;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class YuYuWebConfigurationManager
    {
        /// <summary>
        /// 配置节名称
        /// </summary>
        public const string SectionGroupName = "yuyu.web";

        /// <summary>
        /// YuYu.Web配置节组
        /// </summary>
        public static YuYuWebConfigurationSectionGroup YuYuWebConfigurationSectionGroup = (YuYuWebConfigurationSectionGroup)WebConfigurationManager.OpenWebConfiguration("~/web.config").GetSectionGroup(SectionGroupName);
    }
}
