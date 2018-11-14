using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetCoreObject.Common;
using NetCoreObject.Core;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreObject
{
    public class AccountToken
    {
        public string UserID { get; set; }
        public string UserName { get; set; }

        public List<SysModule> MenuList { get; set; }
    }
    public class BaseController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        public static AccountToken SysUserModel;
        public static SysBasicConfig BasicConfig;
        public static IConfiguration _config;
        public SugarBase Service;
        public List<SysModule> ModuleList { get; set; }

        public BaseController(IConfiguration config, IHostingEnvironment _hostingEnvironment)
        {
            Service = new SugarBase();
            _config = config;
            //SugarBase.SetConnectionString(config);
            //RedisHelper.GetIntance(config);
            hostingEnvironment = _hostingEnvironment;
            //Utils.ServerPath = hostingEnvironment.ContentRootPath;
            SysUserModel = getToken();
            BasicConfig = BasicConfigHelper.getBasicConfig().Result;
            //DataHelper.GetIntance(config);
            ModuleList = GetSysModule();
        }

        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <returns></returns>
        public AccountToken getToken()
        {
            var model = new AccountToken();
            var MenuList = new List<SysModule>();
            try
            {
                var auth = HttpContextHelper.Current.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).Result;
                if (auth.Succeeded)
                {
                    var tokenCookieModel = new AccountToken() { UserID = HttpContextHelper.Current.User.Claims.First().Value };
                    //}
                    //    var tokenCookie = CookieHelper.GetCookie("systoken");
                    //if (!string.IsNullOrEmpty(tokenCookie))
                    //{
                    //var tokenCookieModel = JsonConvert.Deserialize<AccountToken>(MD5CryptHelper.Decrypt(tokenCookie));

                    if (RedisHelper.KeyExistsAsync("system:SysToken:" + tokenCookieModel.UserID).Result)
                    {
                        var SysTokenStr = RedisHelper.StringGetAsync("system:SysToken:" + tokenCookieModel.UserID).Result;
                        if (!string.IsNullOrEmpty(SysTokenStr))
                        {
                            model = JsonConvert.Deserialize<AccountToken>(MD5CryptHelper.Decrypt(SysTokenStr));
                            if (model == null)
                            {
                                model = new AccountToken();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                model = new AccountToken();
            }
            
            return model;
        }
        public List<SysModule> GetSysModule() {
            var list = new List<SysModule>();
            try
            {
                var MenuListStr = RedisHelper.StringGet("system:SysModule");
                if (!string.IsNullOrEmpty(MenuListStr))
                {
                    list = JsonConvert.DeserializeJsonToList<SysModule>(MD5CryptHelper.Decrypt(MenuListStr));
                }
            }
            catch (Exception)
            {
                list = new List<SysModule>();
            }
            return list;
        }
        /// <summary>
        /// 生成账号
        /// </summary>
        /// <param name="Length">随机数长度</param>
        /// <param name="isnumber">是否包含数字，默认true</param>
        /// <param name="islower">是否包含小写字母，默认true</param>
        /// <param name="isupper">是否包含大写字母，默认true</param>
        /// <returns></returns>
        public string GetUserAccount(int Length, bool isnumber = true, bool islower = true, bool isupper = true)
        {
            var UserAccount = Utils.GenerateRandomNumber(Length, isnumber, islower, isupper);
            using (var db = SugarBase.GetIntance())
            {
                var isBo = db.Queryable<UserInfo>().Where(it => it.UserAccount.Contains(UserAccount)).Any();
                if (isBo)
                {
                    UserAccount = GetUserAccount(Length);
                }
            }
            return UserAccount;
        }
        /// <summary>
        /// 刷新菜单
        /// </summary>
        public void setMenuCache()
        {
            using (var db = SugarBase.GetIntance())
            {
                var menu_list_all = db.Queryable<SysModule>().Where(m=>m.Status).ToList();
                RedisHelper.StringSet("system:SysModule", MD5CryptHelper.Encrypt(JsonConvert.Serialize(menu_list_all)));
            }
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="Title">日志内容</param>
        /// <param name="LogType">日志类型，1:用户登录日志，2:内容操作日志，3:菜单操作日志，4:模板管理日志，5:数据备份日志，6:组件操作日志</param>
        /// <param name="LogGrade">日志级别,升序</param>
        public void SetSysLog(string Title, int LogType, int LogGrade)
        {
            try
            {
                var logModel = new SysLog()
                {
                    UserName = SysUserModel.UserName,
                    UserID = SysUserModel.UserID,
                    Title = Title,
                    IP = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
                    LogType = LogType,
                    LogGrade = LogGrade.ToString(),
                    RequestUrl = FytRequest.GetRawUrl(),
                    AddDate = DateTime.Now
                };
                using (var db = SugarBase.GetIntance())
                {
                    db.Insertable(logModel).ExecuteCommandAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
