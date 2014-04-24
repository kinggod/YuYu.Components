using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Optimization;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class YuYuWebOptimizationConfigurationManager : YuYuWebConfigurationManager
    {
        /// <summary>
        /// 配置节名称
        /// </summary>
        public const string WebOptimizationSectionGroupName = "webOptimization";

        /// <summary>
        /// 注册捆绑
        /// </summary>
        /// <param name="bundles"></param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            foreach (WebOptimizationBundleElement element in YuYuWebOptimizationConfigurationSectionGroup.BundleCollection.Bundles.BundleElements)
            {
                Bundle bundle = string.IsNullOrWhiteSpace(element.Type) ? new Bundle(element.VirtualPath) : Activator.CreateInstance(Type.GetType(element.Type), element.VirtualPath) as Bundle;
                foreach (WebOptimizationFileElement file in element.Files.FileElements)
                {
                    bundle.Include(file.VirtualPath);
                }
                foreach (WebOptimizationDirectoryElement directory in element.Directories.DirectoryElements)
                {
                    bundle.IncludeDirectory(directory.VirtualPath, directory.SearchPattern, directory.SearchSubdirectories);
                }
                bundles.Add(bundle);
            }
        }

        /// <summary>
        /// WebOptimization配置节组
        /// </summary>
        public static YuYuWebOptimizationConfigurationSectionGroup YuYuWebOptimizationConfigurationSectionGroup = (YuYuWebOptimizationConfigurationSectionGroup)YuYuWebConfigurationSectionGroup.SectionGroups[WebOptimizationSectionGroupName];
    }
}
