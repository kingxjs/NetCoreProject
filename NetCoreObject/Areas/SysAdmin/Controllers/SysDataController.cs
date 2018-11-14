using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetCoreObject.Common;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;
using NetCoreObject.Core;
using SqlSugar;

namespace NetCoreObject.Areas.SysAdmin.Controllers
{
    [Area("SysAdmin")]
    public class SysDataController : BaseController
    {
        public SysDataController(IConfiguration config, IHostingEnvironment _hostingEnvironment) : base(config, _hostingEnvironment)
        {
        }

        public IActionResult Index()
        {
            var number = Utils.GetOrderNumber();
            ViewBag.SqlNumber = number;
            var ConnStr = _config.GetConnectionString("DefaultConnection");
            string[] result = ConnStr.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Split('=')[1]).ToArray();
            ViewBag.database = result[3];
            ViewBag.paths = Utils.GetMapPath("/wwwroot/upload/backdb/") + result[3] + "_" + number + ".sql";
            ViewBag.taskpath = Utils.GetMapPath("/wwwroot/upload/backdb/");

            using (var db = SugarBase.GetIntance())
            {
                var taskModel = db.Queryable<SysTask>().Where(m => m.Type == "database").First();

                if (taskModel == null)
                {
                    taskModel = new SysTask();
                }

                ViewBag.Task = taskModel;
            }

            return View();
        }
        public IActionResult Downs()
        {
            return View();
        }
        /// <summary>
        /// 返回List
        /// </summary>
        /// <returns></returns>
        public JsonResult GetFiles()
        {
            var jsonm = new ResultJson();
            try
            {
                string path = "/wwwroot/upload/backdb";
                var list = ConvertHelper<FileModel>.ConvertToList(FileHelper.GetFileTable(Utils.GetMapPath(path)));
                jsonm.data = list.OrderByDescending(m => m.time);
                jsonm.count = list.Count();
                jsonm.code = 0;
            }
            catch (Exception e)
            {
                jsonm.msg = e.Message;
                jsonm.status = 500;
            }
            return Json(jsonm);
        }

        /// <summary>
        /// 下载数据库备份文件
        /// </summary>
        /// <returns></returns>
        public IActionResult FileDown()
        {
            try
            {
                var file = FytRequest.GetQueryString("file");

                SetSysLog("【下载】备份数据库文件", 8, 1);

                string spath = "/wwwroot/upload/backdb/" + file;

                var stream = System.IO.File.OpenRead(Utils.GetMapPath(spath));

                string fileExt = FileHelper.GetFileExt(file);

                //获取文件的ContentType

                //var provider = new FileExtensionContentTypeProvider();

                //var memi = provider.Mappings[fileExt];

                return File(stream, "application/pdf", Path.GetFileName(spath));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        public JsonResult Delete()
        {
            var jsonm = new ResultJson();
            try
            {
                string idlist = FytRequest.GetFormString("id");
                if (idlist.Contains(","))
                {
                    string[] str = idlist.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string t in str)
                    {
                        FileHelper.DeleteFile(Utils.GetMapPath("/wwwroot/upload/backdb/") + t);
                    }
                }
                else
                {
                    FileHelper.DeleteFile(Utils.GetMapPath("/wwwroot/upload/backdb/") + idlist);
                }
                SetSysLog("【删除】备份数据库文件", 8, 1);
            }
            catch (Exception e)
            {
                jsonm.msg = e.Message;
                jsonm.status = 500;
            }
            return Json(jsonm);
        }
        [HttpPost]
        public JsonResult BakBackUp(string SqlNumber)
        {
            if (string.IsNullOrEmpty(SqlNumber))
            {
                SqlNumber = Utils.GetOrderNumber();
            }
            var jsonm = new ResultJson() { msg = "备份任务正在后台处理，请稍后到备份文件菜单中查看！", backurl = "/SysAdmin/SysData/Index" };
            try
            {
                //数据库备份操作
                DataHelper.BakBackUpFun(SqlNumber);
                //添加备份数据库日志
                SetSysLog("【备份】数据库", 8, 1);

            }
            catch (Exception e)
            {
                jsonm.msg = e.Message;
                jsonm.status = 500;
            }
            return Json(jsonm);
        }

        [HttpPost]
        public JsonResult BakBackTask(SysTask task)
        {
            var jsonm = new ResultJson();
            try
            {
                //数据库备份操作
                //DataHelper.BakBackUpFun(SqlNumber);
                //添加备份数据库日志
                SetSysLog("【备份】数据库", 8, 1);

            }
            catch (Exception e)
            {
                jsonm.msg = e.Message;
                jsonm.status = 500;
            }
            return Json(jsonm);
        }
    }
}