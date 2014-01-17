using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 会员关系提供程序
    /// </summary>
    public interface IMembershipProvider
    {
        /// <summary>
        /// 获取当前系统所有可用的权限
        /// </summary>
        /// <returns></returns>
        IList<IAuthority> GetAllAuthorities();

        /// <summary>
        /// 确定当前用户是否拥有指定的角色
        /// </summary>
        /// <param name="user">已授权用户</param>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        bool IsInRole(IPrincipal user, Guid roleID);

        /// <summary>
        /// 确定当前用户是否拥有指定的角色
        /// </summary>
        /// <param name="account">帐户实例对象</param>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        bool IsInRole(IAccount account, Guid roleID);

        /// <summary>
        /// 确定当前用户是否拥有指定的权限
        /// </summary>
        /// <param name="user">已授权用户</param>
        /// <param name="actionName">权限所定义的业务处理器名称</param>
        /// <param name="controllerName">权限所定义的控制器名称</param>
        /// <param name="namespace">权限所在命名空间</param>
        /// <returns></returns>
        bool HasAuthority(IPrincipal user, string actionName, string controllerName, string @namespace = null);

        /// <summary>
        /// 确定当前用户是否拥有指定的权限
        /// </summary>
        /// <param name="account">帐户实例对象</param>
        /// <param name="actionName">权限所定义的业务处理器名称</param>
        /// <param name="controllerName">权限所定义的控制器名称</param>
        /// <param name="namespace">权限所在命名空间</param>
        /// <returns></returns>
        bool HasAuthority(IAccount account, string actionName, string controllerName, string @namespace = null);

        /// <summary>
        /// 确定当前用户是否拥有指定权限
        /// </summary>
        /// <param name="user">已授权用户</param>
        /// <param name="authorityID">权限ID</param>
        /// <returns></returns>
        bool HasAuthority(IPrincipal user, Guid authorityID);

        /// <summary>
        /// 确定当前用户是否拥有指定权限
        /// </summary>
        /// <param name="account">帐户实例对象</param>
        /// <param name="authorityID">权限ID</param>
        /// <returns></returns>
        bool HasAuthority(IAccount account, Guid authorityID);

        /// <summary>
        /// 获取当前已授权用户的帐户实例对象
        /// </summary>
        /// <param name="user">已授权帐户</param>
        /// <returns></returns>
        IAccount AuthenticatedUserAccount(IPrincipal user);

        /// <summary>
        /// 为当前用户分配角色
        /// </summary>
        /// <param name="account">帐户实例对象</param>
        /// <param name="roleIDs">角色ID</param>
        /// <returns></returns>
        bool AssignRoles(IAccount account, Guid[] roleIDs);

        /// <summary>
        /// 获取当前用户已分配的角色
        /// </summary>
        /// <param name="user">已授权用户</param>
        /// <returns></returns>
        IList<IRole> AssignedRoles(IPrincipal user);

        /// <summary>
        /// 获取当前用户已分配的角色
        /// </summary>
        /// <param name="account">帐户实例对象</param>
        /// <returns></returns>
        IList<IRole> AssignedRoles(IAccount account);

        /// <summary>
        /// 获取当前角色可被分配的权限
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        IList<IAuthority> CanAssignAuthorities(IRole role);

        /// <summary>
        /// 为当前角色分配权限
        /// </summary>
        /// <param name="role">角色实例对象</param>
        /// <param name="authorityIDs">权限ID</param>
        /// <returns></returns>
        bool AssignAuthorities(IRole role, Guid[] authorityIDs);

        /// <summary>
        /// 获取当前角色已分配的权限
        /// </summary>
        /// <param name="role">角色实例对象</param>
        /// <returns></returns>
        IList<IAuthority> AssignedAuthorities(IRole role);

        /// <summary>
        /// 为当前用户拒绝权限
        /// </summary>
        /// <param name="account">帐户实例对象</param>
        /// <param name="authorityIDs">权限ID</param>
        /// <returns></returns>
        bool DenyAuthorities(IAccount account, Guid[] authorityIDs);

        /// <summary>
        /// 获取当前用户已取消的权限
        /// </summary>
        /// <param name="account">帐户实例对象</param>
        /// <returns></returns>
        IList<IAuthority> DeniedAuthorities(IAccount account);

        /// <summary>
        /// 获取当前用户已取消的权限
        /// </summary>
        /// <param name="user">已授权用户</param>
        /// <returns></returns>
        IList<IAuthority> DeniedAuthorities(IPrincipal user);
    }
}
