using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetCoreObject.Common;
using NetCoreObject.Core;
using SqlSugar;

namespace NetCoreObject.Areas.SysAdmin.Controllers
{
    [Area("SysAdmin")]
    public class HomeController : BaseController
    {
        public HomeController(IConfiguration config, IHostingEnvironment _hostingEnvironment) : base(config, _hostingEnvironment)
        {
        }

        public IActionResult Index()
        {
            //var list = DB.Queryable<SysMenu>().ToList();
            var objList = new List<object>();
            //var TokenModel = getToken();
            var list = SysUserModel.MenuList;
            var menu_ids = new List<string>();
            if (list != null)
            {
                foreach (var item in list.Where(m=>m.Type==1))
                {
                    if (!menu_ids.Contains(item.ID))
                    {
                        var obj = new
                        {
                            menuId = item.ID,
                            menuName = item.Name,
                            menuIcon = item.Icon,
                            menuHref = !string.IsNullOrEmpty(item.Href) ? item.Href : "",
                            parentMenuId = item.ParentID
                        };
                        objList.Add(obj);
                        menu_ids.Add(item.ID);
                    }
                }
            }
            ViewBag.objList = objList;
            ViewBag.TokenModel = SysUserModel;
            return View();
        }
        public IActionResult Welcome()
        {
            ViewBag.SysUserModel = SysUserModel;
            return View();
        }
        public JsonResult RefreshCache()
        {

            var jsonm = new ResultJson();
            try
            {
                using (var db = SugarBase.GetIntance())
                {
                    //刷新配置项
                    //RedisHelper.StringSet(KeyHelper.CACHE_SITE_CONFIG, LoadConfig(Utils.GetXmlMapPath(KeyHelper.FILE_SITE_XML_CONFING)));
                    BasicConfigHelper.setBasicConfig();
                    //刷新菜单
                    setMenuCache();
                    //刷新当前登录权限
                    var usermodel = db.Queryable<SysUser>().Where(m => m.SysUserID == SysUserModel.UserID).First();
                    if (usermodel != null)
                    {
                        var userModel = new AccountToken();
                        userModel.UserID = usermodel.SysUserID;
                        userModel.UserName = usermodel.SysNickName;

                        var menu_list = db.Queryable<SysModule, SysRoleModule, SysRole, SysUserRole>((sm, srm, sr, sur) => new object[] {
        JoinType.Left,sm.ID==srm.ModuleID,
        JoinType.Left,srm.RoleID==sr.RoleID,
        JoinType.Left,sr.RoleID==sur.RoleID,
                        })
                        .Where((sm, srm, sr, sur) => sur.UserID == usermodel.SysUserID)
                        .OrderBy((sm, srm, sr, sur) => sm.Sort, OrderByType.Desc)
                        .Select(sm => new SysModule { ID = sm.ID, Href = sm.Href, Business = sm.Business, Icon = sm.Icon, Name = sm.Name, Sort = sm.Sort, Type = sm.Type, ParentID = sm.ParentID }).ToList();

                        userModel.MenuList = menu_list;

                        var daySpan = TimeSpan.FromMinutes(30);
                        RedisHelper.StringSet("system:SysToken:" + userModel.UserID, MD5CryptHelper.Encrypt(JsonConvert.Serialize(userModel)), daySpan);
                    }
                }
            }
            catch (Exception ex)
            {
                jsonm.status = 500;
                jsonm.msg = "清理失败";
                LogProvider.Error("清理缓存", ex.StackTrace, ex.Message);
            }

            return Json(jsonm);
        }
    }
}