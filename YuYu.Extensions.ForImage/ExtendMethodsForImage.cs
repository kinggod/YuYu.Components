using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Drawing.Drawing2D;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtendMethodsForImage
    {
        /// <summary>
        /// 输出验证码，该验证码默认由[A-Za-z0-9]组成(如无指定字符集characters)
        /// 默认字体包含："Verdana", "Tahoma", "Arial", "Helvetica Neue", "Helvetica", "Sans - Serif"
        /// 实际包含字体以参数指定及程序所在服务器安装字体及以上字体共有列表为准
        /// </summary>
        /// <param name="image">底图</param>
        /// <param name="verifyCode">返回的验证码字符串</param>
        /// <param name="characters">自定义字符集</param>
        /// <param name="minCount">最小显示字符数量</param>
        /// <param name="maxCount">最多显示字符数量</param>
        /// <param name="fontSize">字号</param>
        /// <param name="fontFamilies">字体</param>
        /// <returns></returns>
        public static Image DrawCharacters(this Image image, out string verifyCode, string characters, int minCount, int maxCount, int fontSize, params string[] fontFamilies)
        {
            Random r = _GetRandom();
            string codeSource = string.IsNullOrWhiteSpace(characters) ? "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789" : characters;
            string[] fontFams = fontFamilies;
            if (fontFams == null || fontFams.Length == 0)
                fontFams = new string[] { "Verdana", "Tahoma", "Arial", "Helvetica Neue", "Helvetica", "Sans - Serif" };
            FontStyle[] fontStyles = { FontStyle.Bold, FontStyle.Italic, FontStyle.Regular };
            Graphics g = Graphics.FromImage(image);
            Pen pen = null;
            Font font = null;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;//设置高质量插值法
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
            //设置图片背景
            g.Clear(_BackColor());
            //绘制干扰线
            for (int i = 0; i < 24; i++)
            {
                int x = r.Next(image.Width);
                int y = r.Next(image.Height);
                pen = new Pen(_BackColor());
                g.DrawLine(pen, x, y, x + r.Next(30) * (r.Next(3) - 1), y + r.Next(30) * (r.Next(3) - 1));
                pen.Dispose();
            }
            //绘制字符
            int cCount = r.Next(maxCount - minCount + 1) + minCount;//绘制字符数
            float cWidth = (float)(image.Width - 10) / cCount;//字符所占宽度
            StringBuilder tempCode = new StringBuilder(cCount);
            for (int i = 0; i < cCount; i++)
            {
                char c = codeSource[r.Next(codeSource.Length)];
                font = new Font(fontFams[r.Next(fontFams.Length)], fontSize, fontStyles[r.Next(fontStyles.Length)]);
                pen = new Pen(_FontColor());
                g.DrawString(c.ToString(), font, pen.Brush, (5 + cWidth * i), r.Next(2, image.Height - fontSize - 2));
                font.Dispose();
                pen.Dispose();
                tempCode.Append(c);
            }
            verifyCode = tempCode.ToString();
            return image;
        }

        /// <summary>
        /// 输出验证码，该验证码由汉字组成
        /// 默认字体包含："宋体", "华文仿宋", "幼圆", "黑体", "楷体", "华文新魏", "方正姚体", "华文行楷", "华文细黑", "隶书"
        /// 实际包含字体以参数指定及程序所在服务器安装字体及以上字体共有列表为准
        /// </summary>
        /// <param name="image">底图</param>
        /// <param name="verifyCode">返回的验证码字符串</param>
        /// <param name="minCount">最小显示字符数量</param>
        /// <param name="maxCount">最多显示字符数量</param>
        /// <param name="fontSize">字号</param>
        /// <param name="encoding">编码格式：简体中文(GBK)，繁体中文(BIG5)</param>
        /// <param name="fontFamilies">字体</param>
        /// <returns></returns>
        public static Image DrawCharacters(this Image image, out string verifyCode, int minCount, int maxCount, int fontSize, string encoding = "GBK", params string[] fontFamilies)
        {
            Random r = _GetRandom();
            string[] fontFams = fontFamilies;
            if (fontFams == null || fontFams.Length == 0)
                fontFams = new string[] { "宋体", "华文仿宋", "幼圆", "黑体", "楷体", "华文新魏", "方正姚体", "华文行楷", "华文细黑", "隶书" };
            FontStyle[] fontStyles = { FontStyle.Italic, FontStyle.Regular };
            Graphics g = Graphics.FromImage(image);
            Pen pen = null;
            Font font = null;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;//设置高质量插值法
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
            //设置图片背景
            g.Clear(_BackColor());
            //绘制干扰线
            for (int i = 0; i < 24; i++)
            {
                int x = r.Next(image.Width);
                int y = r.Next(image.Height);
                pen = new Pen(_BackColor());
                g.DrawLine(pen, x, y, x + r.Next(30) * (r.Next(3) - 1), y + r.Next(30) * (r.Next(3) - 1));
                pen.Dispose();
            }
            //绘制字符
            int cCount = r.Next(maxCount - minCount + 1) + minCount;//绘制字符数
            float cWidth = (float)(image.Width - 10) / cCount;//字符所占宽度
            Encoding gb = encoding == null ? Encoding.GetEncoding("GBK") : Encoding.GetEncoding(encoding);
            object[] bytes = _RegionCode(cCount);
            StringBuilder tempCode = new StringBuilder(cCount);
            for (int i = 0; i < cCount; i++)
            {
                string c = gb.GetString((byte[])Convert.ChangeType(bytes[i], typeof(byte[])));
                font = new Font(fontFams[r.Next(fontFams.Length)], fontSize, fontStyles[r.Next(fontStyles.Length)]);
                pen = new Pen(_FontColor());
                g.DrawString(c, font, pen.Brush, (5 + cWidth * i), r.Next(2, image.Height - fontSize - 2));
                font.Dispose();
                pen.Dispose();
                tempCode.Append(c);
            }
            verifyCode = tempCode.ToString();
            return image;
        }

        /// <summary>
        /// 输出验证码，该验证码由不超过3位非负整数的加减乘除方程式构成，值为该方程式运算结果
        /// 默认字体包含："Verdana", "Tahoma", "Arial", "Helvetica Neue", "Helvetica", "Sans - Serif"
        /// 实际包含字体以参数指定及程序所在服务器安装字体及以上字体共有列表为准
        /// </summary>
        /// <param name="image">底图</param>
        /// <param name="verifyCode">运算结果</param>
        /// <param name="maxA">加法算术式各数字最大值</param>
        /// <param name="maxB">减法算数字被减数最大值</param>
        /// <param name="maxC">乘法算术式各数字最大值</param>
        /// <param name="maxD">除法算术式被除数最大值</param>
        /// <param name="fontSize">字号</param>
        /// <param name="questionMark">是否显示“?”</param>
        /// <param name="fontFamilies">字体</param>
        /// <returns></returns>
        public static Image DrawCharacters(this Image image, out int verifyCode, int maxA, int maxB, int maxC, int maxD, int fontSize, bool questionMark = true, params string[] fontFamilies)
        {
            Random r = _GetRandom();
            char[] operators = { '+', '-', '×', '÷' };
            if (fontFamilies == null || fontFamilies.Length == 0)
                fontFamilies = new string[] { "Verdana", "Tahoma", "Arial", "Helvetica Neue", "Helvetica", "Sans - Serif" };
            FontStyle[] fontStyles = { FontStyle.Bold, FontStyle.Italic, FontStyle.Regular };
            Graphics g = Graphics.FromImage(image);
            Pen pen = null;
            Font font = null;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;//设置高质量插值法
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
            //设置图片背景
            g.Clear(_BackColor());
            //绘制干扰线
            for (int i = 0; i < 24; i++)
            {
                int x = r.Next(image.Width);
                int y = r.Next(image.Height);
                pen = new Pen(_BackColor());
                g.DrawLine(pen, x, y, x + r.Next(30) * (r.Next(3) - 1), y + r.Next(30) * (r.Next(3) - 1));
                pen.Dispose();
            }
            int a, b, c;
            char o = operators[r.Next(operators.Length)];
            switch (o)
            {
                case '+':
                    a = r.Next(maxA);
                    b = r.Next(maxA);
                    c = a + b;
                    break;
                case '-':
                    int tmp = maxB / 2;
                    a = r.Next(tmp, maxB);
                    b = r.Next(tmp);
                    c = a - b;
                    break;
                case '×':
                    a = r.Next(maxC);
                    b = r.Next(maxC);
                    c = a * b;
                    break;
                case '÷':
                    a = r.Next(maxD);
                    IList<int> divisors = _GetDivisors(a);
                    b = divisors[r.Next(divisors.Count)];
                    c = a / b;
                    break;
                default:
                    o = '+';
                    a = r.Next(maxA);
                    b = r.Next(maxA);
                    c = a + b;
                    break;
            }
            IList<string> equation = new List<string> { a.ToString(), o.ToString(), b.ToString(), "=" };
            if (questionMark)
                equation.Add("?");
            //绘制字符
            int cCount = equation[0].Length + equation[2].Length + 3;
            float cWidth = (float)(image.Width - 10) / cCount;//字符所占宽度
            float cx = 5;
            StringBuilder tempCode = new StringBuilder(cCount);
            for (int i = 0; i < equation.Count; i++)
            {
                string ch = equation[i];
                font = new Font(fontFamilies[r.Next(fontFamilies.Length)], fontSize, fontStyles[r.Next(fontStyles.Length)]);
                pen = new Pen(_FontColor());
                g.DrawString(ch.ToString(), font, pen.Brush, cx, r.Next(2, image.Height - fontSize - 2));
                cx += equation[i].Length * cWidth;
                font.Dispose();
                pen.Dispose();
            }
            verifyCode = c;
            return image;
        }

        /// <summary>
        /// 绘制背景色
        /// </summary>
        /// <param name="image">源图片</param>
        /// <param name="color">背景色</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="zoom">等比缩放</param>
        /// <returns></returns>
        public static Image DrawBackground(this Image image, Color color, int width, int height, bool zoom = true)
        {
            Image background = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(background);
            graphics.InterpolationMode = InterpolationMode.High;//设置高质量插值法
            graphics.SmoothingMode = SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
            graphics.Clear(color); //清空画布并以指定背景色填充
            image = image.Resize(width, height, zoom);
            graphics.DrawImage(image, new Point((background.Width - image.Width) / 2, (background.Height - image.Height) / 2));
            graphics.Dispose();
            return background;
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="image">源图片</param>
        /// <param name="start">裁剪开始坐标</param>
        /// <param name="end">裁剪结束坐标</param>
        /// <returns></returns>
        public static Image Crop(this Image image, Point start, Point end)
        {
            return Crop(image, start.X, start.Y, end.X - start.X, end.Y - start.Y);
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="image">源图片</param>
        /// <param name="start">裁剪开始坐标</param>
        /// <param name="size">裁剪尺寸</param>
        /// <returns></returns>
        public static Image Crop(this Image image, Point start, Size size)
        {
            return Crop(image, start.X, start.Y, size.Width, size.Height);
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="image">源图片</param>
        /// <param name="start">裁剪开始坐标</param>
        /// <param name="width">裁剪宽度</param>
        /// <param name="height">裁剪高度</param>
        /// <returns></returns>
        public static Image Crop(this Image image, Point start, int width, int height)
        {
            return Crop(image, start.X, start.Y, width, height);
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="image">源图片</param>
        /// <param name="startX">裁剪开始 X 坐标</param>
        /// <param name="startY">裁剪开始 Y 坐标</param>
        /// <param name="width">裁剪宽度</param>
        /// <param name="height">裁剪高度</param>
        /// <returns></returns>
        public static Image Crop(this Image image, int startX, int startY, int width, int height)
        {
            if (startX >= image.Width || startY >= image.Height || width <= 0 || height <= 0)
                return image;
            if (startX < 0)
                startX = 0;
            if (startY < 0)
                startY = 0;
            if (width > image.Width - startX)
                width = image.Width - startX;
            if (height > image.Height - startY)
                height = image.Height - startY;
            if (image.RawFormat.Guid == ImageFormat.Gif.Guid)
            {
                int w = image.Width;
                int h = image.Height;
                GifDecoder GifDecoder = new GifDecoder();
                GifDecoder.Read(image);
                int count = GifDecoder.FramesCount;
                AnimatedGifEncoder age = new AnimatedGifEncoder();
                MemoryStream ms = new MemoryStream();
                age.Start(ms);
                age.SetRepeat(GifDecoder.LoopCount);
                for (int i = 0; i < count; i++)
                {
                    Image frame = new Bitmap(width, height);
                    Graphics g = Graphics.FromImage(frame);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;//设置高质量插值法
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
                    g.Clear(Color.Transparent); //清空画布并以透明背景色填充
                    g.DrawImage(GifDecoder.GetFrame(i), -startX, -startY, w, h);
                    g.Dispose();
                    age.SetDelay(GifDecoder.Delay(i));
                    age.AddFrame(frame);
                }
                age.Finish();
                return Image.FromStream(ms);
            }
            else
            {
                Image @new = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(@new);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;//设置高质量插值法
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
                g.Clear(Color.Transparent); //清空画布并以透明背景色填充
                g.DrawImage(image, -startX, -startY, image.Width, image.Height);
                g.Dispose();
                return @new;
            }
        }

        /// <summary>
        /// 调整大小
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="zoom">等比缩放</param>
        /// <returns></returns>
        public static Image Resize(this Image image, int? width, int? height, bool zoom = true)
        {
            if (width == null && height == null)
                return image;
            int w = image.Width, h = image.Height;
            if (width != null && height != null)
            {
                double wW = (double)image.Width / width.Value, hH = (double)image.Height / height.Value;
                if (zoom)
                {
                    if (wW > hH)
                    {
                        w = width.Value;
                        h = (int)(image.Height / wW);
                    }
                    else
                    {
                        w = (int)(image.Width / hH);
                        h = height.Value;
                    }
                }
                else
                {
                    w = width.Value;
                    h = height.Value;
                }
            }
            else if (width != null)
            {
                w = width.Value;
                h = zoom ? (int)(image.Height / ((double)image.Width / width.Value)) : image.Height;
            }
            else if (height != null)
            {
                w = zoom ? (int)(image.Width / ((double)image.Height / height.Value)) : image.Width;
                h = height.Value;
            }
            if (image.Width == w && image.Height == h)
                return image;
            if (image.RawFormat.Guid == ImageFormat.Gif.Guid)
            {
                GifDecoder GifDecoder = new GifDecoder();
                GifDecoder.Read(image);
                int count = GifDecoder.FramesCount;
                AnimatedGifEncoder e = new AnimatedGifEncoder();
                MemoryStream ms = new MemoryStream();
                e.Start(ms);
                //int loopCount = 0;
                e.SetRepeat(GifDecoder.LoopCount);
                //int j = loopCount;
                for (int i = 0; i < count; i++)
                {
                    Image frame = GifDecoder.GetFrame(i).GetThumbnailImage(w, h, () => { return false; }, IntPtr.Zero);
                    e.SetDelay(GifDecoder.Delay(i));
                    e.AddFrame(frame);
                }
                e.Finish();
                Image gif = Image.FromStream(ms);
                ms.Dispose();
                return gif;
            }
            else
            {
                Bitmap tmp1 = new Bitmap(image);
                Image tmp2 = tmp1.GetThumbnailImage(w, h, () => { return false; }, IntPtr.Zero);
                Bitmap result = new Bitmap(tmp2);
                image.Dispose();
                tmp1.Dispose();
                tmp2.Dispose();
                return result;
            }
        }

        /// <summary>
        /// 绘制水印
        /// </summary>
        /// <param name="image"></param>
        /// <param name="mark">水印图片</param>
        /// <param name="width">水印宽度</param>
        /// <param name="height">水印高度</param>
        /// <param name="startX">水印左上角水平位置</param>
        /// <param name="startY">水印左上角垂直位置</param>
        /// <param name="opacity">透明度</param>
        /// <returns></returns>
        public static Image DrawWatermark(this Image image, Image mark, int startX, int startY, int width, int height, float opacity = 0.5F)
        {
            return DrawWatermark(image, mark, new Point(startX, startY), new Size(width, height), opacity);
        }

        /// <summary>
        /// 绘制水印
        /// </summary>
        /// <param name="image"></param>
        /// <param name="mark">水印图片</param>
        /// <param name="start">水印左上角坐标</param>
        /// <param name="size">水印大小</param>
        /// <param name="opacity">透明度</param>
        /// <returns></returns>
        public static Image DrawWatermark(this Image image, Image mark, Point start, Size size, float opacity = 0.5F)
        {
            if (opacity > 1)
                opacity = 1;
            else if (opacity < 0)
                opacity = 0;
            float[][] ptsArray = { new float[] { 1, 0, 0, 0, 0 }, new float[] { 0, 1, 0, 0, 0 }, new float[] { 0, 0, 1, 0, 0 }, new float[] { 0, 0, 0, opacity, 0 }, new float[] { 0, 0, 0, 0, 1 } };
            ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
            ImageAttributes imgAttributes = new ImageAttributes();
            imgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            Graphics g = Graphics.FromImage(image);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;//设置高质量插值法
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
            g.DrawImage(mark, new Rectangle(start, size), 0, 0, mark.Width, mark.Height, GraphicsUnit.Pixel, imgAttributes);
            g.Dispose();
            return image;
        }

        /// <summary>
        /// 组合多张图片
        /// </summary>
        /// <param name="image">底图</param>
        /// <param name="images">图片集合</param>
        /// <param name="zoom">等比缩放</param>
        /// <returns></returns>
        public static Image Combine(this Image image, IList<Image> images, bool zoom = true)
        {
            if (images != null && images.Count() > 0)
            {
                Graphics g = Graphics.FromImage(image);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;//设置高质量插值法
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
                g.Clear(Color.Transparent);
                foreach (Image img in images)
                {
                    Image s = img;
                    if (zoom)
                        s = Resize(img, image.Width, image.Height, zoom);
                    g.DrawImage(s, new Point((image.Width - s.Width) / 2, (image.Height - s.Height) / 2));
                }
                g.Dispose();
                return image;
            }
            else
                return image;
        }

        /// <summary>
        /// 将多张图片组合成一个GIF动画
        /// </summary>
        /// <param name="image">底图</param>
        /// <param name="images">图片字典（Keys：图片集合；Values：帧延迟）</param>
        /// <param name="zoom">等比缩放</param>
        /// <param name="loopCount">GIF循环次数（0为无限循环）</param>
        /// <returns></returns>
        public static Image Combine(this Image image, IDictionary<Image, int> images, bool zoom = true, int loopCount = 0)
        {
            if (loopCount < 0)
                throw new ArgumentException("必须为非负整数值！", "loopCount");
            if (images != null && images.Count() > 0)
            {
                AnimatedGifEncoder animatedGifEncoder = new AnimatedGifEncoder();
                MemoryStream memoryStream = new MemoryStream();
                animatedGifEncoder.Start(memoryStream);
                animatedGifEncoder.SetRepeat(loopCount);
                animatedGifEncoder.SetDelay(0);
                animatedGifEncoder.AddFrame(image);
                foreach (Image img in images.Keys)
                {
                    Image frame = Resize(img, image.Width, image.Height, zoom);
                    animatedGifEncoder.SetDelay(images[img]);
                    animatedGifEncoder.AddFrame(frame);
                }
                animatedGifEncoder.Finish();
                return Image.FromStream(memoryStream);
            }
            else
                return image;
        }

        #region

        private static IList<int> _GetDivisors(int number)
        {
            if (number >= 0)
            {
                if (number == 0)
                    return new List<int> { 1 };
                IList<int> divisors = new List<int>(2);
                divisors.Add(1);
                for (int i = 2; i < number; i++)
                {
                    if (number % i == 0)
                        divisors.Add(i);
                }
                if (number > 1)
                    divisors.Add(number);
                return divisors;
            }
            return null;
        }

        private static Random _Random;
        private static Random _GetRandom()
        {
            if (_Random == null)
                _Random = new Random();
            return _Random;
        }

        /// <summary>
        /// 随机的背景色
        /// </summary>
        private static Color _BackColor()
        {
            Random random = _GetRandom();
            return Color.FromArgb(random.Next(150) + 100, random.Next(150) + 100, random.Next(150) + 100);
        }

        /// <summary>
        /// 随机的字体色
        /// </summary>
        private static Color _FontColor()
        {
            Random random = _GetRandom();
            return Color.FromArgb(random.Next(150), random.Next(150), random.Next(150));
        }

        /// <summary>
        /// 输出随机汉字
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns></returns>
        private static object[] _RegionCode(int count)
        {
            //定义一个字符串数组储存汉字编码的组成元素   
            string[] rBase = new String[16] { "0 ", "1 ", "2 ", "3 ", "4 ", "5 ", "6 ", "7 ", "8 ", "9 ", "a ", "b ", "c ", "d ", "e ", "f " };
            Random r = new Random();
            //定义一个object数组用来   
            object[] bytes = new object[count];
            //每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bject数组中
            //每个汉字有四个区位码组成
            //区位码第1位和区位码第2位作为字节数组第一个元素
            //区位码第3位和区位码第4位作为字节数组第二个元素
            for (int i = 0; i < count; i++)
            {
                //区位码第1位   
                int r1 = r.Next(11, 14);
                string str_r1 = rBase[r1].Trim();
                //区位码第2位   
                r = new Random(r1 * unchecked((int)DateTime.Now.Ticks) + i);//更换随机数发生器的种子避免产生重复值
                int r2;
                if (r1 == 13)
                    r2 = r.Next(0, 7);
                else
                    r2 = r.Next(0, 16);
                string str_r2 = rBase[r2].Trim();
                //区位码第3位   
                r = new Random(r2 * unchecked((int)DateTime.Now.Ticks) + i);
                int r3 = r.Next(10, 16);
                string str_r3 = rBase[r3].Trim();
                //区位码第4位   
                r = new Random(r3 * unchecked((int)DateTime.Now.Ticks) + i);
                int r4;
                if (r3 == 10)
                    r4 = r.Next(1, 16);
                else if (r3 == 15)
                    r4 = r.Next(0, 15);
                else
                    r4 = r.Next(0, 16);
                string str_r4 = rBase[r4].Trim();
                //定义两个字节变量存储产生的随机汉字区位码   
                byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);
                //将两个字节变量存储在字节数组中   
                byte[] str_r = new byte[] { byte1, byte2 };
                //将产生的一个汉字的字节数组放入object数组中   
                bytes.SetValue(str_r, i);
            }
            return bytes;
        }

        #endregion
    }
}
