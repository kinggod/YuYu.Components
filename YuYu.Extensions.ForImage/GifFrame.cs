using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace YuYu.Components
{
    /// <summary>
    /// Gif动画帧
    /// </summary>
    internal class GifFrame
    {
        /// <summary>
        /// 初始化一个Gif动画帧
        /// </summary>
        /// <param name="image"></param>
        /// <param name="delay"></param>
        public GifFrame(Image image, int delay)
        {
            this.Image = image;
            this.Delay = delay;
        }

        /// <summary>
        /// 图片
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// 延时
        /// </summary>
        public int Delay { get; set; }
    }
}
