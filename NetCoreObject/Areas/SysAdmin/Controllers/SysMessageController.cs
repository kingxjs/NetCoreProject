using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetCoreObject.Core;
using NetCoreObject.Common;
using SqlSugar;

namespace NetCoreObject.Areas.SysAdmin.Controllers
{
    [Area("SysAdmin")]
    public class SysMessageController : BaseController
    {
        public SysMessageController(IConfiguration config, IHostingEnvironment _hostingEnvironment) : base(config, _hostingEnvironment)
        {
        }
        /// <summary>
        /// 查看消息
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <returns></returns>
        public IActionResult SendIndex()
        {
            return View();
        }
        /// <summary>
        /// 编辑消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Change(string id)
        {
            var model = new SysMessage();

            Service.Command<Outsourcing>((db, o) =>
            {
                if (!string.IsNullOrEmpty(id))
                {
                    model = db.Queryable<SysMessage>().Where(m => m.ID == id).First();
                }
                else
                {
                    model.ID = "";
                }
            });
            return View(model);
        }
        /// <summary>
        /// 获取接收到的消息
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="Type">0，所有，1：通知，2：私信</param>
        /// <returns></returns>
        public JsonResult GetList(int limit, int page, int Type = 0)
        {
            var jsonm = new ResultJson();
            try
            {
                Service.Command<SysMessage>((db, o) =>
                {
                    var total = 0;
                    var list = db.Queryable<SysMessage>();
                    if (Type != 0)
                    {
                        list.Where(m => m.Type == Type);
                    }
                    else
                    {
                        list.Where(m => m.Type == 1 || m.Type == 2);
                    }
                    var list1 = list.OrderBy(m => m.CreateTime, OrderByType.Desc)
                        .ToPageList(page, limit, ref total).ToList();

                    jsonm.data = list1;
                    jsonm.count = total;
                    jsonm.code = 0;
                });
            }
            catch (Exception ex)
            {
                LogProvider.Error("消息中心", ex.StackTrace, ex.Message);

                jsonm.status = 500;
                jsonm.msg = "获取失败";
            }

            return Json(jsonm);
        }
        /// <summary>
        /// 获取发送的消息
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="Type">0，所有，1：我发送的通知，2：我发送的私信</param>
        /// <returns></returns>
        public JsonResult GetSendList(int limit, int page, int Type = 0)
        {
            var jsonm = new ResultJson();
            try
            {
                Service.Command<SysMessage>((db, o) =>
                {
                    var total = 0;
                    var list = db.Queryable<SysMessage>();
                    if (Type != 0)
                    {
                        Type = Type + 2;
                        list.Where(m => m.Type == Type);
                    }
                    else
                    {
                        list.Where(m => m.Type == 3 || m.Type == 4);
                    }
                    var list1 = list.OrderBy(m => m.CreateTime, OrderByType.Desc)
                        .ToPageList(page, limit, ref total).ToList();

                    jsonm.data = list1;
                    jsonm.count = total;
                    jsonm.code = 0;
                });
            }
            catch (Exception ex)
            {
                LogProvider.Error("消息中心", ex.StackTrace, ex.Message);

                jsonm.status = 500;
                jsonm.msg = "获取失败";
            }

            return Json(jsonm);
        }
    }
}