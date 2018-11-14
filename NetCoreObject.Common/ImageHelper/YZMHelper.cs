using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NetCoreObject.Common
{
    /// <summary>  
    /// 验证图片类  
    /// </summary>  
    public static class YZMHelper
    {
        public static byte[] CreateValidateGraphic(out string Code, int CodeLength, int Width, int Height, int FontSize)
        {
            String sCode = String.Empty;
            //颜色列表，用于验证码、噪线、噪点
            Color[] oColors ={
             Color.Black,
             Color.Red,
             Color.Blue,
             Color.Green,
             Color.Orange,
             Color.Brown,
             Color.Brown,
             Color.DarkBlue
            };
            //字体列表，用于验证码
            string[] oFontNames = { "Times New Roman", "MS Mincho", "Book Antiqua", "Gungsuh", "PMingLiU", "Impact" };
            //验证码的字元集，去掉了一些容易混淆的字元
            char[] oCharacter = {
       '2','3','4','5','6','8','9',
       'A','B','C','D','E','F','G','H','J','K', 'L','M','N','P','R','S','T','W','X','Y'
      };
            Random oRnd = new Random();
            Bitmap oBmp = null;
            Graphics oGraphics = null;
            int N1 = 0;
            System.Drawing.Point oPoint1 = default(System.Drawing.Point);
            System.Drawing.Point oPoint2 = default(System.Drawing.Point);
            string sFontName = null;
            Font oFont = null;
            Color oColor = default(Color);

            //生成验证码字串
            for (N1 = 0; N1 <= CodeLength - 1; N1++)
            {
                sCode += oCharacter[oRnd.Next(oCharacter.Length)];
            }

            oBmp = new Bitmap(Width, Height);
            oGraphics = Graphics.FromImage(oBmp);
            oGraphics.Clear(System.Drawing.Color.White);
            try
            {
                for (N1 = 0; N1 <= 4; N1++)
                {
                    //画噪线
                    oPoint1.X = oRnd.Next(Width);
                    oPoint1.Y = oRnd.Next(Height);
                    oPoint2.X = oRnd.Next(Width);
                    oPoint2.Y = oRnd.Next(Height);
                    oColor = oColors[oRnd.Next(oColors.Length)];
                    oGraphics.DrawLine(new Pen(oColor), oPoint1, oPoint2);
                }

                float spaceWith = 0, dotX = 0, dotY = 0;
                if (CodeLength != 0)
                {
                    spaceWith = (Width - FontSize * CodeLength - 10) / CodeLength;
                }

                for (N1 = 0; N1 <= sCode.Length - 1; N1++)
                {
                    //画验证码字串
                    sFontName = oFontNames[oRnd.Next(oFontNames.Length)];
                    oFont = new Font(sFontName, FontSize, FontStyle.Italic);
                    oColor = oColors[oRnd.Next(oColors.Length)];

                    dotY = (Height - oFont.Height) / 2 + 2;//中心下移2像素
                    dotX = Convert.ToSingle(N1) * FontSize + (N1 + 1) * spaceWith;

                    oGraphics.DrawString(sCode[N1].ToString(), oFont, new SolidBrush(oColor), dotX, dotY);
                }

                for (int i = 0; i <= 30; i++)
                {
                    //画噪点
                    int x = oRnd.Next(oBmp.Width);
                    int y = oRnd.Next(oBmp.Height);
                    Color clr = oColors[oRnd.Next(oColors.Length)];
                    oBmp.SetPixel(x, y, clr);
                }

                Code = sCode;
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                oBmp.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                oGraphics.Dispose();
            }
        }
        public static byte[] CreateCheckCodeImage(out string Code, int CodeLength, int FontSize = 12)
        {
            Random oRnd = new Random();
            string checkCode = string.Empty;
            //验证码的字元集，去掉了一些容易混淆的字元
            char[] oCharacter = {
       '2','3','4','5','6','8','9',
       'A','B','C','D','E','F','G','H','J','K', 'L','M','N','P','R','S','T','W','X','Y'
      };
            int N1 = 0;
            //生成验证码字串
            for (N1 = 0; N1 <= CodeLength - 1; N1++)
            {
                checkCode += oCharacter[oRnd.Next(oCharacter.Length)];
            }
            Bitmap image = new Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的背景噪音线
                for (int i = 0; i < 2; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Black), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", FontSize, (FontStyle.Bold));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new
                System.Drawing.Drawing2D.LinearGradientBrush(
                new Rectangle(0, 0, image.Width, image.Height),
                Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);
                //画图片的前景噪音点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Gif);
                Code = checkCode;
                return ms.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
    }
}
