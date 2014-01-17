using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 会员关系
    /// </summary>
    public class YuYuMembership
    {
        /// <summary>
        /// 提供程序
        /// </summary>
        public static IMembershipProvider Provider = YuYuAuthorizationProvider.MembershipProvider;

        /// <summary>
        /// 获取当前系统所有可用的权限
        /// </summary>
        /// <returns></returns>
        public static IList<IAuthority> GetAllAuthorities()
        {
            return Provider.GetAllAuthorities();
        }

        /// <summary>
        /// 确定当前用户是否拥有指定的角色
        /// </summary>
        /// <param name="user">已授权用户</param>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        public static bool IsInRole(IPrincipal user, Guid roleID)
        {
            return Provider.IsInRole(user, roleID);
        }

        /// <summary>
        /// 确定当前用户是否拥有指定的角色
        /// </summary>
        /// <param name="account">帐户实例对象</param>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        public static bool IsInRole(IAccount account, Guid roleID)
        {
            return Provider.IsInRole(account, roleID);
        }

        /// <summary>
        /// 确定当前用户是否拥有指定的权限
        /// </summary>
        /// <param name="user">已授权用户</param>
        /// <param name="actionName">权限所定义的业务处理器名称</param>
        /// <param name="controllerName">权限所定义的控制器名称</param>
        /// <param name="namespace">权限所在命名空间</param>
        /// <returns></returns>
        public static bool HasAuthority(IPrincipal user, string actionName, string controllerName, string @namespace = null)
        {
            return Provider.HasAuthority(user, actionName, controllerName, @namespace);
        }

        /// <summary>
        /// 确定当前用户是否拥有指定的权限
        /// </summary>
        /// <param name="account">帐户实例对象</param>
        /// <param name="actionName">权限所定义的业务处理器名称</param>
        /// <param name="controllerName">权限所定义的控制器名称</param>
        /// <param name="namespace">权限所在命名空间</param>
        /// <returns></returns>
        public static bool HasAuthority(IAccount account, string actionName, string controllerName, string @namespace = null)
        {
            return Provider.HasAuthority(account, actionName, controllerName, @namespace);
        }

        /// <summary>
        /// 确定当前用户是否拥有指定权限
        /// </summary>
        /// <param name="user">已授权用户</param>
        /// <param name="authorityID">权限ID</param>
        /// <returns></returns>
        public static bool HasAuthority(IPrincipal user, Guid authorityID)
        {
            return Provider.HasAuthority(user, authorityID);
        }

        /// <summary>
        /// 确定当前用户是否拥有指定权限
        /// </summary>
        /// <param name="account">帐户实例对象</param>
        /// <param name="authorityID">权限ID</param>
        /// <returns></returns>
        public static bool HasAuthority(IAccount account, Guid authorityID)
        {
            return Provider.HasAuthority(account, authorityID);
        }

        /// <summary>
        /// 获取当前已授权用户的帐户实例对象
        /// </summary>
        /// <param name="user">已授权帐户</param>
        /// <returns></returns>
        public static IAccount AuthenticatedUserAccount(IPrincipal user)
        {
            return Provider.AuthenticatedUserAccount(user);
        }

        /// <summary>
        /// 为当前用户分配角色
        /// </summary>
        /// <param name="account">帐户实例对象</param>
        /// <param name="roleIDs">角色ID</param>
        /// <returns></returns>
        public static bool AssignRoles(IAccount account, Guid[] roleIDs)
        {
            return Provider.AssignRoles(account, roleIDs);
        }

        /// <summary>
        /// 获取当前用户已分配的角色
        /// </summary>
        /// <param name="user">已授权用户</param>
        /// <returns></returns>
        public static IList<IRole> AssignedRoles(IPrincipal user)
        {
            return Provider.AssignedRoles(user);
        }

        /// <summary>
        /// 获取当前用户已分配的角色
        /// </summary>
        /// <param name="account">帐户实例对象</param>
        /// <returns></returns>
        public static IList<IRole> AssignedRoles(IAccount account)
        {
            return Provider.AssignedRoles(account);
        }

        /// <summary>
        /// 获取当前角色可被分配的权限
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static IList<IAuthority> CanAssignAuthorities(IRole role)
        {
            return Provider.CanAssignAuthorities(role);
        }

        /// <summary>
        /// 为当前角色分配权限
        /// </summary>
        /// <param name="role">角色实例对象</param>
        /// <param name="authorityIDs">权限ID</param>
        /// <returns></returns>
        public static bool AssignAuthorities(IRole role, Guid[] authorityIDs)
        {
            return Provider.AssignAuthorities(role, authorityIDs);
        }

        /// <summary>
        /// 获取当前角色已分配的权限
        /// </summary>
        /// <param name="role">角色实例对象</param>
        /// <returns></returns>
        public static IList<IAuthority> AssignedAuthorities(IRole role)
        {
            return Provider.AssignedAuthorities(role);
        }

        /// <summary>
        /// 为当前用户拒绝权限
        /// </summary>
        /// <param name="account">帐户实例对象</param>
        /// <param name="authorityIDs">权限ID</param>
        /// <returns></returns>
        public static bool DenyAuthorities(IAccount account, Guid[] authorityIDs)
        {
            return Provider.DenyAuthorities(account, authorityIDs);
        }

        /// <summary>
        /// 获取当前用户已取消的权限
        /// </summary>
        /// <param name="account">帐户实例对象</param>
        /// <returns></returns>
        public static IList<IAuthority> DeniedAuthorities(IAccount account)
        {
            return Provider.DeniedAuthorities(account);
        }

        /// <summary>
        /// 获取当前用户已取消的权限
        /// </summary>
        /// <param name="user">已授权用户</param>
        /// <returns></returns>
        public static IList<IAuthority> DeniedAuthorities(IPrincipal user)
        {
            return Provider.DeniedAuthorities(user);
        }
    }
}
