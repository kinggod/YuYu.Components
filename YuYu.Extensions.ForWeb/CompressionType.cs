using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public enum CompressionType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,

        /// <summary>
        /// GZip压缩
        /// </summary>
        GZip,

        /// <summary>
        /// Deflate压缩
        /// </summary>
        Deflate,
    }
}
