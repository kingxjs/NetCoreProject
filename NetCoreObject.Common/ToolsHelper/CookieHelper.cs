using System;
using Microsoft.AspNetCore.Http;

namespace NetCoreObject.Common
{
    /// <summary>
    /// Cookie辅助类
    /// </summary>
    public class CookieHelper
    {
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpContextHelper.Current.Response.Cookies.Append(strName, strValue);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="expires">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpContextHelper.Current.Response.Cookies.Append(strName, strValue, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(expires)
            });
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            HttpContextHelper.Current.Request.Cookies.TryGetValue(strName, out string value);
            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            return value;
        }

        /// <summary>
        /// 清除指定Cookie
        /// </summary>
        /// <param name="cookiename">cookiename</param>
        public static void ClearCookie(string cookiename)
        {
            HttpContextHelper.Current.Response.Cookies.Delete(cookiename);
        }
    }
}
