using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 上传结果
    /// </summary>
    public enum UploadResult
    {
        /// <summary>
        /// 拒绝的格式
        /// </summary>
        Denied,

        /// <summary>
        /// 上传出错
        /// </summary>
        Error,

        /// <summary>
        /// 文件太大
        /// </summary>
        TooLarge,

        /// <summary>
        /// 成功
        /// </summary>
        Success,
    }
}
