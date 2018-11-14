using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreObject.Core;
using SqlSugar;
using NetCoreObject.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace NetCoreObject.Areas.SysAdmin.Controllers
{
    [Area("SysAdmin")]
    public class SysModuleController : BaseController
    {
        public SysModuleController(IConfiguration config, IHostingEnvironment _hostingEnvironment) : base(config, _hostingEnvironment)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Change(string id)
        {
            var pid = FytRequest.GetQueryString("pid");
            var model = new SysModule();

            Service.Command<Outsourcing>((db, o) =>
            {
                if (!string.IsNullOrEmpty(id))
                {
                    model = db.Queryable<SysModule>().Where(m => m.ID == id).First();
                }
                else
                {
                    model.ParentID = pid;
                    model.ID = "";
                }
                var List = db.Queryable<SysModule>().Where(m => m.ID != model.ID).ToList();
                ViewBag.List = JsonConvert.Serialize(o.GetSysModuleJson(List, "0"));
            });



            return View(model);
        }

        public JsonResult GetList(string Name)
        {
            var lstRes = new List<SysModule>();
            try
            {
                using (var db = SugarBase.GetIntance())
                {
                    if (!string.IsNullOrEmpty(Name))
                    {
                        lstRes = db.Queryable<SysModule>()
                        .Where(m => m.Name.Contains(Name.Trim()))
                        .OrderBy(m => m.Type)
                        .OrderBy(m => m.Sort, OrderByType.Desc)
                        .OrderBy(m => m.CreateTime, OrderByType.Asc).ToList();
                    }
                    else
                    {
                        lstRes = db.Queryable<SysModule>()
                        .OrderBy(m => m.Type)
                        .OrderBy(m => m.Sort, OrderByType.Desc)
                        .OrderBy(m => m.CreateTime, OrderByType.Asc).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogProvider.Error("获取项目组列表", ex.StackTrace, ex.Message);
            }

            return Json(new { code = 0, data = lstRes });
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
                        var model = db.Queryable<SysModule>().Where(m => m.ID == ID).First();
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
                LogProvider.Error("改变系统模块状态", ex.StackTrace, ex.Message);
                jsonm.status = 500;
                jsonm.msg = "修改失败";
            }
            return Json(jsonm);
        }
        public JsonResult ChangeSubmit(SysModule Model)
        {
            var jsonm = new ResultJson();
            jsonm.data = Model;
            var model = new SysModule();
            try
            {
                using (var db = SugarBase.GetIntance())
                {
                    if (!string.IsNullOrEmpty(Model.ID))
                    {
                        model = db.Queryable<SysModule>().Where(m => m.ID == Model.ID).First();
                    }
                    if (model == null)
                    {
                        model = new SysModule();
                    }
                    model.Icon = Model.Icon;
                    model.Href = Model.Href;
                    model.Sort = Model.Sort;
                    model.Type = Model.Type;
                    model.Remarks = Model.Remarks;
                    model.Business = Model.Business;
                    model.ParentID = Model.ParentID;
                    if (string.IsNullOrEmpty(Model.ID))
                    {
                        SetSysLog("【添加菜单】" + Model.Name, 3, 1);
                        model.Name = Model.Name;
                        db.Insertable(model).ExecuteCommand();
                        jsonm.status = 200;
                    }
                    else
                    {
                        SetSysLog("【编辑菜单】" + model.Name + " --> " + Model.Name, 3, 1);
                        model.Name = Model.Name;
                        db.Updateable(model).ExecuteCommand();
                        jsonm.status = 200;
                    }
                }
            }
            catch (Exception ex)
            {
                LogProvider.Error("编辑菜单", ex.StackTrace, ex.Message);

                jsonm.status = 500;
                jsonm.msg = "保存失败";
            }


            return Json(jsonm);
        }
        /// <summary>
        /// 菜单栏目自定义排序
        /// </summary>
        /// <returns></returns>
        public JsonResult ColSort()
        {
            var jsonm = new ResultJson();
            var p = FytRequest.GetFormString("p");
            var i = FytRequest.GetFormString("i");
            var ob = FytRequest.GetFormInt("o");
            Service.Command<SysModuleOutsourcing>((db, o) =>
            {
                o.ModuleSort(db, p, i, ob);
            });
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
                        var list = db.Queryable<SysModule>().Where(m => IDs.Contains(m.ID)).ToList();
                        foreach (var item in list)
                        {
                            names.Add(item.Name);
                        }
                        SetSysLog("【删除菜单】" + string.Join(",", names), 3, 1);
                        db.Deleteable<SysModule>().Where(m => IDs.Contains(m.ID)).ExecuteCommand();
                        jsonm.status = 200;
                    }
                    else
                    {
                        jsonm.status = 500;
                        jsonm.msg = "请选择菜单";
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


        public JsonResult GetModuleList()
        {
            var jsonm = new ResultJson();
            try
            {
                var alist = new List<SysModule>();
                var url = FytRequest.GetViewUrl();
                var model = ModuleList.Where(m => url.Equals(m.Href)).FirstOrDefault();
                if (model != null)
                {
                    var chlidList = ModuleList.Where(m => m.Type == 3 && model.ID == m.ParentID).ToList();
                    var list = SysUserModel.MenuList.Where(m => m.Type == 3 && model.ID == m.ParentID).ToList();


                    foreach (var item in chlidList)
                    {
                        foreach (var item2 in list)
                        {
                            if (item.ID == item2.ID)
                            {
                                item.LAY_CHECKED = true;
                                break;
                            }
                        }
                    }
                    alist = chlidList;
                }
                jsonm.status = 200;
                jsonm.data = alist;
                jsonm.msg = "获取成功";
            }
            catch (Exception ex)
            {
                jsonm.status = 500;
                jsonm.msg = ex.Message;
            }
            return Json(jsonm);

        }
    }
}