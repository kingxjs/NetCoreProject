using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreObject.Core;
using NetCoreObject.Common;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.IO;

namespace NetCoreObject.Areas.SysAdmin.Controllers
{
    [Area("SysAdmin")]
    public class AccountController : BaseController
    {
        public AccountController(IConfiguration config, IHostingEnvironment _hostingEnvironment) : base(config, _hostingEnvironment)
        {
        }

        [MyNoActionFilter]
        public IActionResult Login()
        {
            return View();
        }

        [MyNoActionFilter]
        public ActionResult GetImgCode()
        {
            int width = 125;
            int height = 48;
            int fontsize = 22;
            string code = string.Empty;
            byte[] bytes = YZMHelper.CreateValidateGraphic(out code, 4, width, height, fontsize);

            var sessionid = HttpContextHelper.Current.Session.Id;

            CookieHelper.WriteCookie("sessionid", sessionid);

            RedisHelper.StringSet("imgcode:" + sessionid, code);


            return File(bytes, @"image/jpeg");
        }


        [MyNoActionFilter]
        public JsonResult LoginSubmit(string username, string password, string vercode = "")
        {

            var jsonm = new ResultJson();
            try
            {
                ;
                var sessionid = CookieHelper.GetCookie("sessionid");
                var result = false;

                if (RedisHelper.KeyExists("imgcode:" + sessionid))
                {
                    var code = RedisHelper.StringGet("imgcode:" + sessionid);

                    if (code.Trim().ToUpper() == vercode.Trim().ToUpper())
                    {
                        result = true;
                    }
                    else
                    {
                        jsonm.status = 500;
                        jsonm.msg = "验证码错误";
                    }
                }

                if (result)
                {
                    using (var db = SugarBase.GetIntance())
                    {
                        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                        {
                            var model = db.Queryable<SysUser>().Where(m => m.Status == 1 && m.SysUserName.Equals(username.Trim()) && m.SysUserPwd.Equals(SHACryptHelper.SHA256Encrypt(password))).First();
                            if (model != null)
                            {
                                var userModel = new AccountToken();
                                userModel.UserID = model.SysUserID;
                                userModel.UserName = model.SysNickName;

                                var menu_list = db.Queryable<SysModule, SysRoleModule, SysRole, SysUserRole>((sm, srm, sr, sur) => new object[] {
        JoinType.Left,sm.ID==srm.ModuleID,
        JoinType.Left,srm.RoleID==sr.RoleID,
        JoinType.Left,sr.RoleID==sur.RoleID,
                        })
                                .Where((sm, srm, sr, sur) => sur.UserID == model.SysUserID && sm.Status)
                                .OrderBy((sm, srm, sr, sur) => sm.Sort, OrderByType.Desc)
                            .OrderBy((sm, srm, sr, sur) => sm.CreateTime, OrderByType.Asc)
               .Select(sm => new SysModule { ID = sm.ID, Href = sm.Href, Business = sm.Business, Icon = sm.Icon, Name = sm.Name, Sort = sm.Sort, Type = sm.Type, ParentID = sm.ParentID }).ToList();

                                CookieHelper.WriteCookie("systoken", MD5CryptHelper.Encrypt(JsonConvert.Serialize(userModel)), 30);


                                var claims = new[] {
                                new Claim("UserID", model.SysUserID),
                                new Claim("UserName", model.SysNickName)
                            };
                                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                                ClaimsPrincipal user = new ClaimsPrincipal(claimsIdentity);

                                //var identity = new ClaimsIdentity();
                                //identity.AddClaim(new Claim(ClaimTypes.Sid, userModel.UserID));
                                //identity.AddClaim(new Claim(ClaimTypes.Name, userModel.UserName));
                                HttpContextHelper.Current.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);



                                userModel.MenuList = menu_list;


                                var daySpan = TimeSpan.FromMinutes(30);
                                RedisHelper.StringSet("system:SysToken:" + userModel.UserID, MD5CryptHelper.Encrypt(JsonConvert.Serialize(userModel)), daySpan);
                                setMenuCache();

                                SetSysLog("【系统登录】" + userModel.UserName, 1, 1);
                                //HttpContext.Session.SetString("SysToken", MD5CryptHelper.Encrypt(JsonConvert.Serialize(userModel)));
                                jsonm.status = 200;
                                jsonm.msg = "登录成功";
                            }
                            else
                            {
                                jsonm.status = 500;
                                jsonm.msg = "账号或密码错误";
                            }
                        }
                        else
                        {
                            jsonm.status = 500;
                            jsonm.msg = "请填写账号信息";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                jsonm.status = 500;
                jsonm.msg = "登录失败";
                LogProvider.Error("登录", ex.StackTrace, ex.Message);
            }
            return Json(jsonm);
        }

        [MyNoActionFilter]
        public IActionResult Logout()
        {

            var jsonm = new ResultJson();
            try
            {
                using (var db = SugarBase.GetIntance())
                {
                    //var userModel = getToken();
                    if (SysUserModel != null)
                    {
                        if (RedisHelper.KeyExists("system:SysToken:" + SysUserModel.UserID))
                        {
                            HttpContextHelper.Current.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                            RedisHelper.KeyDelete("system:SysToken:" + SysUserModel.UserID);
                            CookieHelper.ClearCookie("systoken");
                            SetSysLog("【系统登出】" + SysUserModel.UserName, 1, 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                jsonm.status = 500;
                jsonm.msg = "登出失败";
                LogProvider.Error("登出", ex.StackTrace, ex.Message);
            }
            return Redirect("/SysAdmin/Account/Login");
        }
    }
}