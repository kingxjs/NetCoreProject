using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NetCoreObject.Common;
using NetCoreObject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreObject
{
    public class MyActionFilter : ActionFilterAttribute
    {
        filterContextInfo fcinfo;
        /// <summary>
        /// 在资源过滤器之后，绑定模型之后，方法执行之前
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            fcinfo = new filterContextInfo(context);
            var isDefined = false;
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                isDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                   .Any(a => a.GetType().Equals(typeof(MyNoActionFilter)));
            }
            var AreasList = new string[] { "Admin", "SysAdmin" };
            if (isDefined) return;
            if (!AreasList.Contains(fcinfo.moduleName))
            {
                return;
            }
            bool result = true;
            var auth = HttpContextHelper.Current.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).Result;
            if (auth.Succeeded)
            {
                var tokenCookieModel = new AccountToken() { UserID = HttpContextHelper.Current.User.Claims.First().Value };
                //var tokenCookie = CookieHelper.GetCookie("systoken");
                //if (!string.IsNullOrEmpty(tokenCookie))
                //{
                //var tokenCookieModel = JsonConvert.Deserialize<AccountToken>(MD5CryptHelper.Decrypt(tokenCookie));
                if (tokenCookieModel != null)
                {
                    if (RedisHelper.KeyExistsAsync("system:SysToken:" + tokenCookieModel.UserID).Result)
                    {
                        var SysTokenStr = RedisHelper.StringGetAsync("system:SysToken:" + tokenCookieModel.UserID).Result;
                        if (!string.IsNullOrEmpty(SysTokenStr))
                        {
                            var model = JsonConvert.Deserialize<AccountToken>(MD5CryptHelper.Decrypt(SysTokenStr));
                            if (model != null)
                            {

                                if (model.MenuList != null && (fcinfo.moduleName != "SysAdmin" && fcinfo.controllerName != "Home") || (fcinfo.moduleName == "SysAdmin" && fcinfo.controllerName != "Home"))
                                {
                                    var SysModuleListStr = "";
                                    if (RedisHelper.KeyExistsAsync("system:SysModule").Result)
                                    {
                                        SysModuleListStr = RedisHelper.StringGetAsync("system:SysModule").Result;
                                    }
                                    var SysModuleList = JsonConvert.DeserializeJsonToList<SysModule>(MD5CryptHelper.Decrypt(SysModuleListStr));
                                    bool isMenu = false;
                                    foreach (var item in SysModuleList)
                                    {
                                        if (context.HttpContext.Request.Path.Value.Equals(item.Href))
                                        {
                                            isMenu = true;
                                            break;
                                        }
                                    }
                                    if (isMenu)
                                    {
                                        isMenu = false;
                                        foreach (var item in model.MenuList)
                                        {
                                            if (context.HttpContext.Request.Path.Value.Equals(item.Href))
                                            {
                                                isMenu = true;
                                                break;
                                            }
                                        }
                                        if (!isMenu)
                                        {
                                            context.HttpContext.Response.StatusCode = 404;
                                        }
                                    }
                                }

                                result = false;
                            }
                        }
                    }
                }
            }


            if (result)
            {
                //context.Result = new RedirectResult("/SysAdmin/Account/Login");
                //context.Result = new RedirectToRouteResult("SysAdmin/Login", new RouteValueDictionary { });
                context.HttpContext.Response.WriteAsync(
                         " <script type='text/javascript'>window.top.location='/SysAdmin/Account/Login'; </script>");
                context.Result = new EmptyResult();
                return;
            }
            if (context.HttpContext.Response.StatusCode == 404)
            {
                context.Result = new RedirectResult("/error/404");
                return;
            }
            base.OnActionExecuting(context);

            //throw new NotImplementedException();
        }
    }
    public class filterContextInfo
    {
        public filterContextInfo(ActionExecutingContext filterContext)
        {
            #region 获取链接中的字符
            // 获取域名
            domainName = filterContext.HttpContext.Request.Path;

            //if (filterContext.HttpContext.Request.Url.Segments.Count() >= 2)
            //{
            //    moduleName = filterContext.HttpContext.Request.Url.Segments[1].Replace('/', ' ').Trim();
            //}
            //获取模块名称
            if (filterContext.RouteData.Values.Keys.Contains("area"))
            {
                moduleName = filterContext.RouteData.Values["area"].ToString();
            }

            //获取 controllerName 名称
            controllerName = filterContext.RouteData.Values["controller"].ToString();

            //获取ACTION 名称
            actionName = filterContext.RouteData.Values["action"].ToString();

            #endregion
        }
        /// <summary>
        /// 获取域名
        /// </summary>
        public string domainName { get; set; }
        /// <summary>
        /// 获取模块名称
        /// </summary>
        public string moduleName { get; set; }
        /// <summary>
        /// 获取 controllerName 名称
        /// </summary>
        public string controllerName { get; set; }
        /// <summary>
        /// 获取ACTION 名称
        /// </summary>
        public string actionName { get; set; }

    }
}
