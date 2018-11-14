using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NetCoreObject.Common
{
    public class MD5CryptHelper
    {
        /// <summary>
        /// 64位md5加密
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string MD5Encrypt64(string name)
        {
            string cl = name;
            //string pwd = "";
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            return Convert.ToBase64String(s);
        }
        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt32(string name)
        {
            string cl = name;
            string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("x2");//"X"
            }
            return pwd;
        }




        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, "SysAccountToken");
        }

        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey)
        {
            var result = "";

            try
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    var KEY_64 = ASCIIEncoding.ASCII.GetBytes(MD5Encrypt32(sKey).Substring(0, 8));
                    var IV_64 = ASCIIEncoding.ASCII.GetBytes(MD5Encrypt32(sKey).Substring(8, 16));


                    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new
                        CryptoStream(ms, cryptoProvider.CreateEncryptor(KEY_64, IV_64), CryptoStreamMode.Write);
                    StreamWriter sw = new StreamWriter(cs);
                    sw.Write(Text);
                    sw.Flush();
                    cs.FlushFinalBlock();
                    ms.Flush();

                    //再转换为一个字符串
                    result = Convert.ToBase64String(ms.GetBuffer(), 0, Int32.Parse(ms.Length.ToString()));
                }
            }
            catch (Exception)
            {
                result = "";
            }

            return result;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, "SysAccountToken");
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            var result = "";

            try
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    var KEY_64 = ASCIIEncoding.ASCII.GetBytes(MD5Encrypt32(sKey).Substring(0, 8));
                    var IV_64 = ASCIIEncoding.ASCII.GetBytes(MD5Encrypt32(sKey).Substring(8, 16));

                    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                    //从字符串转换为字节组
                    Byte[] buffer = Convert.FromBase64String(Text);
                    MemoryStream ms = new MemoryStream(buffer);
                    CryptoStream cs = new
                        CryptoStream(ms, cryptoProvider.CreateDecryptor(KEY_64, IV_64), CryptoStreamMode.Read);
                    StreamReader sr = new StreamReader(cs);
                    result = sr.ReadToEnd();
                }
            }
            catch (Exception)
            {
                result = "";
            }

            return result;
        }
    }
}
