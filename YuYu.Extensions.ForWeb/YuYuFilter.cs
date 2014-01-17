using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace YuYu.Components
{
    /// <summary>
    /// YuYu过滤器
    /// </summary>
    public class YuYuFilter : Stream
    {
        private readonly Stream _ResponseStream;
        private readonly string _FilePathExtension;

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="responseStream">输出流</param>
        /// <param name="filePathExtension">请求的文件扩展名</param>
        public YuYuFilter(Stream responseStream, string filePathExtension)
        {
            _ResponseStream = responseStream;
            _FilePathExtension = filePathExtension;
        }

        /// <summary>
        /// 重写父类
        /// </summary>
        public override bool CanRead
        {
            get { return false; }
        }

        /// <summary>
        /// 重写父类
        /// </summary>
        public override bool CanSeek
        {
            get { return false; }
        }

        /// <summary>
        /// 重写父类
        /// </summary>
        public override bool CanWrite
        {
            get { return true; }
        }

        /// <summary>
        /// 重写父类
        /// </summary>
        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// 重写父类
        /// </summary>
        public override long Position { get; set; }

        /// <summary>
        /// 重写父类
        /// </summary>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 重写父类
        /// </summary>
        public override void SetLength(long length)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 重写父类
        /// </summary>
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 重写父类
        /// </summary>
        public override void Flush()
        {
            _ResponseStream.Flush();
        }

        /// <summary>
        /// 将缓存数据写到输出流（重写父类）
        /// </summary>
        public override void Write(byte[] buffer, int offset, int count)
        {
            string html = System.Text.Encoding.UTF8.GetString(buffer);
            html = RegexHelper.Tabs.Replace(html, string.Empty);
            html = RegexHelper.CarriageReturns.Replace(html, " ");
            html = RegexHelper.SpacesBetweenTags.Replace(html, "> <");
            html = RegexHelper.Spaces.Replace(html, " ");
            html = RegexHelper.SpacesInTagsa.Replace(html, "</");
            html = RegexHelper.SpacesInTagsb.Replace(html, "/>");
            html = RegexHelper.SpacesInTagsc.Replace(html, "<");
            html = RegexHelper.SpacesInTagsd.Replace(html, ">");
            html = html.Replace("//<![CDATA[", string.Empty);
            html = html.Replace("//]]>", string.Empty);
            byte[] response = System.Text.Encoding.UTF8.GetBytes(html);
            _ResponseStream.Write(response, 0, response.GetLength(0));
        }

        /// <summary>
        /// 重写父类
        /// </summary>
        public override void Close()
        {
            _ResponseStream.Close();
        }

        /// <summary>
        /// 重写父类
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _ResponseStream.Dispose();
        }
    }
}
