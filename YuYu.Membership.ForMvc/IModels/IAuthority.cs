using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 权限接口
    /// </summary>
    public interface IAuthority
    {
        /// <summary>
        /// ID
        /// </summary>
        Guid ID { get; }

        /// <summary>
        /// 命名空间
        /// </summary>
        string Namespace { get; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        string ControllerName { get; }

        /// <summary>
        /// 业务处理器名称
        /// </summary>
        string ActionName { get; }
    }
}
