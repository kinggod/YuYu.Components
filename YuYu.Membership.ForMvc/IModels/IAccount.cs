using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 帐户接口
    /// </summary>
    public interface IAccount
    {
        /// <summary>
        /// ID
        /// </summary>
        Guid ID { get; }
    }
}
