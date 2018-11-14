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
	/// Request操作类
	/// </summary>
    public class FytRequest
	{
		/// <summary>
		/// 判断当前页面是否接收到了Post请求
		/// </summary>
		/// <returns>是否接收到了Post请求</returns>
		public static bool IsPost()
		{
			return HttpContextHelper.Current.Request.Method.Equals("POST");
		}

		/// <summary>
		/// 判断当前页面是否接收到了Get请求
		/// </summary>
		/// <returns>是否接收到了Get请求</returns>
		public static bool IsGet()
		{
			return HttpContextHelper.Current.Request.Method.Equals("GET");
		}
        /// <summary>
        /// 获取客户Ip
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
        /// 不包含端口  localhost
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
        /// 包含端口  localhost:8086
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
        /// 返回上一个页面的地址
        /// </summary>
        /// <returns>上一个页面的地址</returns>
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
        /// 得到当前完整主机头
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
		{
			var request =HttpContextHelper.Current.Request;
			return request.Host.Value;
		}

		/// <summary>
		/// 得到主机头
		/// </summary>
		public static string GetHost()
		{
			return HttpContextHelper.Current.Request.Host.Value;
		}

        /// <summary>
		/// 获得当前域名，不需要参数
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
        /// 获取当前请求的原始 URL(URL 中域信息之后的部分,包括查询字符串(如果存在))
        /// </summary>
        /// <returns>原始 URL</returns>
        public static string GetRawUrl()
		{
			return HttpContextHelper.Current.Request.Host.ToString()+ HttpContextHelper.Current.Request.Path.ToString();
		}
		/// <summary>
		/// 获得当前完整Url地址(不包含域名)
		/// </summary>
		/// <returns>当前完整Url地址</returns>
		public static string GetUrl()
		{
			return HttpContextHelper.Current.Request.Path.ToString();
		}

        /// <summary>
        /// 获得指定Url参数的值 并解码
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryStringEncode(string strName)
        {
            return HttpContextHelper.Current.Request.Query == null ? "" : System.Net.WebUtility.UrlDecode(HttpContextHelper.Current.Request.Query[strName]);
        }

	    /// <summary>
		/// 获得指定Url参数的值
		/// </summary>
		/// <param name="strName">Url参数</param>
		/// <returns>Url参数的值</returns>
		public static string GetQueryString(string strName)
		{
            return GetQueryString(strName, false);
		}

        /// <summary>
        /// 获得指定Url参数的值
        /// </summary> 
        /// <param name="strName">Url参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName, bool sqlSafeCheck)
        {
            if (string.IsNullOrEmpty(HttpContextHelper.Current.Request.Query[strName]))
                return "";

            if (sqlSafeCheck && !Utils.IsSafeSqlString(HttpContextHelper.Current.Request.Query[strName]))
                return "unsafe string";

            return HttpContextHelper.Current.Request.Query[strName];
        }

		/// <summary>
		/// 获得当前页面的名称
		/// </summary>
		/// <returns>当前页面的名称</returns>
		public static string GetPageName()
		{
            string [] urlArr = HttpContextHelper.Current.Request.Host.Value.Split('/');
            return urlArr[urlArr.Length - 1].ToLower();
		}

		/// <summary>
		/// 返回表单或Url参数的总个数
		/// </summary>
		/// <returns></returns>
		public static int GetParamCount()
		{
            return HttpContextHelper.Current.Request.Form.Count + HttpContextHelper.Current.Request.Query.Count;
		}

        /// <summary>
        /// 获得指定Url参数的值 并解码
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetFormStringEncode(string strName)
        {
            if (string.IsNullOrEmpty(HttpContextHelper.Current.Request.Form[strName]))
                return "";
            return System.Net.WebUtility.UrlDecode(HttpContextHelper.Current.Request.Form[strName]);
        }

		/// <summary>
		/// 获得指定表单参数的值
		/// </summary>
		/// <param name="strName">表单参数</param>
		/// <returns>表单参数的值</returns>
		public static string GetFormString(string strName)
		{
			return GetFormString(strName, true);
		}

        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName, bool sqlSafeCheck)
        {
            if (string.IsNullOrEmpty(HttpContextHelper.Current.Request.Form[strName]))
                return "";

            if (sqlSafeCheck && !Utils.IsSafeSqlString(HttpContextHelper.Current.Request.Form[strName]))
                return "";

            return HttpContextHelper.Current.Request.Form[strName];
        }

		/// <summary>
		/// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
		/// </summary>
		/// <param name="strName">参数</param>
		/// <returns>Url或表单参数的值</returns>
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
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string strName, bool sqlSafeCheck)
        {
            if ("".Equals(GetQueryString(strName)))
                return GetFormString(strName, sqlSafeCheck);
            else
                return GetQueryString(strName, sqlSafeCheck);
        }

        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的int类型值</returns>
        public static int GetQueryInt(string strName)
        {
            return Utils.StrToInt(HttpContextHelper.Current.Request.Query[strName], 0);
        }

		/// <summary>
		/// 获得指定Url参数的int类型值
		/// </summary>
		/// <param name="strName">Url参数</param>
		/// <param name="defValue">缺省值</param>
		/// <returns>Url参数的int类型值</returns>
		public static int GetQueryInt(string strName, int defValue)
		{
            return Utils.StrToInt(HttpContextHelper.Current.Request.Query[strName], defValue);
        }

        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的int类型值</returns>
        public static int GetFormInt(string strName)
        {
            return GetFormInt(strName, 0);
        }

		/// <summary>
		/// 获得指定表单参数的int类型值
		/// </summary>
		/// <param name="strName">表单参数</param>
		/// <param name="defValue">缺省值</param>
		/// <returns>表单参数的int类型值</returns>
		public static int GetFormInt(string strName, int defValue)
		{
            return Utils.StrToInt(HttpContextHelper.Current.Request.Form[strName], defValue);
		}

		/// <summary>
		/// 获得指定Url或表单参数的int类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
		/// </summary>
		/// <param name="strName">Url或表单参数</param>
		/// <param name="defValue">缺省值</param>
		/// <returns>Url或表单参数的int类型值</returns>
		public static int GetInt(string strName, int defValue)
		{
		    return GetQueryInt(strName, defValue) == defValue ? GetFormInt(strName, defValue) : GetQueryInt(strName, defValue);
		}

	    /// <summary>
        /// 获得指定Url参数的decimal类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的decimal类型值</returns>
        public static decimal GetQueryDecimal(string strName, decimal defValue)
        {
            return Utils.StrToDecimal(HttpContextHelper.Current.Request.Query[strName], defValue);
        }

        /// <summary>
        /// 获得指定表单参数的decimal类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的decimal类型值</returns>
        public static decimal GetFormDecimal(string strName, decimal defValue)
        {
            return Utils.StrToDecimal(HttpContextHelper.Current.Request.Form[strName], defValue);
        }

		/// <summary>
		/// 获得指定Url参数的float类型值
		/// </summary>
		/// <param name="strName">Url参数</param>
		/// <param name="defValue">缺省值</param>
		/// <returns>Url参数的int类型值</returns>
		public static float GetQueryFloat(string strName, float defValue)
		{
            return Utils.StrToFloat(HttpContextHelper.Current.Request.Query[strName], defValue);
        }

        /// <summary>
        /// 获得指定表单参数的float类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的float类型值</returns>
        public static float GetFormFloat(string strName, float defValue)
		{
            return Utils.StrToFloat(HttpContextHelper.Current.Request.Form[strName], defValue);
		}
		
		/// <summary>
		/// 获得指定Url或表单参数的float类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
		/// </summary>
		/// <param name="strName">Url或表单参数</param>
		/// <param name="defValue">缺省值</param>
		/// <returns>Url或表单参数的int类型值</returns>
		public static float GetFloat(string strName, float defValue)
		{
			if (Math.Abs(GetQueryFloat(strName, defValue) - defValue) < 0)
				return GetFormFloat(strName, defValue);
			else
				return GetQueryFloat(strName, defValue);
		}

        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
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
            //例Process
            Process p = new Process();

            p.StartInfo.FileName = "cmd.exe";           //确定程序名
            p.StartInfo.Arguments = "/c " + command;    //确定程式命令行
            p.StartInfo.UseShellExecute = false;        //Shell的使用
            p.StartInfo.RedirectStandardInput = true;   //重定向输入
            p.StartInfo.RedirectStandardOutput = true; //重定向输出
            p.StartInfo.RedirectStandardError = true;   //重定向输出错误
            p.StartInfo.CreateNoWindow = true;          //设置置不显示示窗口
            p.Start();   //00

            p.StandardInput.WriteLine(command);       //也可以用这种方式输入入要行的命令

            p.StandardInput.WriteLine("exit");        //要得加上Exit要不然下一行程式
                                                      //p.WaitForExit();
                                                      //p.Close();
                                                      //return p.StandardOutput.ReadToEnd();        //输出出流取得命令行结果果
            return p.StandardError.ReadToEnd();
        }
    }
}