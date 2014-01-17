using System;
using System.Drawing;
using System.IO;

namespace YuYu.Components
{
    /// <summary>
    /// Gif动画编码器
    /// </summary>
    internal class AnimatedGifEncoder
    {
        /// <summary>
        /// 帧宽
        /// </summary>
        protected int _Width;

        /// <summary>
        /// 帧高
        /// </summary>
        protected int _Height;

        /// <summary>
        /// 指定的透明色
        /// </summary>
        protected Color _Transparent = Color.Empty;

        /// <summary>
        /// 指定透明色在调色板中的索引
        /// </summary>
        protected int _TransIndex;

        /// <summary>
        /// 动画循环次数（默认值-1不循环）
        /// </summary>
        protected int _Repeat = -1;

        /// <summary>
        /// 帧间隔时间
        /// </summary>
        protected int _Delay = 0;

        /// <summary>
        /// 开始输出帧
        /// </summary>
        protected bool _Started = false;
        
        /// <summary>
        /// 字节流
        /// </summary>
        protected Stream _Steam;

        /// <summary>
        /// 当前图像
        /// </summary>
        protected Image _Image; // current frame

        /// <summary>
        /// BGR的帧的字节数组
        /// </summary>
        protected byte[] _Pixels; // BGR byte array from frame
        protected byte[] _IndexedPixels; // converted frame indexed to palette
        protected int _ColorDepth; // number of bit planes
        protected byte[] _ColorTab; // RGB palette
        protected bool[] _UsedEntry = new bool[256]; // active palette entries
        protected int _PalSize = 7; // color table size (bits-1)
        protected int _Dispose = -1; // disposal code (-1 = use default)
        protected bool _CloseStream = false; // close stream when finished
        protected bool _FirstFrame = true;
        protected bool _SizeSet = false; // if false, get size from first frame
        protected int _Sample = 10; // default sample interval for quantizer

        /// <summary>
        /// 设置每帧之间的延迟时间，或改变后续的帧（适用于最后一帧）。
        /// </summary>
        /// <param name="ms">以毫秒为单位的延迟时间</param>
        public void SetDelay(int ms)
        {
            _Delay = (int)Math.Round(ms / 10.0f);
        }

        /// <summary>
        /// 设置GIF最后补充框架和任何后续帧帧处理代码。默认为0，如果没有透明的颜色已经成立，另有2。
        /// </summary>
        /// <param name="code">处置代码</param>
        public void SetDispose(int code)
        {
            if (code >= 0)
                _Dispose = code;
        }

        /// <summary>
        /// Sets the number of times the set of GIF frames should be played.  Default is 1; 0 means play indefinitely.  Must be invoked before the first image is added.
        /// 设置应播放的GIF帧集的次数（默认值是1），0则无限循环。
        /// 必须在第一帧添加前调用
        /// </summary>
        /// <param name="iter">循环次数</param>
        public void SetRepeat(int iter)
        {
            if (iter >= 0)
                _Repeat = iter;
        }

        /// <summary>
        /// 最后补充框架和任何后续帧设置透明色。
        /// 由于所有的颜色是在量化过程中的修改，在最后的调色板给定的颜色最接近的每一帧的颜色，成为该帧的透明色。
        /// 可设置为null，表示没有透明的颜色。
        /// </summary>
        /// <param name="color">被作为透明色进行显示的颜色</param>
        public void SetTransparent(Color color)
        {
            _Transparent = color;
        }

        /// <summary>
        /// 增加未来的GIF帧。帧不立即写入，但实际上是推迟到下一帧收到时序数据，可以插入。
        /// 调用 <code>finish()</code> 刷新所有帧
        /// 如果 <code>setSize</code> 没有被调用, 第一张图片的大小用于所有后续帧。
        /// </summary>
        /// <param name="image">图像</param>
        /// <returns>成功返回 true 否则返回 false</returns>
        public bool AddFrame(Image image)
        {
            if ((image == null) || !_Started)
                return false;
            bool ok = true;
            try
            {
                if (!_SizeSet)
                    SetSize(image.Width, image.Height);// 使用第一帧图像的尺寸
                _Image = image;
                GetImagePixels(); // 转换为正确的格式，如果有必要
                AnalyzePixels(); // build color table & map pixels建立颜色表及地图像素
                if (_FirstFrame)
                {
                    WriteLSD(); // logical screen descriptior
                    WritePalette(); // global color table
                    if (_Repeat >= 0)
                        WriteNetscapeExt(); // use NS app extension to indicate reps
                }
                WriteGraphicCtrlExt(); // write graphic control extension
                WriteImageDesc(); // image descriptor
                if (!_FirstFrame)
                    WritePalette(); // 本地调色板
                WritePixels(); // encode and write pixel data
                _FirstFrame = false;
            }
            catch (IOException)
            {
                ok = false;
            }
            return ok;
        }

