using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NetCoreObject.Common
{
    public static class SendHelper
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="Subject">主题</param>
        /// <param name="Body">正文</param>
        /// <param name="getMail">收件人地址，多个地址间用英文逗号或分号隔开</param>
        /// <returns></returns>
        public static bool Send(string Subject, string Body, List<EmailModel> eList) {

            var bmodel = BasicConfigHelper.getBasicConfig().Result;

            return SendEmailHelper.MailKit(Subject, Body, eList, bmodel.emailfrom, bmodel.emailnickname, bmodel.emailpassword, bmodel.emailsmtp, bmodel.emailport.ToString(), bmodel.emailssl, "", "", "");
        }
        
        public static bool CheckEmail(string email)
        {
            //邮箱正则        
            string dianxin = @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
            Regex dReg = new Regex(dianxin);
            if (dReg.IsMatch(email))
            {
                return true;
            }
            return false;
        }
    }
}
