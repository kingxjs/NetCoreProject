using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using NetCoreObject.Common;
using NetCoreObject.Core;
using Microsoft.AspNetCore.Cors;

namespace NetCoreObject
{
    [EnableCors("AllowCors")]
    public class BaseApiController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        public static AccountToken SysUserModel;
        public static SysBasicConfig BasicConfig;
        public static IConfiguration _config;
        public SugarBase Service;

        public BaseApiController(IConfiguration config, IHostingEnvironment _hostingEnvironment)
        {
            Service = new SugarBase();
            _config = config;
            hostingEnvironment = _hostingEnvironment;
            BasicConfig = BasicConfigHelper.getBasicConfig().Result;
        }
    }
}