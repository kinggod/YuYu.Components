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
        public const string SectionName = "bundleCollection";

        /// <summary>
        /// 注册捆绑
        /// </summary>
        /// <param name="bundles"></param>
        public static void RegisterBundles(System.Web.Optimization.BundleCollection bundles)
        {
            _RegisterBundles(bundles);
        }

        #region

        private static YuYuWebBundleCollectionConfigurationSection _Section = (YuYuWebBundleCollectionConfigurationSection)YuYuWebConfigurationSectionGroup.Sections[SectionName];

        private static void _RegisterBundles(System.Web.Optimization.BundleCollection bundles)
        {
            foreach (BundleElement element in _Section.Bundles.BundleElements)
            {
                Bundle bundle = string.IsNullOrWhiteSpace(element.Type) ? new Bundle(element.VirtualPath) : Activator.CreateInstance(Type.GetType(element.Type), element.VirtualPath) as Bundle;
                foreach (FileElement file in element.Files.FileElements)
                {
                    bundle.Include(file.VirtualPath);
                }
                foreach (DirectoryElement directory in element.Directories.DirectoryElements)
                {
                    bundle.IncludeDirectory(directory.VirtualPath, directory.SearchPattern, directory.SearchSubdirectories);
                }
                bundles.Add(bundle);
            }
        }

        #endregion
    }
}
