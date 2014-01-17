using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 记录
        /// </summary>
        Record = 1,

        /// <summary>
        /// 通知
        /// </summary>
        Notice = 2,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 3,

        /// <summary>
        /// 缺陷
        /// </summary>
        Bug = 4,
    }
}
