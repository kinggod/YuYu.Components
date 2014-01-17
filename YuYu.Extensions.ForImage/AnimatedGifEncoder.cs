using System;
using System.Drawing;
using System.IO;

namespace YuYu.Components
{
    /// <summary>
    /// Gif����������
    /// </summary>
    internal class AnimatedGifEncoder
    {
        /// <summary>
        /// ֡��
        /// </summary>
        protected int _Width;

        /// <summary>
        /// ֡��
        /// </summary>
        protected int _Height;

        /// <summary>
        /// ָ����͸��ɫ
        /// </summary>
        protected Color _Transparent = Color.Empty;

        /// <summary>
        /// ָ��͸��ɫ�ڵ�ɫ���е�����
        /// </summary>
        protected int _TransIndex;

        /// <summary>
        /// ����ѭ��������Ĭ��ֵ-1��ѭ����
        /// </summary>
        protected int _Repeat = -1;

        /// <summary>
        /// ֡���ʱ��
        /// </summary>
        protected int _Delay = 0;

        /// <summary>
        /// ��ʼ���֡
        /// </summary>
        protected bool _Started = false;
        
        /// <summary>
        /// �ֽ���
        /// </summary>
        protected Stream _Steam;

        /// <summary>
        /// ��ǰͼ��
        /// </summary>
        protected Image _Image; // current frame

        /// <summary>
        /// BGR��֡���ֽ�����
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
        /// ����ÿ֮֡����ӳ�ʱ�䣬��ı������֡�����������һ֡����
        /// </summary>
        /// <param name="ms">�Ժ���Ϊ��λ���ӳ�ʱ��</param>
        public void SetDelay(int ms)
        {
            _Delay = (int)Math.Round(ms / 10.0f);
        }

        /// <summary>
        /// ����GIF��󲹳��ܺ��κκ���֡֡������롣Ĭ��Ϊ0�����û��͸������ɫ�Ѿ�����������2��
        /// </summary>
        /// <param name="code">���ô���</param>
        public void SetDispose(int code)
        {
            if (code >= 0)
                _Dispose = code;
        }

        /// <summary>
        /// Sets the number of times the set of GIF frames should be played.  Default is 1; 0 means play indefinitely.  Must be invoked before the first image is added.
        /// ����Ӧ���ŵ�GIF֡���Ĵ�����Ĭ��ֵ��1����0������ѭ����
        /// �����ڵ�һ֡���ǰ����
        /// </summary>
        /// <param name="iter">ѭ������</param>
        public void SetRepeat(int iter)
        {
            if (iter >= 0)
                _Repeat = iter;
        }

        /// <summary>
        /// ��󲹳��ܺ��κκ���֡����͸��ɫ��
        /// �������е���ɫ�������������е��޸ģ������ĵ�ɫ���������ɫ��ӽ���ÿһ֡����ɫ����Ϊ��֡��͸��ɫ��
        /// ������Ϊnull����ʾû��͸������ɫ��
        /// </summary>
        /// <param name="color">����Ϊ͸��ɫ������ʾ����ɫ</param>
        public void SetTransparent(Color color)
        {
            _Transparent = color;
        }

