using Microsoft.AspNetCore.Http;
using NetCoreObject.Common;
using NetCoreObject.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NetCoreObject.Services
{
    public class UploadService
    {
        private readonly SysBasicConfig _sysconfig;
        public UploadService()
        {
            _sysconfig = LoadConfig(Utils.GetXmlMapPath(KeyHelper.FILE_SITE_XML_CONFING));
        }

        /// <summary>
        ///  读取站点配置文件
        /// </summary>
        private SysBasicConfig LoadConfig(string configFilePath)
        {
            return (SysBasicConfig)SerializationHelper.Load(typeof(SysBasicConfig), configFilePath);
        }
        /// <summary>
        /// 单文件上传
        /// </summary>
        /// <param name="file">IFormFile 文件类型</param>
        /// <param name="isThumbnail">是否缩略图</param>
        /// <param name="isWater">是否增加水印</param>
        /// <returns></returns>
        public JsonFile SingleUpload(IFormFile file, bool isThumbnail = false, bool isWater = false)
        {
            var json = new JsonFile() { msg = "上传成功!", status = 500 };

            try
            {
                //扩展名
                var fileExt = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1, file.FileName.Length - file.FileName.LastIndexOf('.') - 1);

                if (!CheckFileExt(fileExt))
                {
                    json.msg = "不允许上传" + $@"{fileExt}" + "类型的文件!";
                    return json;
                }
                //检查文件大小是否合法
                if (!CheckFileSize(fileExt, Int32.Parse(file.Length.ToString())))
                {
                    json.msg = "文件大小超过限制大小("+ this._sysconfig.imgsize + "K)!";
                    return json;
                }
                json.status = 200;
                json.data = UploadFile(file, isThumbnail, isWater);
            }
            catch (Exception e)
            {
                json.status = 500;
                json.msg = "上传失败!"+e.Message;
            }
            return json;
        }

        /// <summary>
        ///  文件上传方法
        /// </summary>
        /// <param name="postedFile">文件类型</param>
        /// <param name="isThumbnail">是否缩略图</param>
        /// <param name="isWater">是否增加水印</param>        
        /// <returns></returns>
        public UploadFileInfo UploadFile(IFormFile fileinput, bool isThumbnail = false, bool isWater = false)
        {
            var filepath = ContentDispositionHeaderValue.Parse(fileinput.ContentDisposition).FileName.Trim('"');
            long fileSize = 0;
            //string fileExt = filepath.Substring(filepath.LastIndexOf('.')).Replace("\"", "");
            string fileExt= fileinput.FileName.Substring(fileinput.FileName.LastIndexOf('.') + 1, fileinput.FileName.Length - fileinput.FileName.LastIndexOf('.') - 1);
            //string fileExt = fileinput.FileName.Substring(filepath.LastIndexOf('.')).Replace("\"", "");
            string fileName = filepath.Substring(filepath.LastIndexOf(@"\", System.StringComparison.Ordinal) + 1); //取得原文件名
            //string fileExt =FileHelper.GetFileExt(postedFile.FileName); //文件扩展名，不含“.”
            //int fileSize = postedFile.ContentLength; //获得文件大小，以字节为单位
            //string fileName = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(@"\", System.StringComparison.Ordinal) + 1); //取得原文件名
            string newFileName = Utils.GetCheckCode(20).ToLower() + "." + fileExt; //随机生成新的文件名
            string newThumbnailFileName = "thumb_" + newFileName; //随机生成缩略图文件名
            string upLoadPath = AssigendPath(fileExt, _sysconfig.filerootpath +"/"+ _sysconfig.filepath); //上传目录相对路径
            string fullUpLoadPath = Utils.GetMapPath(upLoadPath); //上传目录的物理路径
            string newFilePath = upLoadPath + newFileName; //上传后的路径
            string newThumbnailPath = upLoadPath + newThumbnailFileName; //上传后的缩略图路径
            fileSize += fileinput.Length;
            //检查上传的物理路径是否存在，不存在则创建
            if (!Directory.Exists(fullUpLoadPath))
            {
                Directory.CreateDirectory(fullUpLoadPath);
            }
            
            using (FileStream fs = System.IO.File.Create(fullUpLoadPath + newFileName))
            {
                fileinput.CopyTo(fs);
                fs.Flush();
            }
            
            //保存文件
            //postedFile.SaveAs(fullUpLoadPath + newFileName);

            //如果是图片，检查图片是否超出最大尺寸，是则裁剪
            if (IsImage(fileExt) && (this._sysconfig.imgmaxheight > 0 || this._sysconfig.imgmaxwidth > 0))
            {
                Thumbnail.MakeThumbnailImage(fullUpLoadPath + newFileName, fullUpLoadPath + newFileName,
                    this._sysconfig.imgmaxwidth, this._sysconfig.imgmaxheight);
            }
            
            //如果是图片，检查是否需要生成缩略图，是则生成
            if (IsImage(fileExt) && isThumbnail && this._sysconfig.thumbnailwidth > 0 && this._sysconfig.thumbnailheight > 0)
            {
                Thumbnail.MakeThumbnailImage(fullUpLoadPath + newFileName, fullUpLoadPath + newThumbnailFileName,
                    this._sysconfig.thumbnailwidth, this._sysconfig.thumbnailheight, "H");
            }
            //如果是图片，检查是否需要打水印
            if (IsWaterMark(fileExt) && isWater)
            {
                switch (this._sysconfig.watermarktype)
                {
                    case 1:
                        //WaterMark.AddImageSignText(newFilePath, newFilePath,
                        //    this._sysconfig.watermarktext, this._sysconfig.watermarkposition,
                        //    this._sysconfig.watermarkimgquality, this._sysconfig.watermarkfont, this._sysconfig.watermarkfontsize);
                        WaterMark.AddWaterMark(newFilePath, newFilePath,
                            this._sysconfig.watermarktext, this._sysconfig.watermarkposition,
                            this._sysconfig.watermarkimgquality, this._sysconfig.watermarkfont, this._sysconfig.watermarkfontsize);
                        break;
                    case 2:
                        var watermarkpic = this._sysconfig.watermarkpic;
                        if (!watermarkpic.Contains("wwwroot"))
                        {
                            watermarkpic = "/wwwroot" + watermarkpic;
                        }
                        WaterMark.AddImageSignPic(newFilePath, newFilePath,
                            watermarkpic, this._sysconfig.watermarkposition,
                            this._sysconfig.watermarkimgquality, this._sysconfig.watermarktransparency);
                        break;
                }
            }
            var m = new UploadFileInfo()
            {
                OriginalName = filepath,
                Size = FileSizeTransf(fileSize),
                FileName = newFilePath,
                Name = newFileName,
                ThumbFileName = newThumbnailPath,
                ThumbName = newThumbnailFileName,
                FileExt = "." + fileExt,
                Paths = newFilePath
            };
            return m;
        }

        /// <summary>
        /// 是否需要打水印
        /// </summary>
        /// <param name="fileExt">文件扩展名，不含“.”</param>
        private bool IsWaterMark(string fileExt)
        {
            //判断是否开启水印
            if (this._sysconfig.watermarktype <= 0) return false;
            //判断是否可以打水印的图片类型
            ArrayList al = new ArrayList {"bmp", "jpeg", "jpg", "png"};
            if (al.Contains(fileExt.ToLower()))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 检查是否为合法的上传文件
        /// </summary>
        private bool CheckFileExt(string fileExt)
        {
            //检查危险文件
            string[] excExt = { "asp", "aspx", "php", "jsp", "htm", "html" };
            if (excExt.Any(t => string.Equals(t, fileExt, StringComparison.CurrentCultureIgnoreCase)))
            {
                return false;
            }
            //检查合法文件
            var allowExt = this._sysconfig.fileextension.Split(',');
            return allowExt.Any(t => string.Equals(t, fileExt, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// 检查文件大小是否合法
        /// </summary>
        /// <param name="fileExt">文件扩展名，不含“.”</param>
        /// <param name="fileSize">文件大小(B)</param>
        private bool CheckFileSize(string fileExt, int fileSize)
        {
            //判断是否为图片文件
            if (IsImage(fileExt))
            {
                if (this._sysconfig.imgsize > 0 && fileSize > this._sysconfig.imgsize * 1024)
                {
                    return false;
                }
            }
            else
            {
                if (this._sysconfig.attachsize > 0 && fileSize > this._sysconfig.attachsize * 1024)
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        ///  文件大小转为B、KB、MB、GB...
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public string FileSizeTransf(long size)
        {
            String[] units = new String[] { "B", "KB", "MB", "GB", "TB", "PB" };
            long mod = 1024;
            int i = 0;
            while (size > mod)
            {
                size /= mod;
                i++;
            }
            return size + units[i];

        }

        /// <summary>
        ///  根据文件类型分配路径
        /// </summary>
        /// <param name="fileExt"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string AssigendPath(string fileExt, string path)
        {
            if (!Path.IsPathRooted(path))
            {
                if (!path.StartsWith("/"))
                {
                    path = "/" + path;
                }
            }
            if (IsImage(fileExt))
                return path + "/images/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            if (IsVideos(fileExt))
                return path + "/videos/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            if (IsDocument(fileExt))
                return path + "/files/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            if (IsMusics(fileExt))
                return path + "/musics/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            return path + "/others/" + DateTime.Now.ToString("yyyyMMdd") + "/";
        }

        #region 文件格式
        /// <summary>
        /// 是否为图片
        /// </summary>
        /// <param name="fileExt">文件扩展名，不含“.”</param>
        /// <returns></returns>
        private static bool IsImage(string fileExt)
        {
            var images = new List<string> { "bmp", "gif", "jpg", "jpeg", "png" };
            if (images.Contains(fileExt.ToLower())) return true;
            return false;
        }
        /// <summary>
        /// 是否为视频
        /// </summary>
        /// <param name="fileExt">文件扩展名，不含“.”</param>
        /// <returns></returns>
        private static bool IsVideos(string fileExt)
        {
            var videos = new List<string> { "rmvb", "mkv", "ts", "wma", "avi", "rm", "mp4", "flv", "mpeg", "mov", "3gp", "mpg" };
            if (videos.Contains(fileExt.ToLower())) return true;
            return false;
        }
        /// <summary>
        /// 是否为音频
        /// </summary>
        /// <param name="fileExt">文件扩展名，不含“.”</param>
        /// <returns></returns>
        private static bool IsMusics(string fileExt)
        {
            var musics = new List<string> { "mp3", "wav" };
            if (musics.Contains(fileExt.ToLower())) return true;
            return false;
        }
        /// <summary>
        /// 是否为文档
        /// </summary>
        /// <param name="fileExt">文件扩展名，不含“.”</param>
        /// <returns></returns>
        private static bool IsDocument(string fileExt)
        {
            var documents = new List<string> { "doc", "docx", "xls", "xlsx", "ppt", "pptx", "txt", "pdf" };
            if (documents.Contains(fileExt.ToLower())) return true;
            return false;
        }

        /// <summary>
        /// 是否为压缩包
        /// </summary>
        /// <param name="fileExt">文件扩展名，不含“.”</param>
        /// <returns></returns>
        private static bool IsRar(string fileExt)
        {
            var documents = new List<string> { "rar" };
            if (documents.Contains(fileExt.ToLower())) return true;
            return false;
        }
        #endregion
    }
    public class UploadFileInfo
    {
        public string OriginalName { get; set; }
        public string Size { get; set; }

        public string FileName { get; set; }

        public string Name { get; set; }

        public string ThumbFileName { get; set; }
        public string ThumbName { get; set; }

        public string FileExt { get; set; }
        public string Paths { get; set; }
        public bool Status { get; set; }
        public string Msg { get; set; }

    }
}
