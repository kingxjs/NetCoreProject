using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace NetCoreObject.Areas.SysAdmin.Controllers
{
    [Area("SysAdmin")]
    public class WorkflowController : BaseController
    {
        public WorkflowController(IConfiguration config, IHostingEnvironment _hostingEnvironment) : base(config, _hostingEnvironment)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}