using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreObject.Common;
using Microsoft.AspNetCore.Http;
using NetCoreObject.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace NetCoreObject.Areas.SysAdmin.Controllers
{
    [Area("SysAdmin")]
    public class FileUpController : BaseController
    {
        public IActionResult Index()
        {
            ViewBag.httpUrl = FytRequest.GetUrlNoParm();
            return View();
        }
        #region api

        readonly UploadService _up = new UploadService();

        public FileUpController(IConfiguration config, IHostingEnvironment _hostingEnvironment) : base(config, _hostingEnvironment)
        {
        }

        /// <summary>
        /// 获得文件列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetFileData()
        {
            string path = FytRequest.GetFormString("path");
            path = "/" + BasicConfig.filerootpath + "/" + path;
            var jsonm = new ResultJson();
            try
            {
                jsonm.data = ConvertHelper<FileModel>.ConvertToList(FileHelper.GetFileTable(Utils.GetMapPath(path)));
            }
            catch (Exception e)
            {
                jsonm.msg = e.Message;
                jsonm.status = 500;
            }
            return Json(jsonm);
        }
        /// <summary>
        /// 文章多张上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpShopGoods()
        {
            var jsonm = new ResultJson();
            var hfc = Request.Form.Files;
            if (hfc.Count > 0)
            {
                for (int i = 0; i < hfc.Count; i++)
                {
                    IFormFile hpf = Request.Form.Files[i];
                    var jsFile = _up.SingleUpload(hpf, true, false);
                    jsonm.status = jsFile.status;
                    jsonm.msg = jsFile.msg;
                    jsonm.data = jsFile.data;
                }
            }
            //GC.Collect();
            return Json(jsonm);
        }

        /// <summary>
        /// 单个文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SignUpFile()
        {
            var jsonm = new ResultJson();
            try
            {
                IFormFile photo = Request.Form.Files["fileUp"];



                var upFiles = FytRequest.GetQueryString("upFiles");
                if (upFiles == "openfileUp")
                {
                    photo = Request.Form.Files["openfileUp"];
                }
                if (upFiles == "scfile")
                {
                    photo = Request.Form.Files["scfile"];
                }
                if (upFiles == "fileUrlMd")
                {
                    photo = Request.Form.Files["editormd-image-file"];
                }
                //是否缩略图  0=不压缩  1=压缩
                var isThum = FytRequest.GetQueryInt("isThum", 0);
                //是否添加水印
                var isWater = FytRequest.GetQueryInt("isWater", 0);
                if (photo == null)
                {
                    jsonm.status = 401;
                    jsonm.msg = "请选择要上传文件！";
                    return Json(jsonm);
                }
                var jsFile = _up.SingleUpload(photo, Convert.ToBoolean(isThum), Convert.ToBoolean(isWater));
                jsonm.status = jsFile.status;
                jsonm.msg = jsFile.msg;
                //var fileModels = (UploadFileInfo)jsFile.data;
                jsonm.data = jsFile.data; //fileModels.Paths;

                if (upFiles == "fileUrlMd")
                {
                    if (jsonm.status == 200)
                    {
                        var fileModels = (UploadFileInfo)jsFile.data;
                        var obj = new
                        {
                            success = 1,
                            message = jsonm.msg,
                            url = fileModels.Paths
                        };
                        return Json(obj);
                    }
                }
            }
            catch (Exception e)
            {
                jsonm.msg = e.Message;
                jsonm.status = 500;
            }
            return Json(jsonm);
        }

        /// <summary>
        /// 删除文件或文件夹
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteBy()
        {
            var jsonm = new ResultJson();
            try
            {
                var path = FytRequest.GetFormString("path");
                var isFile = FytRequest.GetFormInt("isfile");
                if (isFile == 0)
                {
                    //删除文件夹
                    FileHelper.ClearDirectory(Utils.GetMapPath(path));
                }
                else
                {
                    //删除文件
                    FileHelper.DeleteFile(Utils.GetMapPath(path));
                }
            }
            catch (Exception e)
            {
                jsonm.msg = e.Message;
                jsonm.status = 500;
            }
            return Json(jsonm);
        }
        #endregion
    }
}