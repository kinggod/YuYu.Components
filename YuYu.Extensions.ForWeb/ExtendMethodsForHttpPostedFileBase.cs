using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Drawing;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtendMethodsForHttpPostedFileBase
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">表单提交的文件</param>
        /// <param name="targetDirectory">目标文件夹的相对路径，如“～/Temps/”</param>
        /// <param name="filename">返回上传成功后的文件名</param>
        /// <param name="maxFileSize">可上传最大字节</param>
        /// <param name="allowedFileTypes">被允许的文件格式</param>
        /// <returns></returns>
        public static UploadResult Upload(this HttpPostedFileBase file, string targetDirectory, out string filename, int maxFileSize = 4194304, string allowedFileTypes = "*.jpeg;*.jpg;*.png;*.gif;*.bmp")
        {
            filename = null;
            try
            {
                if (file.ContentLength > maxFileSize)
                    return UploadResult.TooLarge;//上传文件过大
                string fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (allowedFileTypes.ToLower().IndexOf(fileExtension) < 0 && allowedFileTypes.IndexOf("*.*") < 0)
                    return UploadResult.Denied;//文件格式被拒绝
                string directory = HttpContext.Current.Server.MapPath(targetDirectory);
                filename = Guid.NewGuid().ToString("N") + fileExtension;
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                file.SaveAs(Path.Combine(directory, filename));
                return UploadResult.Success;//上传成功
            }
            catch (Exception)
            {
                return UploadResult.Error;//上传中有错误
            }
        }

        /// <summary>
        /// 获取上传文件并转换为Image对象
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Image GetImage(this HttpPostedFileBase file)
        {
            return Image.FromStream(file.InputStream);
        }
    }
}