        /// <summary>
        /// ����δ����GIF֡��֡������д�룬��ʵ�������Ƴٵ���һ֡�յ�ʱ�����ݣ����Բ��롣
        /// ���� <code>finish()</code> ˢ������֡
        /// ��� <code>setSize</code> û�б�����, ��һ��ͼƬ�Ĵ�С�������к���֡��
        /// </summary>
        /// <param name="image">ͼ��</param>
        /// <returns>�ɹ����� true ���򷵻� false</returns>
        public bool AddFrame(Image image)
        {
            if ((image == null) || !_Started)
                return false;
            bool ok = true;
            try
            {
                if (!_SizeSet)
                    SetSize(image.Width, image.Height);// ʹ�õ�һ֡ͼ��ĳߴ�
                _Image = image;
                GetImagePixels(); // ת��Ϊ��ȷ�ĸ�ʽ������б�Ҫ
                AnalyzePixels(); // build color table & map pixels������ɫ����ͼ����
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
                    WritePalette(); // ���ص�ɫ��
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
        /// ���д�����л������ݵ��ļ����ر��ļ��������������д����������򲻹ر�����
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
            //���������ֶι���������ʹ��
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
        /// ����֡�٣��ȼ��� <code>setDelay(1000/fps)</code>��
        /// </summary>
        /// <param name="fps">֡��</param>
        public void SetFrameRate(float fps)
        {
            if (fps != 0f)
                _Delay = (int)Math.Round(100f / fps);
        }

        /// <summary>
        /// ������ɫ������������ͼ��ת��ΪGIF�淶�����������256��ɫ����
        /// ��ֵ�����Ϊ1�����������õ���ɫ���������Ĵ������š�
        /// 10��Ĭ�ϵģ����������õ�ɫ��ӳ���ں�����ٶȡ�������ֵ����20���ٶ����Ÿ��ơ�
        /// </summary>
        /// <param name="quality">�����������</param>
        public void SetQuality(int quality)
        {
            if (quality < 1)
                quality = 1;
            _Sample = quality;
        }
        
        /// <summary>
        /// ����GIF֡��С��
        /// Ĭ�ϴ�С�ǵ��ô˷���ǰ�ĵ�һ֡ͼ���С
        /// </summary>
        /// <param name="width">֡��</param>
        /// <param name="height">֡��</param>
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
        /// �ڸ�����������GIF�ļ��Ĵ������������Զ��رա�
        /// </summary>
        /// <param name="os">д��GIFͼ��������</param>
        /// <returns>�ɹ����� true ���򷵻� false</returns>
        public bool Start(Stream os)
        {
            if (os == null)
                return false;
            bool ok = true;
            _CloseStream = false;
            _Steam = os;
            try
            {
                WriteString("GIF89a"); //�ļ�ͷ
            }
            catch (IOException)
            {
                ok = false;
            }
            return _Started = ok;
        }

        /// <summary>
        /// ��������ָ�����Ƶ�һ��GIF�ļ��ı�д
        /// </summary>
        /// <param name="fileName">�ַ���������ļ�����ȫ�޶���</param>
        /// <returns>�ɹ����� true ���򷵻� false</returns>
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
        /// ����ͼ�����ɫ������ɫ��
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
        /// ���ص�ɫ���н�����ɫ������
        /// </summary>
        /// <param name="color">��ɫֵ</param>
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
        /// ��ȡ�����ֽ����顰���ء���ͼ�������
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
        /// д��ͼ�ο�����չ
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
        /// д��ͼ��������
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
        /// д���߼���Ļ����
        /// </summary>
        protected void WriteLSD()
        {
            //�߼���Ļ��С
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
        /// д��Netscape��Ӧ�ó�����չ������ѭ��������
        /// </summary>
        protected void WriteNetscapeExt()
        {
            _Steam.WriteByte(0x21); // extension introducer
            _Steam.WriteByte(0xff); // app extension label
            _Steam.WriteByte(11); // block size
            WriteString("NETSCAPE" + "2.0"); // app id + auth code
            _Steam.WriteByte(3); // sub-block size
            _Steam.WriteByte(1); // loop sub-block id
            WriteShort(_Repeat); // ѭ��������0������ѭ����
            _Steam.WriteByte(0); // block terminator
        }

        /// <summary>
        /// д����ɫ��
        /// </summary>
        protected void WritePalette()
        {
            _Steam.Write(_ColorTab, 0, _ColorTab.Length);
            int n = (3 * 256) - _ColorTab.Length;
            for (int i = 0; i < n; i++)
                _Steam.WriteByte(0);
        }

        /// <summary>
        /// ���벢д����������
        /// </summary>
        protected void WritePixels()
        {
            LZWEncoder encoder = new LZWEncoder(_Width, _Height, _IndexedPixels, _ColorDepth);
            encoder.Encode(_Steam);
        }

        /// <summary>
        /// д�뵽�������16λֵ��LSB��ǰ
        /// </summary>
        /// <param name="value"></param>
        protected void WriteShort(int value)
        {
            _Steam.WriteByte(Convert.ToByte(value & 0xff));
            _Steam.WriteByte(Convert.ToByte((value >> 8) & 0xff));
        }

        /// <summary>
        /// ���ַ���д�뵽�����
        /// </summary>
        /// <param name="str">�ַ���</param>
        protected void WriteString(string str)
        {
            char[] chars = str.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
                _Steam.WriteByte((byte)chars[i]);
        }
    }
}