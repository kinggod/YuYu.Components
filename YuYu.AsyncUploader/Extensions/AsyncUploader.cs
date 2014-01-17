using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace YuYu.Components
{
    /// <summary>
    /// 异步上传器
    /// </summary>
    public class AsyncUploader
    {
        /// <summary>
        /// 在接收异步上传数据的 Action 中调用此方法以完成数据的上传
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="targetDirectory">上传到的服务器目录，默认“~/Temps/”</param>
        /// <param name="maxFileSize">最大上传文件大小，默认 4194304 bytes</param>
        /// <param name="allowedFileTypes">允许的文件扩展名，默认“*.jpeg;*.jpg;*.png;*.gif;*.bmp”</param>
        public static void Upload(HttpRequestBase request, HttpResponseBase response, string targetDirectory = "~/Temps/", int maxFileSize = 4194304, string allowedFileTypes = "*.jpeg;*.jpg;*.png;*.gif;*.bmp")
        {
            response.Charset = "UTF-8";
            HttpPostedFileBase file = request.Files[0];
            if (file.ContentLength > maxFileSize)
                return;
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (allowedFileTypes.ToLower().IndexOf(fileExtension) < 0 && allowedFileTypes.IndexOf("*.*") < 0)
                return;
            string directory = HttpContext.Current.Server.MapPath(targetDirectory),
                filename = Guid.NewGuid().ToString("N") + fileExtension;
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            file.SaveAs(Path.Combine(directory, filename));
            response.Write(filename);
        }
    }
}
