using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class MailHelper
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toEmail">收件人地址</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="displayName">发件人显示名称</param>
        /// <param name="body">邮件正文</param>
        /// <param name="timeout">超时时间（毫秒），默认60000</param>
        /// <param name="async">true:以异步的方式发送邮件，不阻塞当前线程（默认）；false:阻塞当前线程直到邮件发送完成</param>
        /// <param name="sendCompleted">发送邮件完成后调用的事件，默认null</param>
        /// <returns></returns>
        public static bool SendMail(string toEmail, string subject, string displayName, string body, int timeout = 60000, bool async = true, SendCompletedEventHandler sendCompleted = null)
        {
            return _SendMail(toEmail, null, null, null, subject, displayName, body, null, timeout, async, sendCompleted);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toEmail">收件人地址</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="displayName">发件人显示名称</param>
        /// <param name="body">邮件正文</param>
        /// <param name="attachs">附件</param>
        /// <param name="timeout">超时时间（毫秒），默认60000</param>
        /// <param name="async">true:以异步的方式发送邮件，不阻塞当前线程（默认）；false:阻塞当前线程直到邮件发送完成</param>
        /// <param name="sendCompleted">发送邮件完成后调用的事件，默认null</param>
        /// <returns></returns>
        public static bool SendMail(string toEmail, string subject, string displayName, string body, Attachment[] attachs, int timeout = 60000, bool async = true, SendCompletedEventHandler sendCompleted = null)
        {
            return _SendMail(toEmail, null, null, null, subject, displayName, body, attachs, timeout, async, sendCompleted);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toEmail">收件人地址</param>
        /// <param name="toEmails">收件人地址，多地址以“,”分割</param>
        /// <param name="bcc">密送地址，多地址以“,”分割</param>
        /// <param name="cc">抄送地址，多地址以“,”分割</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="displayName">发件人显示名称</param>
        /// <param name="body">邮件正文</param>
        /// <param name="attachs">附件</param>
        /// <param name="timeout">超时时间（毫秒），默认60000</param>
        /// <param name="async">true:以异步的方式发送邮件，不阻塞当前线程（默认）；false:阻塞当前线程直到邮件发送完成</param>
        /// <param name="sendCompleted">发送邮件完成后调用的事件，默认null</param>
        /// <returns></returns>
        public static bool SendMail(string toEmail, string toEmails, string bcc, string cc, string subject, string displayName, string body, Attachment[] attachs, int timeout = 60000, bool async = true, SendCompletedEventHandler sendCompleted = null)
        {
            return _SendMail(toEmail, toEmails, bcc, cc, subject, displayName, body, attachs, timeout, async, sendCompleted);
        }

        #region Private

        private static bool _SendMail(string toEmail, string toEmails, string bcc, string cc, string subject, string displayName, string body, Attachment[] attachs, int timeout, bool async, SendCompletedEventHandler sendCompleted)
        {
            try
            {
                SmtpSection smtp = NetSectionGroup.GetSectionGroup(WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath)).MailSettings.Smtp;
                MailAddress from = new MailAddress(smtp.From, displayName, Encoding.UTF8);
                MailAddress to = new MailAddress(toEmail, null, Encoding.UTF8);
                MailMessage mail = new MailMessage(from, to);
                if (!string.IsNullOrWhiteSpace(toEmails))
                    mail.To.Add(toEmails);
                if (!string.IsNullOrWhiteSpace(bcc))
                    mail.Bcc.Add(bcc);
                if (!string.IsNullOrWhiteSpace(cc))
                    mail.CC.Add(cc);
                mail.Subject = subject;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.Body = body;
                mail.BodyEncoding = Encoding.UTF8;
                if (attachs != null && attachs.Length > 0)
                    foreach (Attachment item in attachs)
                        mail.Attachments.Add(item);
                mail.IsBodyHtml = true;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                SmtpClient smtpClient = new SmtpClient(smtp.Network.Host, smtp.Network.Port) { Timeout = timeout };
                if (sendCompleted != null)
                    smtpClient.SendCompleted += sendCompleted;
                if (async)
                    smtpClient.SendAsync(mail, mail);
                else
                {
                    smtpClient.Send(mail);
                    mail.Dispose();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

    }
}
