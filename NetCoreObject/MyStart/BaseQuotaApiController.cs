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
    public class BaseQuotaApiController : BaseApiController
    {
        

        public BaseQuotaApiController(IConfiguration config, IHostingEnvironment _hostingEnvironment) : base(config, _hostingEnvironment)
        {
            
        }

       
    }
}