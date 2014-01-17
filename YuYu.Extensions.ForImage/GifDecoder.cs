using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    internal class GifDecoder
    {
        /// <summary>
        /// 文件读取结果：正常
        /// </summary>
        public const int STATUS_OK = 0;

        /// <summary>
        /// 文件读取结果：格式错误
        /// </summary>
        public const int STATUS_FORMAT_ERROR = 1;

        /// <summary>
        /// 文件读取结果：读取失败
        /// </summary>
        public const int STATUS_OPEN_ERROR = 2;

        protected Stream inStream;
        protected int status;

        protected int width; // full image width
        protected int height; // full image height
        protected bool gctFlag; // global color table used
        protected int gctSize; // size of global color table
        protected int loopCount = 1; // iterations; 0 = repeat forever

        protected int[] gct; // global color table
        protected int[] lct; // local color table
        protected int[] act; // active color table

        protected int bgIndex; // background color index
        protected int bgColor; // background color
        protected int lastBgColor; // previous bg color
        protected int pixelAspect; // pixel aspect ratio

        protected bool lctFlag; // local color table flag
        protected bool interlace; // interlace flag
        protected int lctSize; // local color table size

        protected int ix, iy, iw, ih; // current image rectangle
        protected Rectangle lastRect; // last image rect
        protected Image image; // current frame
        protected Bitmap bitmap;
        protected Image lastImage; // previous frame

        protected byte[] block = new byte[256]; // current data block
        protected int blockSize = 0; // block size

        // last graphic control extension info
        protected int dispose = 0;
        // 0=no action; 1=leave in place; 2=restore to bg; 3=restore to prev
        protected int lastDispose = 0;
        protected bool transparency = false; // use transparent color
        protected int delay = 0; // delay in milliseconds
        protected int transIndex; // transparent color index

        protected static readonly int MaxStackSize = 4096;
        // max decoder pixel stack size

        // LZW decoder working arrays
        protected short[] prefix;
        protected byte[] suffix;
        protected byte[] pixelStack;
        protected byte[] pixels;

        protected ArrayList frames; // frames read from current file
        protected int frameCount;

        /// <summary>
        /// 指定帧的延时时间
        /// </summary>
        /// <param name="index">帧索引</param>
        /// <returns></returns>
        public int Delay(int index)
        {
            delay = -1;
            if (index >= 0 && index < frameCount)
                delay = ((GifFrame)frames[index]).Delay;
            return delay;
        }

        /// <summary>
        /// 总帧数
        /// </summary>
        public int FramesCount
        {
            get { return frameCount; }
        }

        /// <summary>
        /// 第一帧图像
        /// </summary>
        /// <returns></returns>
        public Image FirstFrame
        {
            get { return GetFrame(0); }
        }

        /// <summary>
        /// 动画循环次数(0则无限循环)
        /// </summary>
        /// <returns></returns>
        public int LoopCount
        {
            get { return loopCount; }
        }

        /// <summary>
        /// Creates new frame image from current data (and previous frames as specified by their disposition codes).
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private int[] GetPixels(Bitmap bitmap)
        {
            int[] pixels = new int[3 * image.Width * image.Height];
            int count = 0;
            for (int th = 0; th < image.Height; th++)
            {
                for (int tw = 0; tw < image.Width; tw++)
                {
                    Color color = bitmap.GetPixel(tw, th);
                    pixels[count] = color.R;
                    count++;
                    pixels[count] = color.G;
                    count++;
                    pixels[count] = color.B;
                    count++;
                }
            }
            return pixels;
        }

        private void SetPixels(int[] pixels)
        {
            int count = 0;
            for (int th = 0; th < image.Height; th++)
                for (int tw = 0; tw < image.Width; tw++)
                {
                    Color color = Color.FromArgb(pixels[count++]);
                    bitmap.SetPixel(tw, th, color);
                }
        }

        protected void SetPixels()
        {
            int[] dest = GetPixels(bitmap);
            // fill in starting image contents based on last image's dispose code
            if (lastDispose > 0)
            {
                if (lastDispose == 3)
                {
                    // use image before last
                    int n = frameCount - 2;
                    if (n > 0)
                        lastImage = GetFrame(n - 1);
                    else
                        lastImage = null;
                }

                if (lastImage != null)
                {
                    int[] prev = GetPixels(new Bitmap(lastImage));
                    Array.Copy(prev, 0, dest, 0, width * height);
                    if (lastDispose == 2)
                    {
                        // fill last image rect area with background color
                        Graphics g = Graphics.FromImage(image);
                        Color c = Color.Empty;
                        if (transparency)
                            c = Color.FromArgb(0, 0, 0, 0); 	// assume background is transparent
                        else
                            c = Color.FromArgb(lastBgColor);
                        Brush brush = new SolidBrush(c);
                        g.FillRectangle(brush, lastRect);
                        brush.Dispose();
                        g.Dispose();
                    }
                }
            }

            // copy each source line to the appropriate place in the destination
            int pass = 1;
            int inc = 8;
            int iline = 0;
            for (int i = 0; i < ih; i++)
            {
                int line = i;
                if (interlace)
                {
                    if (iline >= ih)
                    {
                        pass++;
                        switch (pass)
                        {
                            case 2:
                                iline = 4;
                                break;
                            case 3:
                                iline = 2;
                                inc = 4;
                                break;
                            case 4:
                                iline = 1;
                                inc = 2;
                                break;
                        }
                    }
                    line = iline;
                    iline += inc;
                }
                line += iy;
                if (line < height)
                {
                    int k = line * width;
                    int dx = k + ix; // start of line in dest
                    int dlim = dx + iw; // end of dest line
                    if ((k + width) < dlim)
                        dlim = k + width; // past dest edge
                    int sx = i * iw; // start of line in source
                    while (dx < dlim)
                    {
                        // map color and insert in destination
                        int index = ((int)pixels[sx++]) & 0xff;
                        int c = act[index];
                        if (c != 0)
                            dest[dx] = c;
                        dx++;
                    }
                }
            }
            SetPixels(dest);
        }

        /// <summary>
        /// 获取指定索引的帧图像
        /// </summary>
        /// <param name="index"></param>
        /// <returns>BufferedImage representation of frame, or null if index is invalid.</returns>
        public Image GetFrame(int index)
        {
            Image im = null;
            if (index >= 0 && index < frameCount)
                im = ((GifFrame)frames[index]).Image;
            return im;
        }

        /// <summary>
        /// 图像大小
        /// </summary>
        /// <returns></returns>
        public Size FrameSize
        {
            get { return new Size(width, height); }
        }

        /// <summary>
        /// 从缓冲流中读取Gif帧
        /// </summary>
        /// <param name="stream">缓冲流</param>
        /// <returns>读取状态码（0=没有错误）</returns>
        public int Read(Stream stream)
        {
            Init();
            if (stream != null)
            {
                this.inStream = stream;
                ReadHeader();
                if (!Error)
                {
                    ReadContents();
                    if (frameCount < 0)
                        status = STATUS_FORMAT_ERROR;
                }
                stream.Close();
            }
            else
                status = STATUS_OPEN_ERROR;
            return status;
        }

        /// <summary>
        /// 从图像对象中读取Gif帧
        /// </summary>
        /// <param name="image">图像对象</param>
        /// <returns>读取状态码（0=没有错误）</returns>
        public int Read(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, (ImageFormat)image.RawFormat);
            ms.Seek(0, SeekOrigin.Begin);
            return Read(ms);
        }

        /// <summary>
        /// 从指定文件/ URL源（网址假设，如果名称中包含“:/”或“文件：”）读取Gif帧
        /// </summary>
        /// <param name="fileName">源</param>
        /// <returns>读取状态码（0=没有错误）</returns>
        public int Read(string fileName)
        {
            status = STATUS_OK;
            try
            {
                fileName = fileName.Trim().ToLower();
                status = Read(new FileInfo(fileName).OpenRead());
            }
            catch (IOException)
            {
                status = STATUS_OPEN_ERROR;
            }
            return status;
        }

        /// <summary>
        /// LZW压缩的图像数据解码成像素阵列
        /// </summary>
        protected void DecodeImageData()
        {
            int NullCode = -1;
            int npix = iw * ih;
            int available, clear, code_mask, code_size, end_of_information, in_code, old_code, bits, code, count, i, datum, data_size, first, top, bi, pi;
            if (pixels == null || pixels.Length < npix)
                pixels = new byte[npix]; //分配新的像素阵列
            if (prefix == null)
                prefix = new short[MaxStackSize];
            if (suffix == null)
                suffix = new byte[MaxStackSize];
            if (pixelStack == null)
                pixelStack = new byte[MaxStackSize + 1];

            //初始化GIF数据流的解码器。
            data_size = Read();
            clear = 1 << data_size;
            end_of_information = clear + 1;
            available = clear + 2;
            old_code = NullCode;
            code_size = data_size + 1;
            code_mask = (1 << code_size) - 1;
            for (code = 0; code < clear; code++)
            {
                prefix[code] = 0;
                suffix[code] = (byte)code;
            }

            //解码GIF像素流
            datum = bits = count = first = top = pi = bi = 0;
            for (i = 0; i < npix; )
            {
                if (top == 0)
                {
                    if (bits < code_size)
                    {
                        //加载字节代码，直到有足够的位。
                        if (count == 0)
                        {
                            //读取新的数据块
                            count = ReadBlock();
                            if (count <= 0)
                                break;
                            bi = 0;
                        }
                        datum += (((int)block[bi]) & 0xff) << bits;
                        bits += 8;
                        bi++;
                        count--;
                        continue;
                    }

                    //  Get the next code.
                    code = datum & code_mask;
                    datum >>= code_size;
                    bits -= code_size;

                    //  Interpret the code
                    if (code > available || code == end_of_information)
                        break;
                    if (code == clear)
                    {
                        //  Reset decoder.
                        code_size = data_size + 1;
                        code_mask = (1 << code_size) - 1;
                        available = clear + 2;
                        old_code = NullCode;
                        continue;
                    }
                    if (old_code == NullCode)
                    {
                        pixelStack[top++] = suffix[code];
                        old_code = code;
                        first = code;
                        continue;
                    }
                    in_code = code;
                    if (code == available)
                    {
                        pixelStack[top++] = (byte)first;
                        code = old_code;
                    }
                    while (code > clear)
                    {
                        pixelStack[top++] = suffix[code];
                        code = prefix[code];
                    }
                    first = ((int)suffix[code]) & 0xff;

                    //添加一个新的字符串到字符串表,
                    if (available >= MaxStackSize)
                        break;
                    pixelStack[top++] = (byte)first;
                    prefix[available] = (short)old_code;
                    suffix[available] = (byte)first;
                    available++;
                    if ((available & code_mask) == 0&& available < MaxStackSize)
                    {
                        code_size++;
                        code_mask += available;
                    }
                    old_code = in_code;
                }

                //弹出一个关闭像素栈像素.
                top--;
                pixels[pi++] = pixelStack[top];
                i++;
            }
            for (i = pi; i < npix; i++)
                pixels[i] = 0; //清除的缺失像素
        }

        /// <summary>
        /// 如果读/解码期间遇到了错误，则返回true
        /// </summary>
        /// <returns></returns>
        protected bool Error
        {
            get { return status != STATUS_OK; }
        }

        /// <summary>
        /// 初始化或重新初始化读取器
        /// </summary>
        protected void Init()
        {
            status = STATUS_OK;
            frameCount = 0;
            frames = new ArrayList();
            gct = null;
            lct = null;
        }

        /// <summary>
        /// 从输入流中读取一个单字节
        /// </summary>
        /// <returns></returns>
        protected int Read()
        {
            try
            {
                return inStream.ReadByte();
            }
            catch (IOException)
            {
                status = STATUS_FORMAT_ERROR;
                return 0;
            }
        }

        /// <summary>
        /// 从输入中读取下一个可变长度块.
        /// </summary>
        /// <returns>“缓冲”中的字节数</returns>
        protected int ReadBlock()
        {
            blockSize = Read();
            int n = 0;
            if (blockSize > 0)
            {
                try
                {
                    int count = 0;
                    while (n < blockSize)
                    {
                        count = inStream.Read(block, n, blockSize - n);
                        if (count == -1)
                            break;
                        n += count;
                    }
                }
                catch (IOException)
                {
                }
                if (n < blockSize)
                    status = STATUS_FORMAT_ERROR;
            }
            return n;
        }

        /// <summary>
        /// 读取256 RGB整数值的颜色表
        /// </summary>
        /// <param name="ncolors">ncolors int number of colors to read</param>
        /// <returns>int array containing 256 colors (packed ARGB with full alpha)</returns>
        protected int[] ReadColorTable(int ncolors)
        {
            int nbytes = 3 * ncolors;
            int[] tab = null;
            byte[] c = new byte[nbytes];
            int n = 0;
            try
            {
                n = inStream.Read(c, 0, c.Length);
            }
            catch (IOException)
            {
            }
            if (n < nbytes)
                status = STATUS_FORMAT_ERROR;
            else
            {
                tab = new int[256]; // max size to avoid bounds checks
                int i = 0;
                int j = 0;
                while (i < ncolors)
                {
                    int r = ((int)c[j++]) & 0xff;
                    int g = ((int)c[j++]) & 0xff;
                    int b = ((int)c[j++]) & 0xff;
                    tab[i++] = (int)((uint)0xff000000 | ((uint)r << 16) | ((uint)g << 8) | (uint)b);
                }
            }
            return tab;
        }

        /// <summary>
        /// Main file parser.  Reads GIF content blocks.
        /// </summary>
        protected void ReadContents()
        {
            // read GIF file content blocks
            bool done = false;
            while (!(done || Error))
            {
                int code = Read();
                switch (code)
                {
                    case 0x2C: // image separator
                        ReadImage();
                        break;
                    case 0x21: // extension
                        code = Read();
                        switch (code)
                        {
                            case 0xf9: // graphics control extension
                                ReadGraphicControlExt();
                                break;
                            case 0xff: // application extension
                                ReadBlock();
                                string app = string.Empty;
                                for (int i = 0; i < 11; i++)
                                    app += (char)block[i];
                                if (app.Equals("NETSCAPE2.0"))
                                    ReadNetscapeExt();
                                else
                                    Skip(); // don't care
                                break;
                            default: // uninteresting extension
                                Skip();
                                break;
                        }
                        break;
                    case 0x3b: // terminator
                        done = true;
                        break;
                    case 0x00: // bad byte, but keep going and see what happens
                        break;
                    default:
                        status = STATUS_FORMAT_ERROR;
                        break;
                }
            }
        }

        /// <summary>
        /// Reads Graphics Control Extension values
        /// </summary>
        protected void ReadGraphicControlExt()
        {
            Read(); // block size
            int packed = Read(); // packed fields
            dispose = (packed & 0x1c) >> 2; // disposal method
            if (dispose == 0)
                dispose = 1; // elect to keep old image if discretionary
            transparency = (packed & 1) != 0;
            delay = ReadShort() * 10; // delay in milliseconds
            transIndex = Read(); // transparent color index
            Read(); // block terminator
        }

        /// <summary>
        /// Reads GIF file header information.
        /// </summary>
        protected void ReadHeader()
        {
            string id = string.Empty;
            for (int i = 0; i < 6; i++)
                id += (char)Read();
            if (!id.StartsWith("GIF"))
            {
                status = STATUS_FORMAT_ERROR;
                return;
            }
            ReadLSD();
            if (gctFlag && !Error)
            {
                gct = ReadColorTable(gctSize);
                bgColor = gct[bgIndex];
            }
        }

        /// <summary>
        /// Reads next frame image
        /// </summary>
        protected void ReadImage()
        {
            ix = ReadShort(); // (sub)image position & size
            iy = ReadShort();
            iw = ReadShort();
            ih = ReadShort();
            int packed = Read();
            lctFlag = (packed & 0x80) != 0; // 1 - local color table flag
            interlace = (packed & 0x40) != 0; // 2 - interlace flag
            // 3 - sort flag
            // 4-5 - reserved
            lctSize = 2 << (packed & 7); // 6-8 - local color table size
            if (lctFlag)
            {
                lct = ReadColorTable(lctSize); // read table
                act = lct; // make local table active
            }
            else
            {
                act = gct; // make global table active
                if (bgIndex == transIndex)
                    bgColor = 0;
            }
            int save = 0;
            if (transparency)
            {
                save = act[transIndex];
                act[transIndex] = 0; // set transparent color if specified
            }
            if (act == null)
                status = STATUS_FORMAT_ERROR; // no color table defined
            if (Error)
                return;
            DecodeImageData(); // decode pixel data
            Skip();
            if (Error) 
                return;
            frameCount++;
            // create new image to receive frame data
            //		image =
            //			new BufferedImage(width, height, BufferedImage.TYPE_INT_ARGB_PRE);
            bitmap = new Bitmap(width, height);
            image = bitmap;
            SetPixels(); // transfer pixel data to image
            frames.Add(new GifFrame(bitmap, delay)); // add image to frame list
            if (transparency)
                act[transIndex] = save;
            ResetFrame();
        }

        /// <summary>
        /// Reads Logical Screen Descriptor
        /// </summary>
        protected void ReadLSD()
        {
            // logical screen size
            width = ReadShort();
            height = ReadShort();
            // packed fields
            int packed = Read();
            gctFlag = (packed & 0x80) != 0; // 1   : global color table flag
            // 2-4 : color resolution
            // 5   : gct sort flag
            gctSize = 2 << (packed & 7); // 6-8 : gct size
            bgIndex = Read(); // background color index
            pixelAspect = Read(); // pixel aspect ratio
        }

        /// <summary>
        /// Reads Netscape extenstion to obtain iteration count
        /// </summary>
        protected void ReadNetscapeExt()
        {
            do
            {
                ReadBlock();
                if (block[0] == 1)
                {
                    // loop count sub-block
                    int b1 = ((int)block[1]) & 0xff;
                    int b2 = ((int)block[2]) & 0xff;
                    loopCount = (b2 << 8) | b1;
                }
            } while ((blockSize > 0) && !Error);
        }

        /// <summary>
        /// Reads next 16-bit value, LSB first
        /// </summary>
        /// <returns></returns>
        protected int ReadShort()
        {
            // read 16-bit value, LSB first
            return Read() | Read() << 8;
        }

        /// <summary>
        /// Resets frame state for reading next image.
        /// </summary>
        protected void ResetFrame()
        {
            lastDispose = dispose;
            lastRect = new Rectangle(ix, iy, iw, ih);
            lastImage = image;
            lastBgColor = bgColor;
            lct = null;
        }

        /// <summary>
        /// Skips variable length blocks up to and including next zero length block.
        /// </summary>
        protected void Skip()
        {
            do
            {
                ReadBlock();
            } while (blockSize > 0 && !Error);
        }
    }
}