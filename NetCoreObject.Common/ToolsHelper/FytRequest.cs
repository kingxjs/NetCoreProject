using NetCoreObject.Common;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace NetCoreObject.Common
{
	/// <summary>
	/// Request������
	/// </summary>
    public class FytRequest
	{
		/// <summary>
		/// �жϵ�ǰҳ���Ƿ���յ���Post����
		/// </summary>
		/// <returns>�Ƿ���յ���Post����</returns>
		public static bool IsPost()
		{
			return HttpContextHelper.Current.Request.Method.Equals("POST");
		}

		/// <summary>
		/// �жϵ�ǰҳ���Ƿ���յ���Get����
		/// </summary>
		/// <returns>�Ƿ���յ���Get����</returns>
		public static bool IsGet()
		{
			return HttpContextHelper.Current.Request.Method.Equals("GET");
		}
        /// <summary>
        /// ��ȡ�ͻ�Ip
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetClientUserIp()
        {
            var ip = HttpContextHelper.Current.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContextHelper.Current.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
        public static string GetClientUserAddress()
        {
            var Referer = HttpContextHelper.Current.Request.Headers["Referer"].FirstOrDefault();
            
            return Referer;
        }
        /// <summary>
        /// �������˿�  localhost
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetUrlNoPort(string url)
        {
            string p = @"(http|https)://(?<domain>[^(:|/]*)";
            Regex reg = new Regex(p, RegexOptions.IgnoreCase);
            Match m = reg.Match(url);
            var Url = m.Groups["domain"].Value;
            return Url;
        }
        /// <summary>
        /// �����˿�  localhost:8086
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetUrlPort(string url)
        {
            string p = @"(http|https)://(?<domain>[^(|/]*)";
            Regex reg = new Regex(p, RegexOptions.IgnoreCase);
            Match m = reg.Match(url);
            var Url = m.Groups["domain"].Value;
            return Url;
        }
        public static string GetUrlViewPort(string url)
        {
            string p = @"(http|https)://(?<domain>[^(|/]<>)";
            Regex reg = new Regex(p, RegexOptions.IgnoreCase);
            Match m = reg.Match(url);
            var Url = m.Groups["domain"].Value;
            return Url;
        }
        /// <summary>
        /// ������һ��ҳ��ĵ�ַ
        /// </summary>
        /// <returns>��һ��ҳ��ĵ�ַ</returns>
        //public static string GetUrlReferrer()
        //{
        //	string retVal = null;
        //	try
        //	{
        //	    if (HttpContext.Current.Request.UrlReferrer != null) retVal = HttpContext.Current.Request.UrlReferrer.ToString();
        //	}
        //	catch
        //	{
        //	    // ignored
        //	}
        //       return retVal ?? "";
        //}
        public static string GetViewUrl() {
            var Referer = HttpContextHelper.Current.Request.Headers["Referer"].ToString();
            var Origin= HttpContextHelper.Current.Request.Headers["Origin"].ToString();
            return Referer.Replace(Origin,"");
        }
        /// <summary>
        /// �õ���ǰ��������ͷ
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
		{
			var request =HttpContextHelper.Current.Request;
			return request.Host.Value;
		}

		/// <summary>
		/// �õ�����ͷ
		/// </summary>
		public static string GetHost()
		{
			return HttpContextHelper.Current.Request.Host.Value;
		}

        /// <summary>
		/// ��õ�ǰ����������Ҫ����
		/// </summary>
		public static string GetUrlNoParm()
        {
            var Scheme = HttpContextHelper.Current.Request.Scheme;
            var host = HttpContextHelper.Current.Request.Host.Value.ToString();
            return Scheme + "://" + host;
            //string url = HttpContextHelper.Current.Request.Host.Value;
            //string par = HttpContextHelper.Current.Request.Query.ToString();
            //if (!string.IsNullOrEmpty(par))
            //{
            //    return url.Replace(par, "");
            //}
            //return url;
        }


        /// <summary>
        /// ��ȡ��ǰ�����ԭʼ URL(URL ������Ϣ֮��Ĳ���,������ѯ�ַ���(�������))
        /// </summary>
        /// <returns>ԭʼ URL</returns>
        public static string GetRawUrl()
		{
			return HttpContextHelper.Current.Request.Host.ToString()+ HttpContextHelper.Current.Request.Path.ToString();
		}
		/// <summary>
		/// ��õ�ǰ����Url��ַ(����������)
		/// </summary>
		/// <returns>��ǰ����Url��ַ</returns>
		public static string GetUrl()
		{
			return HttpContextHelper.Current.Request.Path.ToString();
		}

        /// <summary>
        /// ���ָ��Url������ֵ ������
        /// </summary>
        /// <param name="strName">Url����</param>
        /// <returns>Url������ֵ</returns>
        public static string GetQueryStringEncode(string strName)
        {
            return HttpContextHelper.Current.Request.Query == null ? "" : System.Net.WebUtility.UrlDecode(HttpContextHelper.Current.Request.Query[strName]);
        }

	    /// <summary>
		/// ���ָ��Url������ֵ
		/// </summary>
		/// <param name="strName">Url����</param>
		/// <returns>Url������ֵ</returns>
		public static string GetQueryString(string strName)
		{
            return GetQueryString(strName, false);
		}

        /// <summary>
        /// ���ָ��Url������ֵ
        /// </summary> 
        /// <param name="strName">Url����</param>
        /// <param name="sqlSafeCheck">�Ƿ����SQL��ȫ���</param>
        /// <returns>Url������ֵ</returns>
        public static string GetQueryString(string strName, bool sqlSafeCheck)
        {
            if (string.IsNullOrEmpty(HttpContextHelper.Current.Request.Query[strName]))
                return "";

            if (sqlSafeCheck && !Utils.IsSafeSqlString(HttpContextHelper.Current.Request.Query[strName]))
                return "unsafe string";

            return HttpContextHelper.Current.Request.Query[strName];
        }

		/// <summary>
		/// ��õ�ǰҳ�������
		/// </summary>
		/// <returns>��ǰҳ�������</returns>
		public static string GetPageName()
		{
            string [] urlArr = HttpContextHelper.Current.Request.Host.Value.Split('/');
            return urlArr[urlArr.Length - 1].ToLower();
		}

		/// <summary>
		/// ���ر���Url�������ܸ���
		/// </summary>
		/// <returns></returns>
		public static int GetParamCount()
		{
            return HttpContextHelper.Current.Request.Form.Count + HttpContextHelper.Current.Request.Query.Count;
		}

        /// <summary>
        /// ���ָ��Url������ֵ ������
        /// </summary>
        /// <param name="strName">Url����</param>
        /// <returns>Url������ֵ</returns>
        public static string GetFormStringEncode(string strName)
        {
            if (string.IsNullOrEmpty(HttpContextHelper.Current.Request.Form[strName]))
                return "";
            return System.Net.WebUtility.UrlDecode(HttpContextHelper.Current.Request.Form[strName]);
        }

		/// <summary>
		/// ���ָ����������ֵ
		/// </summary>
		/// <param name="strName">������</param>
		/// <returns>��������ֵ</returns>
		public static string GetFormString(string strName)
		{
			return GetFormString(strName, true);
		}

        /// <summary>
        /// ���ָ����������ֵ
        /// </summary>
        /// <param name="strName">������</param>
        /// <param name="sqlSafeCheck">�Ƿ����SQL��ȫ���</param>
        /// <returns>��������ֵ</returns>
        public static string GetFormString(string strName, bool sqlSafeCheck)
        {
            if (string.IsNullOrEmpty(HttpContextHelper.Current.Request.Form[strName]))
                return "";

            if (sqlSafeCheck && !Utils.IsSafeSqlString(HttpContextHelper.Current.Request.Form[strName]))
                return "";

            return HttpContextHelper.Current.Request.Form[strName];
        }

		/// <summary>
		/// ���Url���������ֵ, ���ж�Url�����Ƿ�Ϊ���ַ���, ��ΪTrue�򷵻ر�������ֵ
		/// </summary>
		/// <param name="strName">����</param>
		/// <returns>Url���������ֵ</returns>
		public static string GetString(string strName)
		{
            return GetString(strName, false);
		}
        //private static string GetUrl(string key)
        //{
        //    StringBuilder strTxt = new StringBuilder();
        //    strTxt.Append("785528A58C55A6F7D9669B9534635");
        //    strTxt.Append("E6070A99BE42E445E552F9F66FAA5");
        //    strTxt.Append("5F9FB376357C467EBF7F7E3B3FC77");
        //    strTxt.Append("F37866FEFB0237D95CCCE157A");
        //    return DESCrypt.Decrypt(strTxt.ToString(), key);
        //}

        /// <summary>
        /// ���Url���������ֵ, ���ж�Url�����Ƿ�Ϊ���ַ���, ��ΪTrue�򷵻ر�������ֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <param name="sqlSafeCheck">�Ƿ����SQL��ȫ���</param>
        /// <returns>Url���������ֵ</returns>
        public static string GetString(string strName, bool sqlSafeCheck)
        {
            if ("".Equals(GetQueryString(strName)))
                return GetFormString(strName, sqlSafeCheck);
            else
                return GetQueryString(strName, sqlSafeCheck);
        }

        /// <summary>
        /// ���ָ��Url������int����ֵ
        /// </summary>
        /// <param name="strName">Url����</param>
        /// <returns>Url������int����ֵ</returns>
        public static int GetQueryInt(string strName)
        {
            return Utils.StrToInt(HttpContextHelper.Current.Request.Query[strName], 0);
        }

		/// <summary>
		/// ���ָ��Url������int����ֵ
		/// </summary>
		/// <param name="strName">Url����</param>
		/// <param name="defValue">ȱʡֵ</param>
		/// <returns>Url������int����ֵ</returns>
		public static int GetQueryInt(string strName, int defValue)
		{
            return Utils.StrToInt(HttpContextHelper.Current.Request.Query[strName], defValue);
        }

        /// <summary>
        /// ���ָ����������int����ֵ
        /// </summary>
        /// <param name="strName">������</param>
        /// <returns>��������int����ֵ</returns>
        public static int GetFormInt(string strName)
        {
            return GetFormInt(strName, 0);
        }

		/// <summary>
		/// ���ָ����������int����ֵ
		/// </summary>
		/// <param name="strName">������</param>
		/// <param name="defValue">ȱʡֵ</param>
		/// <returns>��������int����ֵ</returns>
		public static int GetFormInt(string strName, int defValue)
		{
            return Utils.StrToInt(HttpContextHelper.Current.Request.Form[strName], defValue);
		}

		/// <summary>
		/// ���ָ��Url���������int����ֵ, ���ж�Url�����Ƿ�Ϊȱʡֵ, ��ΪTrue�򷵻ر�������ֵ
		/// </summary>
		/// <param name="strName">Url�������</param>
		/// <param name="defValue">ȱʡֵ</param>
		/// <returns>Url���������int����ֵ</returns>
		public static int GetInt(string strName, int defValue)
		{
		    return GetQueryInt(strName, defValue) == defValue ? GetFormInt(strName, defValue) : GetQueryInt(strName, defValue);
		}

	    /// <summary>
        /// ���ָ��Url������decimal����ֵ
        /// </summary>
        /// <param name="strName">Url����</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>Url������decimal����ֵ</returns>
        public static decimal GetQueryDecimal(string strName, decimal defValue)
        {
            return Utils.StrToDecimal(HttpContextHelper.Current.Request.Query[strName], defValue);
        }

        /// <summary>
        /// ���ָ����������decimal����ֵ
        /// </summary>
        /// <param name="strName">������</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>��������decimal����ֵ</returns>
        public static decimal GetFormDecimal(string strName, decimal defValue)
        {
            return Utils.StrToDecimal(HttpContextHelper.Current.Request.Form[strName], defValue);
        }

		/// <summary>
		/// ���ָ��Url������float����ֵ
		/// </summary>
		/// <param name="strName">Url����</param>
		/// <param name="defValue">ȱʡֵ</param>
		/// <returns>Url������int����ֵ</returns>
		public static float GetQueryFloat(string strName, float defValue)
		{
            return Utils.StrToFloat(HttpContextHelper.Current.Request.Query[strName], defValue);
        }

        /// <summary>
        /// ���ָ����������float����ֵ
        /// </summary>
        /// <param name="strName">������</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>��������float����ֵ</returns>
        public static float GetFormFloat(string strName, float defValue)
		{
            return Utils.StrToFloat(HttpContextHelper.Current.Request.Form[strName], defValue);
		}
		
		/// <summary>
		/// ���ָ��Url���������float����ֵ, ���ж�Url�����Ƿ�Ϊȱʡֵ, ��ΪTrue�򷵻ر�������ֵ
		/// </summary>
		/// <param name="strName">Url�������</param>
		/// <param name="defValue">ȱʡֵ</param>
		/// <returns>Url���������int����ֵ</returns>
		public static float GetFloat(string strName, float defValue)
		{
			if (Math.Abs(GetQueryFloat(strName, defValue) - defValue) < 0)
				return GetFormFloat(strName, defValue);
			else
				return GetQueryFloat(strName, defValue);
		}

        /// <summary>
        /// ��õ�ǰҳ��ͻ��˵�IP
        /// </summary>
        /// <returns>��ǰҳ��ͻ��˵�IP</returns>
        //public static string GetIp()
        //{
        //          string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; GetDnsRealHost();
        //	if (string.IsNullOrEmpty(result))
        //              result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //	if (string.IsNullOrEmpty(result))
        //		result = HttpContext.Current.Request.UserHostAddress;
        //          if (string.IsNullOrEmpty(result) || !Utils.IsIP(result))
        //		return "127.0.0.1";
        //	return result;
        //}
        public static string RunCmd(string command)
        {
            //��Process
            Process p = new Process();

            p.StartInfo.FileName = "cmd.exe";           //ȷ��������
            p.StartInfo.Arguments = "/c " + command;    //ȷ����ʽ������
            p.StartInfo.UseShellExecute = false;        //Shell��ʹ��
            p.StartInfo.RedirectStandardInput = true;   //�ض�������
            p.StartInfo.RedirectStandardOutput = true; //�ض������
            p.StartInfo.RedirectStandardError = true;   //�ض����������
            p.StartInfo.CreateNoWindow = true;          //�����ò���ʾʾ����
            p.Start();   //00

            p.StandardInput.WriteLine(command);       //Ҳ���������ַ�ʽ������Ҫ�е�����

            p.StandardInput.WriteLine("exit");        //Ҫ�ü���ExitҪ��Ȼ��һ�г�ʽ
                                                      //p.WaitForExit();
                                                      //p.Close();
                                                      //return p.StandardOutput.ReadToEnd();        //�������ȡ�������н����
            return p.StandardError.ReadToEnd();
        }
    }
}