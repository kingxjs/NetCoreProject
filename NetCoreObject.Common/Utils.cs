using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace NetCoreObject.Common
{
    /// <summary>
    /// 公共方法
    /// </summary>
    public class Utils
    {
        public static string ServerPath = "";
        #region 显示错层方法
        /// <summary>
        /// 显示错层方法
        /// </summary>
        public static string LevelName(string name, decimal? level)
        {
            if (level > 0)
            {
                string nbsp = " ";
                for (int i = 0; i < level; i++)
                {
                    nbsp += "　";
                }
                name = nbsp + "  |--" + name;
            }
            return name;
        }
        #endregion


        #region 截取字符串
        public static string GetSubString(string pSrcString, int pLength, string pTailString)
        {
            return GetSubString(pSrcString, 0, pLength, pTailString);
        }
        public static string GetSubString(string pSrcString, int pStartIndex, int pLength, string pTailString)
        {
            string str = pSrcString;
            byte[] bytes = Encoding.UTF8.GetBytes(pSrcString);
            foreach (char ch in Encoding.UTF8.GetChars(bytes))
            {
                if (((ch > 'ࠀ') && (ch < '一')) || ((ch > 0xac00) && (ch < 0xd7a3)))
                {
                    if (pStartIndex >= pSrcString.Length)
                    {
                        return "";
                    }
                    return pSrcString.Substring(pStartIndex, ((pLength + pStartIndex) > pSrcString.Length) ? (pSrcString.Length - pStartIndex) : pLength);
                }
            }
            if (pLength < 0)
            {
                return str;
            }
            byte[] sourceArray = Encoding.Default.GetBytes(pSrcString);
            if (sourceArray.Length <= pStartIndex)
            {
                return str;
            }
            int length = sourceArray.Length;
            if (sourceArray.Length > (pStartIndex + pLength))
            {
                length = pLength + pStartIndex;
            }
            else
            {
                pLength = sourceArray.Length - pStartIndex;
                pTailString = "";
            }
            int num2 = pLength;
            int[] numArray = new int[pLength];
            byte[] destinationArray = null;
            int num3 = 0;
            for (int i = pStartIndex; i < length; i++)
            {
                if (sourceArray[i] > 0x7f)
                {
                    num3++;
                    if (num3 == 3)
                    {
                        num3 = 1;
                    }
                }
                else
                {
                    num3 = 0;
                }
                numArray[i] = num3;
            }
            if ((sourceArray[length - 1] > 0x7f) && (numArray[pLength - 1] == 1))
            {
                num2 = pLength + 1;
            }
            destinationArray = new byte[num2];
            Array.Copy(sourceArray, pStartIndex, destinationArray, 0, num2);
            return (Encoding.Default.GetString(destinationArray) + pTailString);
        }
        #endregion

        #region 对象转换处理
        /// <summary>
        /// 将字符串转换为int类型数组
        /// </summary>
        /// <param name="str">如1,2,3,4,5</param>
        /// <returns></returns>
        public static List<int> StrToListInt(string str)
        {
            var slist = str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<int>();
            foreach (var item in slist)
            {
                list.Add(int.Parse(item));
            }
            return list;
        }

        /// <summary>
        /// 将字符串转换为int类型数组
        /// </summary>
        /// <param name="str">如1,2,3,4,5</param>
        /// <returns></returns>
        public static List<string> StrToListString(string str)
        {
            var slist = str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<string>();
            foreach (var item in slist)
            {
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object expression)
        {
            return expression != null && IsNumeric(expression.ToString());
        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(string expression)
        {
            string str = expression;
            if (!(str?.Length > 0) || str.Length > 11 || !Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$")) return false;
            return (str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1');
        }

        /// <summary>
        /// 是否为Double类型
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object expression)
        {
            return expression != null && Regex.IsMatch(expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");
        }

        /// <summary>
        /// 将字符串转换为数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>字符串数组</returns>
        public static string[] GetStrArray(string str)
        {
            return str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 将字符串转换为列表
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>字符串列表</returns>
        public static List<int> GetStrList(string str)
        {
            var list = new List<int>();
            var temp = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in temp)
            {
                list.Add(Convert.ToInt32(item));
            }
            return list;
        }

        /// <summary>
        /// 将数组转换为字符串
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="speater">分隔符</param>
        /// <returns>String</returns>
        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// object型转换为bool型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
                return StrToBool(expression, defValue);

            return defValue;
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string expression, bool defValue)
        {
            if (expression != null)
            {
                if (string.Compare(expression, "true", StringComparison.OrdinalIgnoreCase) == 0)
                    return true;
                else if (string.Compare(expression, "false", StringComparison.OrdinalIgnoreCase) == 0)
                    return false;
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjToInt(object expression, int defValue)
        {
            if (expression != null)
                return StrToInt(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// 将字符串转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string expression, int defValue)
        {
            if (string.IsNullOrEmpty(expression) || expression.Trim().Length >= 11 || !Regex.IsMatch(expression.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            int rv;
            if (Int32.TryParse(expression, out rv))
                return rv;

            return Convert.ToInt32(StrToFloat(expression, defValue));
        }

        /// <summary>
        /// Object型转换为decimal型
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal ObjToDecimal(object expression, decimal defValue)
        {
            if (expression != null)
                return StrToDecimal(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal StrToDecimal(string expression, decimal defValue)
        {
            if ((expression == null) || (expression.Length > 10))
                return defValue;

            decimal intValue = defValue;
            bool isDecimal = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
            if (isDecimal)
                decimal.TryParse(expression, out intValue);
            return intValue;
        }

        /// <summary>
        /// Object型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjToFloat(object strValue, float defValue)
        {
            return strValue != null ? StrToFloat(strValue.ToString(), defValue) : defValue;
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string strValue, float defValue)
        {
            if ((strValue == null) || (strValue.Length > 10))
                return defValue;

            float intValue = defValue;
            {
                var isFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (isFloat)
                    float.TryParse(strValue, out intValue);
            }
            return intValue;
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime); return dtStart.Add(toNow);
        }

        /// <summary>
        /// 获得当前时间的毫秒总数
        /// </summary>
        /// <returns></returns>
        public static double GetTimeTotalSeconds()
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan span = DateTime.Now.Subtract(startTime);
            return span.TotalSeconds;
        }



        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str, DateTime defValue)
        {
            if (string.IsNullOrEmpty(str)) return defValue;
            DateTime dateTime;
            if (DateTime.TryParse(str, out dateTime))
                return dateTime;
            return defValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str)
        {
            return StrToDateTime(str, DateTime.Now);
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj)
        {
            return StrToDateTime(obj.ToString());
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj, DateTime defValue)
        {
            return StrToDateTime(obj.ToString(), defValue);
        }

        #endregion

        #region 分割字符串
        public static string[] SplitString(string strContent, char strSplit)
        {
            return strContent.Split(new char[] { strSplit }, StringSplitOptions.RemoveEmptyEntries);
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!string.IsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit, StringComparison.Ordinal) < 0)
                    return new string[] { strContent };

                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, int count)
        {
            string[] result = new string[count];
            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < count; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }
        #endregion

        #region 检测是否有Sql危险字符
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            //return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
            var returnValue = true;
            try
            {
                if (str.Trim() != "")
                {
                    var sqlStr = "exec|insert+|select+|delete|update|count|chr|mid|master+|truncate|char|declare|drop+|drop+table|creat+|create|*|iframe|script|";
                    sqlStr += "exec+|insert|delete+|update+|count(|count+|chr+|+mid(|+mid+|+master+|truncate+|char+|+char(|declare+|drop+table|creat+table";
                    var anySqlStr = sqlStr.Split('|');
                    var anySqlStrList = new List<string>();
                    anySqlStrList.AddRange(anySqlStr);
                    if (anySqlStrList.Contains(str.ToLower()))
                    {
                        returnValue = false;
                    }
                }
            }
            catch
            {
                returnValue = false;
            }
            return returnValue;
        }

        /// <summary>
        /// 检查危险字符
        /// </summary>
        /// <param name="sInput"></param>
        /// <returns></returns>
        public static string Filter(string sInput)
        {
            if (string.IsNullOrEmpty(sInput))
                return null;
            string sInput1 = sInput.ToLower();
            string output = sInput;
            string pattern = @"*|and|exec|insert|select|delete|update|count|master|truncate|declare|char(|mid(|chr(|'";
            if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                throw new Exception("字符串中含有非法字符!");
            }
            else
            {
                output = output.Replace("'", "''");
            }
            return output;
        }

        /// <summary> 
        /// 检查过滤设定的危险字符
        /// </summary>
        /// <param name="word"></param>
        /// <param name="inText">要过滤的字符串 </param> 
        /// <returns>如果参数存在不安全字符，则返回true </returns> 
        public static bool SqlFilter(string word, string inText)
        {
            if (inText == null)
                return false;
            foreach (string i in word.Split('|'))
            {
                if ((inText.ToLower().IndexOf(i + " ", StringComparison.Ordinal) > -1) || (inText.ToLower().IndexOf(" " + i, StringComparison.Ordinal) > -1))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 删除最后结尾的一个逗号
        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(",", StringComparison.Ordinal));
        }
        #endregion

        #region 删除最后结尾的指定字符后的字符
        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            if (str.LastIndexOf(strchar, StringComparison.Ordinal) >= 0 && str.LastIndexOf(strchar, StringComparison.Ordinal) == str.Length - 1)
            {
                return str.Substring(0, str.LastIndexOf(strchar, StringComparison.Ordinal));
            }
            return str;
        }
        #endregion

        #region 生成随机字母或数字

        private static readonly Random Random = new Random();

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <returns></returns>
        public static string Number(int length)
        {
            return Number(length, false);
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <param name="sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string Number(int length, bool sleep)
        {
            if (sleep)
                System.Threading.Thread.Sleep(2);
            string result = "";

            for (int i = 0; i < length; i++)
            {
                result += Random.Next(10).ToString();
            }
            return result;
        }
        /// <summary>
        /// 生成随机字母字符串(数字字母混和)
        /// </summary>
        /// <param name="codeCount">待生成的位数</param>
        public static string GetCheckCode(int codeCount)
        {
            string str = string.Empty;
            int rep = 0;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }

        /// <summary>
        /// 根据日期和随机码生成订单号
        /// </summary>
        /// <returns></returns>
        public static string GetOrderNumber()
        {
            string num = DateTime.Now.ToString("yyyyMMddHHmmssms"); //yyyyMMddHHmmssms
            return num + Number(2);
        }

        #endregion

        //#region 截取字符长度
        ///// <summary>
        ///// 截取字符长度
        ///// </summary>
        ///// <param name="inputString">字符</param>
        ///// <param name="len">长度</param>
        ///// <returns></returns>
        //public static string CutString(string inputString, int len)
        //{
        //    if (string.IsNullOrEmpty(inputString))
        //        return "";
        //    inputString = DropHtml(inputString);
        //    ASCIIEncoding ascii = new ASCIIEncoding();
        //    int tempLen = 0;
        //    string tempString = "";
        //    byte[] s = ascii.GetBytes(inputString);
        //    for (int i = 0; i < s.Length; i++)
        //    {
        //        if ((int)s[i] == 63)
        //        {
        //            tempLen += 2;
        //        }
        //        else
        //        {
        //            tempLen += 1;
        //        }

        //        try
        //        {
        //            tempString += inputString.Substring(i, 1);
        //        }
        //        catch
        //        {
        //            break;
        //        }

        //        if (tempLen > len)
        //            break;
        //    }
        //    //如果截过则加上半个省略号 
        //    byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
        //    if (mybyte.Length > len)
        //        tempString += "…";
        //    return tempString;
        //}

        ////public static string DropHtml(string htmlstring)
        ////{
        ////    if (string.IsNullOrEmpty(htmlstring)) return "";
        ////    //删除脚本  
        ////    htmlstring = Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
        ////    //删除HTML  
        ////    htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
        ////    htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
        ////    htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
        ////    htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
        ////    htmlstring = Regex.Replace(htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
        ////    htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
        ////    htmlstring = Regex.Replace(htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
        ////    htmlstring = Regex.Replace(htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
        ////    htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
        ////    htmlstring = Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
        ////    htmlstring = Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
        ////    htmlstring = Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
        ////    htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

        ////    htmlstring = Regex.Replace(htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
        ////    htmlstring = htmlstring.Replace("<", "");
        ////    htmlstring = htmlstring.Replace(">", "");
        ////    htmlstring = htmlstring.Replace("\r\n", "");
        ////    htmlstring = HttpContext.Current.Server.HtmlEncode(htmlstring).Trim();
        ////    return htmlstring;
        ////}
        //#endregion

        #region TXT代码转换成HTML格式
        /// <summary>
        /// 字符串字符处理
        /// </summary>
        /// <param name="chr">等待处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        /// //把TXT代码转换成HTML格式
        public static string ToHtml(string chr)
        {
            var sb = new StringBuilder(chr);
            sb.Replace("&", "&amp;");
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            sb.Replace("\r\n", "<br />");
            sb.Replace("\n", "<br />");
            sb.Replace("\t", " ");
            //sb.Replace(" ", "&nbsp;");
            return sb.ToString();
        }
        #endregion

        #region HTML代码转换成TXT格式
        /// <summary>
        /// 字符串字符处理
        /// </summary>
        /// <param name="input">等待处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        /// //把HTML代码转换成TXT格式
        public static String ToTxt(String input)
        {
            StringBuilder sb = new StringBuilder(input);
            sb.Replace("&nbsp;", " ");
            sb.Replace("<br>", "\r\n");
            sb.Replace("<br>", "\n");
            sb.Replace("<br />", "\n");
            sb.Replace("<br />", "\r\n");
            sb.Replace("&lt;", "<");
            sb.Replace("&gt;", ">");
            sb.Replace("&amp;", "&");
            return sb.ToString();
        }
        #endregion

        #region 检查是否为IP地址
        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        #endregion

        #region 获得配置文件节点XML文件的绝对路径
        public static string GetXmlMapPath(string xmlName)
        {
            return GetMapPath("/XmlConfig/WebSet.config");
        }
        #endregion

        #region 获得当前绝对路径
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (strPath.ToLower().StartsWith("http://"))
            {
                return strPath;
            }
            //if (HttpContextHelper.Current != null)
            //{
            //    return HttpContextHelper.Current.Server.MapPath(strPath);
            //}
            //else //非web程序引用
            //{
            strPath = strPath.Replace("//", "/").Replace("/", "\\");
            //if (strPath.StartsWith("\\"))
            //{
            //    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
            //}
            return FileHelper.MapPath(strPath);
            //return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            //}
        }
        #endregion

        #region 替换特殊信息   如手机号码 138****5566
        /// <summary>
        /// 替换特殊信息
        /// </summary>
        /// <param name="value">信息字符串</param>
        /// <param name="startLen">头部保留长度(默认4位)</param>
        /// <param name="endLen">尾部保留长度(默认4位)</param>
        /// <param name="specialChar">特殊字符格式(例如"*")</param>
        /// <returns></returns>
        public static string ReplaceWithSpecialChar(string value, int startLen = 4, int endLen = 4, char specialChar = '*')
        {
            try
            {
                int lenth = value.Length - startLen - endLen;
                string replaceStr = value.Substring(startLen, lenth);
                string specialStr = string.Empty;
                for (int i = 0; i < replaceStr.Length; i++)
                {
                    specialStr += specialChar;
                }
                value = value.Replace(replaceStr, specialStr);
            }
            catch (Exception)
            {
                throw;
            }
            return value;
        }
        #endregion

        #region POST/GET请求

        public static int Timeout = 5000;//时间点

        /// <summary>
        /// 获取请求的反馈信息
        /// </summary>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public static string DoGetRequest(string url)
        {
            HttpWebRequest hwRequest;

            string strResult = string.Empty;
            try
            {
                hwRequest = (System.Net.HttpWebRequest)WebRequest.Create(url);
                hwRequest.Timeout = Timeout;
                hwRequest.Method = "GET";
                hwRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            }
            catch
            {
                return strResult;
            }

            //get response
            try
            {
                var hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.UTF8);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch
            {
                return strResult;
            }

            return strResult;
        }


        /// <summary>
        /// 获取请求的反馈信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="bData">参数字节数组</param>
        /// <returns></returns>
        private static String DoPostRequest(string url, byte[] bData)
        {
            HttpWebRequest hwRequest;

            string strResult = string.Empty;
            try
            {
                ServicePointManager.Expect100Continue = false;//远程服务器返回错误: (417) Expectation failed 异常源自HTTP1.1协议的一个规范： 100(Continue)
                hwRequest = (HttpWebRequest)WebRequest.Create(url);
                hwRequest.Timeout = Timeout;
                hwRequest.Method = "POST";
                hwRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                hwRequest.ContentLength = bData.Length;
                Stream smWrite = hwRequest.GetRequestStream();
                smWrite.Write(bData, 0, bData.Length);
                smWrite.Close();
            }
            catch
            {
                return strResult;
            }

            //get response
            try
            {
                var hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.UTF8);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch
            {
                return strResult;
            }

            return strResult;
        }
        /// <summary>
        /// 构造WebClient提交
        /// </summary>
        /// <param name="url">提交地址</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        private static string DoPostRequest(string url, string encoding)
        {
            try
            {
                var wc = new WebClient();
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                int p = url.IndexOf("?", StringComparison.Ordinal);
                string sData = url.Substring(p + 1);
                url = url.Substring(0, p);
                byte[] data = Encoding.GetEncoding(encoding).GetBytes(sData);
                byte[] res = wc.UploadData(url, "POST", data);
                string result = Encoding.GetEncoding(encoding).GetString(res);
                return result;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// Post提交方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string PostMethod(string url, string param)
        {
            byte[] data = Encoding.UTF8.GetBytes(param);
            return DoPostRequest(url, data);
        }
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="encoding">编码gb2312/utf8</param>
        /// <param name="param">参数</param>
        /// <returns>结果</returns>
        public static string PostMethod(string url, string encoding, string param)
        {
            HttpWebRequest hwRequest;
            string strResult = string.Empty;
            byte[] bData = null;
            if (string.IsNullOrEmpty(param))
            {
                int p = url.IndexOf("?", StringComparison.Ordinal);
                string sData = "";
                if (p > 0)
                {
                    sData = url.Substring(p + 1);
                    url = url.Substring(0, p);
                }
                bData = Encoding.GetEncoding(encoding).GetBytes(sData);

            }
            else
            {
                bData = Encoding.GetEncoding(encoding).GetBytes(param);
            }
            try
            {
                ServicePointManager.Expect100Continue = false;//远程服务器返回错误: (417) Expectation failed 异常源自HTTP1.1协议的一个规范： 100(Continue)
                hwRequest = (HttpWebRequest)WebRequest.Create(url);
                hwRequest.Timeout = Timeout;
                hwRequest.Method = "POST";
                hwRequest.ContentType = "application/x-www-form-urlencoded";
                hwRequest.ContentLength = bData.Length;
                Stream smWrite = hwRequest.GetRequestStream();
                smWrite.Write(bData, 0, bData.Length);
                smWrite.Close();
            }
            catch
            {
                return strResult;
            }
            //get response
            try
            {
                var hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), encoding: Encoding.GetEncoding(encoding));
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch
            {
                return strResult;
            }

            return strResult;
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHttp(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.ContentType = "application/json";
            request.Method = "GET";
            request.Timeout = 20000;

            //byte[] btBodys = Encoding.UTF8.GetBytes(body);
            //httpWebRequest.ContentLength = btBodys.Length;
            //httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            var response = request.GetResponse() as HttpWebResponse;

            var httpWebResponse = (HttpWebResponse)request.GetResponse();

            var instream = response?.GetResponseStream();
            if (instream == null) return "";
            var sr = new StreamReader(instream, Encoding.UTF8);
            var responseContent = sr.ReadToEnd();

            httpWebResponse.Close();
            sr.Close();

            return responseContent;
        }
        #endregion

        #region 访问提交创建文件 （供生成静态页面使用，无需模板）
        /// <summary>
        /// 访问提交创建文件 （供生成静态页面使用，无需模板）
        /// 调用实例 Utils.CreateFileHtml("http://www.xiaomi.com", Server.MapPath("/xxx.html"));
        /// </summary>
        /// <param name="url">原网址</param>
        /// <param name="createpath">生成路径</param>
        /// <returns>true false</returns>
        public static bool CreateFileHtml(string url, string createpath)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string result = PostMethod(url, "");
                if (!string.IsNullOrEmpty(result))
                {
                    if (string.IsNullOrEmpty(createpath))
                    {
                        createpath = "/default.html";
                    }
                    string filepath = createpath.Substring(createpath.LastIndexOf(@"\", StringComparison.Ordinal));
                    createpath = createpath.Substring(0, createpath.LastIndexOf(@"\", StringComparison.Ordinal));
                    if (!Directory.Exists(createpath))
                    {
                        Directory.CreateDirectory(createpath);
                    }
                    createpath = createpath + filepath;
                    try
                    {
                        FileStream fs2 = new FileStream(createpath, FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs2, System.Text.Encoding.UTF8);
                        sw.Write(result);
                        sw.Close();
                        fs2.Close();
                        return true;
                    }
                    catch { return false; }
                }
                return false;
            }
            return false;
        }
        #endregion




        private static List<char> number_list = new List<char>() {
            '0','1','2','3','4','5','6','7','8','9'
        };
        private static List<char> lower_list = new List<char>() {
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'
        };
        private static List<char> upper_list = new List<char>() {
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
        };
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="Length">随机数长度</param>
        /// <param name="isnumber">是否包含数字，默认true</param>
        /// <param name="islower">是否包含小写字母，默认true</param>
        /// <param name="isupper">是否包含大写字母，默认true</param>
        /// <returns></returns>
        public static string GenerateRandomNumber(int Length, bool isnumber = true, bool islower = true, bool isupper = true)
        {
            var random_list = new List<char>();

            if (isnumber)
            {
                random_list.AddRange(number_list);
            }
            if (islower)
            {
                random_list.AddRange(lower_list);
            }
            if (isupper)
            {
                random_list.AddRange(upper_list);
            }

            StringBuilder newRandom = new StringBuilder();
            Random rd = new Random();

            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(random_list[rd.Next(random_list.Count)]);
            }
            return newRandom.ToString();
        }
        /// <summary>
        /// 验证手机号固定电话
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool CheckMobliePhone(string phone)
        {
            //电信手机号码正则        
            string dianxin = @"^1[3578][01379]\d{8}$";
            Regex dReg = new Regex(dianxin);
            //联通手机号正则        
            string liantong = @"^1[34578][01256]\d{8}$";
            Regex tReg = new Regex(liantong);
            //移动手机号正则        
            string yidong = @"^(134[012345678]\d{7}|1[34578][012356789]\d{8})$";
            Regex yReg = new Regex(yidong);

            if (dReg.IsMatch(phone) || tReg.IsMatch(phone) || yReg.IsMatch(phone))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 验证邮箱格式
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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
