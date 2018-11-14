using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NetCoreObject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreObject
{
    /// <summary>
    /// 不做后台权限拦截
    /// </summary>
    public class MyNoActionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 在资源过滤器之后，绑定模型之后，方法执行之前
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
    }
}
