using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetCoreObject.Common;
using NetCoreObject.Core;

namespace NetCoreObject.Areas.SysAdmin.Controllers
{
    [Area("SysAdmin")]
    public class SysBasicController : BaseController
    {
        public SysBasicController(IConfiguration config, IHostingEnvironment _hostingEnvironment) : base(config, _hostingEnvironment)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            //var model = RedisHelper.StringGet<SysBasicConfig>(KeyHelper.CACHE_SITE_CONFIG);

            //if (model != null) return View(model);
            //RedisHelper.StringSet(KeyHelper.CACHE_SITE_CONFIG, LoadConfig(Utils.GetXmlMapPath(KeyHelper.FILE_SITE_XML_CONFING)));
            //model = RedisHelper.StringGet<SysBasicConfig>(KeyHelper.CACHE_SITE_CONFIG);

            return View(BasicConfig);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Index(SysBasicConfig model1)
        {
            var jsonm = new ResultJson();
            try
            {
                var model = BasicConfig;

                if (string.IsNullOrEmpty(model1.emailpassword))
                {
                    model1.emailpassword = model.emailpassword;
                }
                if (string.IsNullOrEmpty(model1.smspassword))
                {
                    model1.smspassword = model.smspassword;
                }
                SerializationHelper.Save(model1, Utils.GetXmlMapPath(KeyHelper.FILE_SITE_XML_CONFING));
                //刷新配置项
                BasicConfigHelper.setBasicConfig();

                return Json(new ResultJson() { msg = "修改成功", backurl = "" });
            }
            catch (Exception ex)
            {
                jsonm.msg = "修改失败";
                jsonm.status = 500;
                LogProvider.Error("修改配置项", ex.StackTrace, ex.Message);
            }
            return Json(jsonm);
        }
    }
}