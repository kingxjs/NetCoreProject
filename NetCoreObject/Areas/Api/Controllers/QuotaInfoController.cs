using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetCoreObject.Common;
using System;
using NetCoreObject.Core;

namespace NetCoreObject.Areas.Api.Controllers
{
    [Produces("application/json")]
    //[Route("api/Quota")]
    [Route("api/[controller]/[action]")]
    public class QuotaInfoController : BaseQuotaApiController
    {
        public QuotaInfoController(IConfiguration config, IHostingEnvironment _hostingEnvironment) : base(config, _hostingEnvironment)
        {

        }

        [HttpPost]
        public ResultJson GetModuleList()
        {
            var jsonm = new ResultJson();

            try
            {

            }
            catch (Exception ex)
            {
                jsonm.status = 500;
                jsonm.msg = "获取失败";
                LogProvider.Error("获取平台模块", ex.StackTrace, ex.Message);
            }
            return jsonm;
        }
    }
}