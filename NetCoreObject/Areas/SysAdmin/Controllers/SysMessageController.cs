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

        public IActionResult Index()
        {
            return View();
        }
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
                    var list1 = list.OrderBy(m => m.CreateTime, OrderByType.Desc)
                        .ToPageList(page, limit, ref total).ToList();

                    jsonm.data = list1;
                    jsonm.count = total;
                    jsonm.code = 0;
                });
                //using (var db = SugarBase.GetIntance())
                //{
                //    var total = 0;
                //    var list = db.Queryable<SysMessage>();
                //    if (Type != 0)
                //    {
                //        list.Where(m => m.Type == Type);
                //    }
                //    var list1 = list.OrderBy(m => m.CreateTime, OrderByType.Desc)
                //        .ToPageList(page, limit, ref total).ToList();

                //    jsonm.data = list1;
                //    jsonm.count = total;
                //    jsonm.code = 0;
                //}
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