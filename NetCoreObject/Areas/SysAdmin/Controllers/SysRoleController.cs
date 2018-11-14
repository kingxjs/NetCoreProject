using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreObject.Core;
using NetCoreObject.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace NetCoreObject.Areas.SysAdmin.Controllers
{
    [Area("SysAdmin")]
    public class SysRoleController : BaseController
    {
        public SysRoleController(IConfiguration config, IHostingEnvironment _hostingEnvironment) : base(config, _hostingEnvironment)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Change(string id)
        {
            var pid = FytRequest.GetQueryString("pid");
            SysRole model = new SysRole();
            var rel_list = new List<string>();
            using (var db = SugarBase.GetIntance())
            {
                if (!string.IsNullOrEmpty(id))
                {
                    model = db.Queryable<SysRole>().Where(m => m.RoleID == id).First();

                    var rel_list1 = db.Queryable<SysRoleModule>().Where(m => m.RoleID == model.RoleID).ToList();
                    if (rel_list1 != null)
                    {
                        foreach (var item in rel_list1)
                        {
                            rel_list.Add(item.ModuleID);
                        }
                    }
                }
                else
                {
                    model.RoleID = "";
                }

                var menu_list = db.Queryable<SysModule>().ToList();
                if (rel_list.Count > 0)
                {
                    foreach (var item in menu_list)
                    {
                        if (rel_list.Contains(item.ID))
                        {
                            item.LAY_CHECKED = true;
                        }
                    }
                }
                ViewBag.MenuList = menu_list;
                ViewBag.RelMenuList = rel_list;

            }
            return View(model);
        }

        public JsonResult GetList(int limit, int page, string Name, bool Status = true)
        {
            var lstRes = new List<SysRole>();
            int startIndex = (page == 1 || page == 0) ? 0 : limit * (page - 1);
            var total = 0;
            try
            {
                using (var db = SugarBase.GetIntance())
                {
                    if (!string.IsNullOrEmpty(Name))
                    {
                        lstRes = db.Queryable<SysRole>()
                        .Where(m => m.RoleName.Contains(Name.Trim()))
                        .OrderBy(m => m.CreateTime)
                        .ToPageList(page, limit, ref total).ToList();
                    }
                    else
                    {
                        lstRes = db.Queryable<SysRole>()
                        .OrderBy(m => m.CreateTime)
                        .ToPageList(page, limit, ref total).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogProvider.Error("获取系统角色列表", ex.StackTrace, ex.Message);
            }
            var rows = lstRes;
            return Json(new { code = 0, count = total, data = rows });
        }
        public JsonResult ChangeSubmit(SysRole Model)
        {
            var jsonm = new ResultJson();
            jsonm.data = Model;
            var model = new SysRole();
            try
            {
                using (var db = SugarBase.GetIntance())
                {
                    if (!string.IsNullOrEmpty(Model.RoleID))
                    {
                        model = db.Queryable<SysRole>().Where(m => m.RoleID == Model.RoleID).First();
                    }
                    model.RoleName = Model.RoleName;
                    model.Remarks = Model.Remarks;
                    if (string.IsNullOrEmpty(Model.RoleID))
                    {
                        db.Insertable(model).ExecuteCommand();
                        jsonm.status = 200;
                    }
                    else
                    {
                        db.Updateable(model).ExecuteCommand();
                        jsonm.status = 200;
                    }

                    db.Deleteable<SysRoleModule>().Where(m => m.RoleID == model.RoleID).ExecuteCommand();
                    if (Model.ModuleList != null)
                    {
                        List<SysRoleModule> rel_list = new List<SysRoleModule>();
                        foreach (var item in Model.ModuleList)
                        {
                            if (!string.IsNullOrEmpty(item.ID))
                            {
                                var SysRoleMenuModel = new SysRoleModule();
                                SysRoleMenuModel.ModuleID = item.ID;
                                SysRoleMenuModel.RoleID = model.RoleID;
                                rel_list.Add(SysRoleMenuModel);
                            }
                        }
                        if (rel_list.Count > 0)
                        {
                            db.Insertable(rel_list).ExecuteCommand();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogProvider.Error("编辑角色", ex.StackTrace, ex.Message);

                jsonm.status = 500;
                jsonm.msg = "保存失败";
            }


            return Json(jsonm);
        }
        public JsonResult setStatus(string ID, bool Status)
        {
            var jsonm = new ResultJson();
            try
            {
                using (var db = SugarBase.GetIntance())
                {
                    if (!string.IsNullOrEmpty(ID))
                    {
                        var model = db.Queryable<SysRole>().Where(m => m.RoleID == ID).First();
                        if (model != null)
                        {
                            model.Status = Status;
                            db.Updateable(model).ExecuteCommand();
                        }
                    }
                    else
                    {
                        jsonm.status = 500;
                        jsonm.msg = "修改失败";
                    }
                }
            }
            catch (Exception ex)
            {
                LogProvider.Error("改变系统角色状态", ex.StackTrace, ex.Message);
                jsonm.status = 500;
                jsonm.msg = "修改失败";
            }
            return Json(jsonm);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult DeleteSubmit(string ids)
        {
            var jsonm = new ResultJson();
            List<string> IDs = new List<string>();
            try
            {
                using (var db = SugarBase.GetIntance())
                {
                    if (!string.IsNullOrEmpty(ids))
                    {
                        string[] ss = ids.Split(',');
                        IDs.AddRange(ss);
                    }
                    jsonm.data = IDs;
                    if (IDs.Count > 0)
                    {
                        List<string> names = new List<string>();
                        var list = db.Queryable<SysRole>().Where(m => IDs.Contains(m.RoleID)).ToList();
                        foreach (var item in list)
                        {
                            names.Add(item.RoleName);
                        }
                        SetSysLog("【删除角色】" + string.Join(",", names), 3, 1);
                        db.Deleteable<SysRole>().Where(m => IDs.Contains(m.RoleID)).ExecuteCommand();
                        jsonm.status = 200;
                    }
                    else
                    {
                        jsonm.status = 500;
                        jsonm.msg = "请选择角色";
                    }
                }
            }
            catch (Exception ex)
            {
                jsonm.status = 500;
                jsonm.msg = "删除失败";
                LogProvider.Error("删除菜单", ex.StackTrace, ex.Message);
            }
            return Json(jsonm);
        }
    }
}