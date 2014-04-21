using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace YuYu.Components
{
    /// <summary>
    /// 帮助类根
    /// </summary>
    public class HelperBase
    {
        private static IList<Timer> _Timers;

        private static Random _R;

        /// <summary>
        /// 使用枚举表示的当前区域和语言
        /// </summary>
        public static Culture CurrentCulture
        {
            get { return Thread.CurrentThread.CurrentCulture.GetCulture(); }
        }

        /// <summary>
        /// 已经被实例化的Random
        /// </summary>
        public static Random Random
        {
            get
            {
                if (_R == null)
                    _R = new Random();
                return _R;
            }
        }

        /// <summary>
        /// 输出随机汉字
        /// </summary>
        /// <param name="length">数量</param>
        /// <returns></returns>
        public static object[] RegionCode(int length)
        {
            return _RegionCode(length);
        }

        /// <summary>
        /// 通过Http Post将文件发送至指定的url
        /// </summary>
        /// <param name="url">目标url</param>
        /// <param name="name">用于检索的文件域名称</param>
        /// <param name="filename">文件全名称</param>
        /// <param name="mime">文件的的MIME</param>
        /// <returns>WebResponse</returns>
        public WebResponse PostFile(string url, string name, string filename, string mime)
        {
            string
                boundary = "----------" + DateTime.Now.Ticks.ToString("x"),
                header = "--" + boundary + "\r\nContent-Disposition: form-data; name=\"" + name + "\"; filename=\"" + Path.GetFileName(filename) + "\"" + "\r\nContent-Type: " + mime + "\r\n\r\n";
            byte[]
                headerBytes = Encoding.UTF8.GetBytes(header),
                boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n"),
                fileBytes = File.ReadAllBytes(filename);
            WebRequest request = WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.ContentLength = headerBytes.Length + fileBytes.Length + boundaryBytes.Length;
            request.Method = "POST";
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(headerBytes, 0, headerBytes.Length);
                stream.Write(fileBytes, 0, fileBytes.Length);
                stream.Write(boundaryBytes, 0, boundaryBytes.Length);
            }
            return request.GetResponse();
        }

        /// <summary>
        /// 清理文件夹。
        /// 删除文件夹及子文件夹中的所有文件
        /// </summary>
        /// <param name="directoryPath">指定要清理的文件夹全路径</param>
        /// <param name="timeSpan">清理间隔时间（毫秒）</param>
        /// <param name="searchPattern">要与 path 中的文件名匹配的搜索字符串。此参数不能以两个句点（“..”）结束，不能在 System.IO.Path.DirectorySeparatorChar或 System.IO.Path.AltDirectorySeparatorChar 的前面包含两个句点（“..”），也不能包含 System.IO.Path.InvalidPathChars中的任何字符</param>
        public static void ClearDirectory(string directoryPath, int timeSpan, string searchPattern)
        {
            if (_Timers == null)
                _Timers = new List<Timer>();
            Timer timer = new Timer(new TimerCallback(m =>
            {
                string[] pathAndPattern = m as string[];
                if (pathAndPattern != null && pathAndPattern.Length == 2)
                {
                    string path = pathAndPattern[0];
                    string pattern = pathAndPattern[1];
                    string[] filePaths = null;
                    try
                    {
                        filePaths = Directory.GetFiles(path, pattern, SearchOption.AllDirectories);
                    }
                    catch { }
                    if (filePaths != null && filePaths.Length > 0)
                        for (int i = 0; i < filePaths.Length; i++)
                            try
                            {
                                FileInfo file = new FileInfo(filePaths[i]);
                                if (file.CreationTime.AddMilliseconds(timeSpan) < DateTime.Now)
                                    file.Delete();
                            }
                            catch
                            {
                                continue;
                            }
                }
                else
                    return;
            }), new string[] { directoryPath, searchPattern }, 5000, timeSpan);
        }

        /// <summary>
        /// 从给定代码字符串创建对象
        /// </summary>
        /// <param name="codeString"></param>
        /// <param name="referencedAssemblies"></param>
        /// <returns></returns>
        public static object CreateObject(string codeString, params string[] referencedAssemblies)
        {
            string tempsPath = AppDomain.CurrentDomain.BaseDirectory + "Temps\\";
            if (!Directory.Exists(tempsPath))
                Directory.CreateDirectory(tempsPath);
            string codeFile = tempsPath + "__temp.cs";
            string assemblyFile = tempsPath + "__temp.dll";
            File.Delete(codeFile);
            File.Delete(assemblyFile);
            FileStream fs = File.Open(codeFile, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(codeString);
            sw.Close();
            sw.Dispose();
            fs.Close();
            fs.Dispose();
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            parameters.OutputAssembly = assemblyFile;
            if (referencedAssemblies != null && referencedAssemblies.Length > 0)
                parameters.ReferencedAssemblies.AddRange(referencedAssemblies);
            CompilerResults results = provider.CompileAssemblyFromFile(parameters, codeFile);
            if (results.Errors.HasErrors || results.Errors.HasWarnings)
                return null;
            else
                return results.CompiledAssembly.CreateInstance("__temp.__temp");
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
    }
}