        /// <summary>
        /// 完成写入所有缓存数据到文件并关闭文件流（如果将数据写入输出流，则不关闭流）
        /// </summary>
        /// <returns></returns>
        public bool Finish()
        {
            if (!_Started) return false;
            bool ok = true;
            _Started = false;
            try
            {
                _Steam.WriteByte(0x3b);
                _Steam.Flush();
                if (_CloseStream)
                    _Steam.Close();
            }
            catch (IOException)
            {
                ok = false;
            }
            //重置数据字段供接下来的使用
            _TransIndex = 0;
            _Steam = null;
            _Image = null;
            _Pixels = null;
            _IndexedPixels = null;
            _ColorTab = null;
            _CloseStream = false;
            _FirstFrame = true;
            return ok;
        }

        /// <summary>
        /// 设置帧速（等价于 <code>setDelay(1000/fps)</code>）
        /// </summary>
        /// <param name="fps">帧速</param>
        public void SetFrameRate(float fps)
        {
            if (fps != 0f)
                _Delay = (int)Math.Round(100f / fps);
        }

        /// <summary>
        /// 设置颜色量化的质量（图像转换为GIF规范所允许的最大的256颜色）。
        /// 下值（最低为1），产生更好的颜色，但缓慢的处理显着。
        /// 10是默认的，并产生良好的色彩映射在合理的速度。不屈服值大于20的速度显着改善。
        /// </summary>
        /// <param name="quality">大于零的整数</param>
        public void SetQuality(int quality)
        {
            if (quality < 1)
                quality = 1;
            _Sample = quality;
        }
        
        /// <summary>
        /// 设置GIF帧大小。
        /// 默认大小是调用此方法前的第一帧图像大小
        /// </summary>
        /// <param name="width">帧宽</param>
        /// <param name="height">帧高</param>
        public void SetSize(int width, int height)
        {
            if (_Started && !_FirstFrame)
                return;
            _Width = width;
            _Height = height;
            if (_Width < 1)
                _Width = 320;
            if (_Height < 1)
                _Height = 240;
            _SizeSet = true;
        }

        /// <summary>
        /// 在给定的流启动GIF文件的创建。该流不自动关闭。
        /// </summary>
        /// <param name="os">写入GIF图像的输出流</param>
        /// <returns>成功返回 true 否则返回 false</returns>
        public bool Start(Stream os)
        {
            if (os == null)
                return false;
            bool ok = true;
            _CloseStream = false;
            _Steam = os;
            try
            {
                WriteString("GIF89a"); //文件头
            }
            catch (IOException)
            {
                ok = false;
            }
            return _Started = ok;
        }

        /// <summary>
        /// 启动具有指定名称的一个GIF文件的编写
        /// </summary>
        /// <param name="fileName">字符串：输出文件的完全限定名</param>
        /// <returns>成功返回 true 否则返回 false</returns>
        public bool Start(string fileName)
        {
            bool ok = true;
            try
            {
                _Steam = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                ok = Start(_Steam);
                _CloseStream = true;
            }
            catch (IOException)
            {
                ok = false;
            }
            return _Started = ok;
        }

        /// <summary>
        /// 分析图像的颜色并创建色域
        /// </summary>
        protected void AnalyzePixels()
        {
            int len = _Pixels.Length;
            int nPix = len / 3;
            _IndexedPixels = new byte[nPix];
            NeuQuant nq = new NeuQuant(_Pixels, len, _Sample);
            _ColorTab = nq.Process();
            int k = 0;
            for (int i = 0; i < nPix; i++)
            {
                int index = nq.Map(_Pixels[k++] & 0xff, _Pixels[k++] & 0xff, _Pixels[k++] & 0xff);
                _UsedEntry[index] = true;
                _IndexedPixels[i] = (byte)index;
            }
            _Pixels = null;
            _ColorDepth = 8;
            _PalSize = 7;
            if (_Transparent != Color.Empty)
                _TransIndex = FindClosest(_Transparent);
        }

        /// <summary>
        /// 返回调色板中近似颜色的索引
        /// </summary>
        /// <param name="color">颜色值</param>
        /// <returns></returns>
        protected int FindClosest(Color color)
        {
            if (_ColorTab == null) return -1;
            int r = color.R;
            int g = color.G;
            int b = color.B;
            int minpos = 0;
            int dmin = 256 * 256 * 256;
            int len = _ColorTab.Length;
            for (int i = 0; i < len; )
            {
                int dr = r - (_ColorTab[i++] & 0xff);
                int dg = g - (_ColorTab[i++] & 0xff);
                int db = b - (_ColorTab[i] & 0xff);
                int d = dr * dr + dg * dg + db * db;
                int index = i / 3;
                if (_UsedEntry[index] && d < dmin)
                {
                    dmin = d;
                    minpos = index;
                }
                i++;
            }
            return minpos;
        }

