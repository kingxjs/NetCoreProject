using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreObject.Controllers
{
    public class ErrorController : Controller
    {
        [Route("error/404")]
        [MyNoActionFilter]
        public IActionResult Error404()
        {
            return View();
        }

        [Route("error/{code:int}")]
        [MyNoActionFilter]
        public IActionResult Error(int code)
        {
            // handle different codes or just return the default error view
            return View();
        }
    }
}