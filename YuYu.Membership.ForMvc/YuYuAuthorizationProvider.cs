using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class YuYuAuthorizationProvider
    {
        /// <summary>
        /// 会员关系提供程序实例
        /// </summary>
        public static IMembershipProvider MembershipProvider
        {
            get
            {
                if (_MembershipProvider == null)
                    throw new Exception("此提供程序尚未实例化。通过Unity配置或调用“SetMembershipProvider”方法以初始化此提供程序。");
                return _MembershipProvider;
            }
        }

        /// <summary>
        /// IOC容器
        /// </summary>
        public static IUnityContainer UnityContainer { get; private set; }

        static IMembershipProvider _MembershipProvider;

        static YuYuAuthorizationProvider()
        {
            UnityContainer = new UnityContainer();
            UnityConfigurationSection configuration = ConfigurationManager.GetSection(UnityConfigurationSection.SectionName) as UnityConfigurationSection;
            configuration.Configure(UnityContainer, "container");
            _MembershipProvider = UnityContainer.Resolve<IMembershipProvider>();
        }

        /// <summary>
        /// 配置会员关系提供程序
        /// </summary>
        /// <param name="membershipProvider"></param>
        public static void SetMembershipProvider(IMembershipProvider membershipProvider)
        {
            _MembershipProvider = membershipProvider;
        }
    }
}