        /// <summary>
        /// 提取到的字节数组“像素”的图像的像素
        /// </summary>
        protected void GetImagePixels()
        {
            int w = _Image.Width;
            int h = _Image.Height;
            if (w != _Width || h != _Height)
            {
                // create new image with right size/format
                Image temp = new Bitmap(_Width, _Height);
                Graphics g = Graphics.FromImage(temp);
                g.DrawImage(_Image, 0, 0);
                _Image = temp;
                g.Dispose();
            }
            _Pixels = new Byte[3 * _Image.Width * _Image.Height];
            int count = 0;
            Bitmap tempBitmap = new Bitmap(_Image);
            for (int th = 0; th < _Image.Height; th++)
            {
                for (int tw = 0; tw < _Image.Width; tw++)
                {
                    Color color = tempBitmap.GetPixel(tw, th);
                    _Pixels[count] = color.R;
                    count++;
                    _Pixels[count] = color.G;
                    count++;
                    _Pixels[count] = color.B;
                    count++;
                }
            }
        }

        /// <summary>
        /// 写入图形控制扩展
        /// </summary>
        protected void WriteGraphicCtrlExt()
        {
            _Steam.WriteByte(0x21); // extension introducer
            _Steam.WriteByte(0xf9); // GCE label
            _Steam.WriteByte(4); // data block size
            int transp, disp;
            if (_Transparent == Color.Empty)
            {
                transp = 0;
                disp = 0; // dispose = no action
            }
            else
            {
                transp = 1;
                disp = 2; // force clear if using transparent color
            }
            if (_Dispose >= 0)
                disp = _Dispose & 7; // user override
            disp <<= 2;

            // packed fields
            _Steam.WriteByte(Convert.ToByte(0 | // 1:3 reserved
                disp | // 4:6 disposal
                0 | // 7   user input - 0 = none
                transp)); // 8   transparency flag

            WriteShort(_Delay); // delay x 1/100 sec
            _Steam.WriteByte(Convert.ToByte(_TransIndex)); // transparent color index
            _Steam.WriteByte(0); // block terminator
        }

        /// <summary>
        /// 写入图像描述符
        /// </summary>
        protected void WriteImageDesc()
        {
            _Steam.WriteByte(0x2c); // image separator
            WriteShort(0); // image position x,y = 0,0
            WriteShort(0);
            WriteShort(_Width); // image size
            WriteShort(_Height);
            // packed fields
            if (_FirstFrame)
                _Steam.WriteByte(0);// no LCT  - GCT is used for first (or only) frame
            else
            {
                // specify normal LCT
                _Steam.WriteByte(Convert.ToByte(0x80 | // 1 local color table  1=yes
                    0 | // 2 interlace - 0=no
                    0 | // 3 sorted - 0=no
                    0 | // 4-5 reserved
                    _PalSize)); // 6-8 size of color table
            }
        }

        /// <summary>
        /// 写入逻辑屏幕描述
        /// </summary>
        protected void WriteLSD()
        {
            //逻辑屏幕大小
            WriteShort(_Width);
            WriteShort(_Height);
            // packed fields
            _Steam.WriteByte(Convert.ToByte(0x80 | // 1   : global color table flag = 1 (gct used)
                0x70 | // 2-4 : color resolution = 7
                0x00 | // 5   : gct sort flag = 0
                _PalSize)); // 6-8 : gct size
            _Steam.WriteByte(0); // background color index
            _Steam.WriteByte(0); // pixel aspect ratio - assume 1:1
        }

        /// <summary>
        /// 写入Netscape的应用程序扩展，定义循环次数。
        /// </summary>
        protected void WriteNetscapeExt()
        {
            _Steam.WriteByte(0x21); // extension introducer
            _Steam.WriteByte(0xff); // app extension label
            _Steam.WriteByte(11); // block size
            WriteString("NETSCAPE" + "2.0"); // app id + auth code
            _Steam.WriteByte(3); // sub-block size
            _Steam.WriteByte(1); // loop sub-block id
            WriteShort(_Repeat); // 循环次数（0则无限循环）
            _Steam.WriteByte(0); // block terminator
        }

        /// <summary>
        /// 写入颜色表
        /// </summary>
        protected void WritePalette()
        {
            _Steam.Write(_ColorTab, 0, _ColorTab.Length);
            int n = (3 * 256) - _ColorTab.Length;
            for (int i = 0; i < n; i++)
                _Steam.WriteByte(0);
        }

        /// <summary>
        /// 编码并写入像素数据
        /// </summary>
        protected void WritePixels()
        {
            LZWEncoder encoder = new LZWEncoder(_Width, _Height, _IndexedPixels, _ColorDepth);
            encoder.Encode(_Steam);
        }

        /// <summary>
        /// 写入到输出流的16位值，LSB在前
        /// </summary>
        /// <param name="value"></param>
        protected void WriteShort(int value)
        {
            _Steam.WriteByte(Convert.ToByte(value & 0xff));
            _Steam.WriteByte(Convert.ToByte((value >> 8) & 0xff));
        }

        /// <summary>
        /// 将字符串写入到输出流
        /// </summary>
        /// <param name="str">字符串</param>
        protected void WriteString(string str)
        {
            char[] chars = str.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
                _Steam.WriteByte((byte)chars[i]);
        }
    }
}