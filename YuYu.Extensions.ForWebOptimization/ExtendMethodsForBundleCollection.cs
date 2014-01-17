using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtendMethodsForBundleCollection
    {
        /// <summary>
        /// 注册绑定
        /// </summary>
        /// <param name="bundles"></param>
        public static void RegisterBundles(this System.Web.Optimization.BundleCollection bundles)
        {
            _RegisterBundles(bundles);
        }

        #region

        private static void _RegisterBundles(System.Web.Optimization.BundleCollection bundles)
        {
            YuYuWebOptimizationConfigurationManager.RegisterBundles(bundles);
        }

        #endregion
    }
}
