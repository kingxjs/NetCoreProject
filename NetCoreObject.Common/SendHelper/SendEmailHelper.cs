//using CDO;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;


namespace NetCoreObject.Common
{
    public class EmailModel
    {
        /// <summary>
        /// 收件人名称
        /// </summary>
        public string ToName { get; set; }
        /// <summary>
        /// 收件人地址
        /// </summary>
        public string ToMail { get; set; }
    }
    public static class SendEmailHelper
    {
        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="Subject">主题</param>
        /// <param name="Body">正文</param>
        /// <param name="getMail">收件人地址</param>
        /// <param name="fromMail">发件人地址</param>
        /// <param name="fromName">发件人名称</param>
        /// <param name="fromPwd">发件人密码</param>
        /// <param name="Host">发件服务器地址</param>
        /// <param name="Port">发件服务器端口</param>
        /// <param name="sslEnable">是否加密</param>
        /// <param name="csMail">抄送人地址，多个地址间用英文逗号或分号隔开</param>
        /// <param name="msMail">密送人地址，多个地址间用英文逗号或分号隔开</param>
        /// <param name="fjMail">附件服务器目录，多个服务器端目录之间以英文逗号或分号隔开</param>
        public static bool MailKit(string Subject, string Body, List<EmailModel> eList, string fromMail, string fromName, string fromPwd, string Host, string Port, bool sslEnable, string csMail, string msMail, string fjMail) {

            var message = new MimeMessage();
            try
            {
                if (string.IsNullOrEmpty(Port)) {
                    Port = "0";
                }
                foreach (var item in eList)
                {
                    message.To.Add(new MailboxAddress(item.ToName,item.ToMail));
                }
                message.From.Add(new MailboxAddress(fromName, fromMail));
                message.Subject = Subject;
                var html = new TextPart("html")
                {
                    Text = Body
                };
                var alternative = new Multipart("alternative");
                alternative.Add(html);
                var multipart = new Multipart("mixed");
                multipart.Add(alternative);
                message.Body = multipart;

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(Host, Int32.Parse(Port), sslEnable);

                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(fromMail, fromPwd);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }
    }
}