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
    public class SysUserController : BaseController
    {
        public SysUserController(IConfiguration config, IHostingEnvironment _hostingEnvironment) : base(config, _hostingEnvironment)
        {
        }

        public IActionResult Index()
        {
            using (var db = SugarBase.GetIntance())
            {
                var role_list = db.Queryable<SysRole>().Where(m => m.Status).ToList();
                ViewBag.RoleList = role_list;
            }
            return View();
        }
        /// <summary>
        /// 添加/编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Change(string id)
        {
            var pid = FytRequest.GetQueryString("pid");
            var model = new SysUser();
            var rel_list = new List<string>();
            using (var db = SugarBase.GetIntance())
            {
                if (!string.IsNullOrEmpty(id))
                {
                    model = db.Queryable<SysUser>().Where(m => m.SysUserID == id).First();
                    var rel_list1 = db.Queryable<SysUserRole>().Where(m => m.UserID == model.SysUserID).ToList();
                    if (rel_list1 != null)
                    {
                        foreach (var item in rel_list1)
                        {
                            rel_list.Add(item.RoleID);
                        }
                    }
                }
                else
                {
                    model.SysUserID = "";
                }
                var role_list = db.Queryable<SysRole>().Where(m => m.Status).ToList();

              

                ViewBag.RoleList = role_list;
                ViewBag.RelRoleList = rel_list;
            }
            return View(model);
        }
        /// <summary>
        /// 个人信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Personal(string id)
        {
            var model = new SysUser();
            var rel_list = new List<string>();
            if (!string.IsNullOrEmpty(id))
            {
                using (var db = SugarBase.GetIntance())
                {
                    model = db.Queryable<SysUser>().Where(m => m.SysUserID == id).First();
                    var rel_list1 = db.Queryable<SysUserRole>().Where(m => m.UserID == model.SysUserID).ToList();
                    if (rel_list1 != null)
                    {
                        foreach (var item in rel_list1)
                        {
                            rel_list.Add(item.RoleID);
                        }
                    }
                }
            }
            else
            {
                return Redirect("/errir/404");
            }
            return View(model);
        }
        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="Name"></param>
        /// <param name="RoleID"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public JsonResult GetList(int limit, int page, string Name, string RoleID, bool Status = true)
        {
            var lstRes = new List<SysUser>();
            int startIndex = (page == 1 || page == 0) ? 0 : limit * (page - 1);
            var total = 0;
            try
            {

                using (var db = SugarBase.GetIntance())
                {
                    var list = db.Queryable<SysUser>()
                    .Where(m => m.Status != 2);
                    if (!string.IsNullOrEmpty(Name))
                    {
                        list.Where(m => m.SysUserName.Contains(Name.Trim()) || m.SysNickName.Contains(Name.Trim()));
                    }
                    //if (!string.IsNullOrEmpty(RoleID))
                    //{
                    //    list.Where()
                    //}
                    lstRes = list.OrderBy(m => m.CreateTime)
                        .ToPageList(page, limit, ref total).ToList();
                }
            }
            catch (Exception ex)
            {
                LogProvider.Error("获取系统用户列表", ex.StackTrace, ex.Message);
            }
            var rows = lstRes;
            return Json(new { code = 0, count = total, data = rows });
        }
        /// <summary>
        /// 添加/编辑  操作
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public JsonResult ChangeSubmit(SysUser Model)
        {
            var jsonm = new ResultJson();
            jsonm.data = Model;
            var model = new SysUser();
            try
            {
                using (var db = SugarBase.GetIntance())
                {
                    if (!string.IsNullOrEmpty(Model.SysUserID))
                    {
                        model = db.Queryable<SysUser>().Where(m => m.SysUserID == Model.SysUserID).First();
                    }
                    model.SysNickName = Model.SysNickName;
                    model.SysUserName = Model.SysUserName;
                    if (!string.IsNullOrEmpty(Model.SysUserPwd))
                    {
                        model.SysUserPwd = SHACryptHelper.SHA256Encrypt(Model.SysUserPwd); ;
                    }


                    if (string.IsNullOrEmpty(Model.SysUserID))
                    {
                        db.Insertable(model).ExecuteCommand();
                        jsonm.status = 200;
                    }
                    else
                    {
                        db.Updateable(model).ExecuteCommand();
                        jsonm.status = 200;
                    }
                    db.Deleteable<SysUserRole>().Where(m => m.UserID == model.SysUserID).ExecuteCommand();
                    if (Model.RoleList != null)
                    {
                        List<SysUserRole> rel_list = new List<SysUserRole>();
                        foreach (var item in Model.RoleList)
                        {
                            if (!string.IsNullOrEmpty(item.RoleID))
                            {
                                var SysUserRoleModel = new SysUserRole();
                                SysUserRoleModel.RoleID = item.RoleID;
                                SysUserRoleModel.UserID = model.SysUserID;
                                rel_list.Add(SysUserRoleModel);
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
                LogProvider.Error("编辑系统用户", ex.StackTrace, ex.Message);

                jsonm.status = 500;
                jsonm.msg = "保存失败";
            }


            return Json(jsonm);
        }
        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public JsonResult PersonalSubmit(SysUser Model)
        {
            var jsonm = new ResultJson();
            jsonm.data = Model;
            var model = new SysUser();
            try
            {
                using (var db = SugarBase.GetIntance())
                {
                    if (!string.IsNullOrEmpty(Model.SysUserID))
                    {
                        model = db.Queryable<SysUser>().Where(m => m.SysUserID == Model.SysUserID).First();
                    }
                    model.SysNickName = Model.SysNickName;


                    db.Updateable(model).ExecuteCommand();
                    jsonm.status = 200;
                }
            }
            catch (Exception ex)
            {
                LogProvider.Error("系统用户编辑个人信息", ex.StackTrace, ex.Message);

                jsonm.status = 500;
                jsonm.msg = "保存失败";
            }


            return Json(jsonm);
        }
        /// <summary>
        /// 改变系统用户状态
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public JsonResult setStatus(string ID, int Status)
        {
            var jsonm = new ResultJson();
            try
            {
                using (var db = SugarBase.GetIntance())
                {
                    if (!string.IsNullOrEmpty(ID))
                    {
                        var model = db.Queryable<SysUser>().Where(m => m.SysUserID == ID).First();
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
                LogProvider.Error("改变系统用户状态", ex.StackTrace, ex.Message);
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
                        db.Updateable<SysUser>().UpdateColumns(m => m.Status == 2).Where(m => IDs.Contains(m.SysUserID)).ExecuteCommand();
                        //DB.Deleteable<SystemUser>().Where(m => IDs.Contains(m.SysUserID)).ExecuteCommand();
                        jsonm.status = 200;
                    }
                    else
                    {
                        jsonm.status = 500;
                        jsonm.msg = "请选择一项或多项";
                    }
                }
            }
            catch (Exception ex)
            {
                jsonm.status = 500;
                jsonm.msg = "删除失败";
                LogProvider.Error("删除系统用户", ex.StackTrace, ex.Message);
            }
            return Json(jsonm);
        }
    }
}