using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreObject.Core;
using System.IO;
using NetCoreObject.Common;
using SqlSugar;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace NetCoreObject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GuestUserController : BaseController
    {
        public GuestUserController(IConfiguration config, IHostingEnvironment _hostingEnvironment) : base(config, _hostingEnvironment)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DelIndex()
        {
            return View();
        }


        public IActionResult Change(string id)
        {
            UserInfo model = new UserInfo();

            using (var db = SugarBase.GetIntance())
            {
                if (!string.IsNullOrEmpty(id))
                {
                    model = db.Queryable<UserInfo>().Where(m => m.UserID == id).First();
                }
                else
                {
                    model.UserID = "";
                }
            }
            return View(model);
        }
        public IActionResult BatchAdd()
        {
            return View();
        }
        public JsonResult GetList(int limit, int page, string Name, bool Status = true)
        {
            var lstRes = new List<UserInfo>();
            int startIndex = (page == 1 || page == 0) ? 0 : limit * (page - 1);
            var total = 0;
            try
            {
                using (var db = SugarBase.GetIntance())
                {
                    var list = db.Queryable<UserInfo>()
                        .Where(m => m.Status == Status);
                    if (!string.IsNullOrEmpty(Name))
                    {
                        list.Where(m => m.FullName.Contains(Name.Trim()) || m.Email.Contains(Name.Trim()) || m.Mobile.Contains(Name.Trim()) || m.UserAccount.Contains(Name.Trim()));
                    }
                    lstRes = list.OrderBy(m => m.CreateTime, OrderByType.Desc)
                        .ToPageList(page, limit, ref total).ToList();
                }
            }
            catch (Exception ex)
            {
                LogProvider.Error("获取用户列表", ex.StackTrace, ex.Message);
            }
            var rows = lstRes;
            return Json(new { code = 0, count = total, data = rows });
        }

        public JsonResult GetData(string UserID)
        {
            var jsonm = new ResultJson();
            try
            {
                var model = new UserInfo();
                using (var db = SugarBase.GetIntance())
                {
                    if (!string.IsNullOrEmpty(UserID))
                    {
                        model = db.Queryable<UserInfo>().Where(m => m.UserID == UserID).First();
                        if (model != null)
                        {
                            jsonm.status = 200;
                            jsonm.data = model;
                        }
                        else
                        {
                            jsonm.status = 500;
                            jsonm.msg = "没有找到用户";
                        }
                    }
                    else
                    {
                        jsonm.status = 500;
                        jsonm.msg = "传值不正确";
                    }
                }
            }
            catch (Exception ex)
            {

                jsonm.status = 500;
                jsonm.msg = ex.Message;
            }
            return Json(jsonm);
        }

        #region api
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="NickName"></param>
        /// <param name="FullName"></param>
        /// <param name="UserAccount"></param>
        /// <param name="UserPassword"></param>
        /// <param name="Email"></param>
        /// <param name="Mobile"></param>
        /// <param name="Remark"></param>
        /// <param name="Status"></param>
        /// <param name="State"></param>
        /// <param name="Gender"></param>
        /// <returns></returns>
        public JsonResult ChangeSubmit(UserInfo Model)
        {
            var jsonm = new ResultJson();
            UserInfo umodel = new UserInfo();
            UserInfo rmodel = new UserInfo();
            try
            {
                using (var db = SugarBase.GetIntance())
                {
                    if (!string.IsNullOrEmpty(Model.UserID))
                    {
                        umodel = db.Queryable<UserInfo>().Where(m => m.UserID == Model.UserID).First();
                    }
                    if (umodel == null)
                    {
                        umodel = new UserInfo();
                    }

                    if (!string.IsNullOrEmpty(umodel.UserAccount))
                    {
                        if (string.IsNullOrEmpty(Model.UserAccount))
                        {
                            Model.UserAccount = umodel.UserAccount;
                        }

                        if (umodel.UserAccount != Model.UserAccount)
                        {
                            Model.UserAccount = umodel.UserAccount;
                        }
                    }
                    var UserAccount2 = "";
                    if (string.IsNullOrEmpty(Model.UserAccount))
                    {
                        UserAccount2 = GetUserAccount(9, true, false, false);
                        Model.UserAccount = "zkey" + UserAccount2;
                    }

                    umodel.UserAccount = Model.UserAccount;
                    rmodel.UserAccount = Model.UserAccount;


                    if (!string.IsNullOrEmpty(Model.UserPassword))
                    {
                        rmodel.UserPassword = SHACryptHelper.SHA256Encrypt(Model.UserPassword);
                    }
                    else if (string.IsNullOrEmpty(rmodel.UserID))
                    {
                        jsonm.status = 500;
                        jsonm.msg = "新添加用户，密码不能为空";
                        return Json(jsonm);
                    }


                    umodel.FullName = Model.FullName;
                    umodel.Email = Model.Email;
                    umodel.Mobile = Model.Mobile;
                    umodel.Gender = Model.Gender;
                    umodel.Remark = Model.Remark;
                    umodel.Status = Model.Status;


                    //每个站点对应的用户信息

                    var userCount = 0;
                    if (string.IsNullOrEmpty(Model.UserID))
                    {
                        userCount = db.Queryable<UserInfo>().Where(m => m.Mobile == rmodel.Mobile || m.Email == rmodel.Email).Count();

                        if (userCount <= 0)
                        {
                            umodel.CreateTime = DateTime.Now;

                            db.Insertable(umodel).ExecuteCommand();

                            jsonm.status = 200;
                        }
                        else
                        {
                            jsonm.status = 500;
                            jsonm.msg = "手机号或邮箱已存在";
                        }
                    }
                    else
                    {
                        userCount = db.Queryable<UserInfo>().Where(m => (m.Mobile == rmodel.Mobile || m.Email == rmodel.Email) && m.UserID == rmodel.UserID).Count();
                        if (userCount > 1)
                        {
                            jsonm.status = 500;
                            jsonm.msg = "用户已存在";
                        }
                        else
                        {

                            db.Updateable(umodel).ExecuteCommand();

                            jsonm.status = 200;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogProvider.Error("编辑用户", ex.StackTrace, ex.Message);

                jsonm.status = 500;
                jsonm.msg = ex.Message;
            }
            return Json(jsonm);
        }
        public JsonResult BatchAddSubmit(List<UserInfo> list)
        {
            var jsonm = new ResultJson();
            try
            {
                if (list.Count > 0)
                {
                    var userList = new List<UserInfo>();
                    foreach (var item in list)
                    {
                        if (!string.IsNullOrEmpty(item.UserPassword))
                        {
                            item.UserPassword = SHACryptHelper.SHA256Encrypt(item.UserPassword);
                        }

                        var umodel = new UserInfo();
                        umodel.UserAccount = item.UserAccount;
                        umodel.Mobile = item.Mobile;
                        umodel.Email = item.Email;
                        umodel.CreateTime = DateTime.Now;

                        item.UserID = umodel.UserID;
                        item.CreateTime = umodel.CreateTime;

                        userList.Add(umodel);
                    }



                    using (var db = SugarBase.GetIntance())
                    {
                        db.Insertable(userList).ExecuteCommand();
                    }
                }
            }
            catch (Exception ex)
            {
                jsonm.status = 500;
                jsonm.msg = ex.Message;
                LogProvider.Error("批量添加用户", ex.StackTrace, ex.Message);
            }
            jsonm.data = list;
            return Json(jsonm);
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="UserPassword"></param>
        /// <returns></returns>
        public JsonResult ChangePassSubmit(string QuotaID, string UserID, string UserPassword)
        {
            var jsonm = new ResultJson();
            UserInfo model = new UserInfo();
            try
            {
                using (var db = SugarBase.GetIntance())
                {
                    if (!string.IsNullOrEmpty(UserID))
                    {
                        model = db.Queryable<UserInfo>().Where(m => m.UserID == UserID).First();
                        if (model != null)
                        {
                            if (model.UserID == UserID)
                            {
                                if (!string.IsNullOrEmpty(UserPassword))
                                {
                                    model.UserPassword = SHACryptHelper.SHA256Encrypt(UserPassword);
                                    db.Updateable(model).ExecuteCommand();
                                    jsonm.status = 200;

                                }
                                else
                                {
                                    jsonm.status = 300;
                                    jsonm.msg = "请填写密码";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogProvider.Error("修改密码", ex.StackTrace, ex.Message);

                jsonm.status = 500;
                jsonm.msg = "保存失败";
            }
            return Json(jsonm);
        }

        public JsonResult DeleteSubmit(string ids)
        {
            var jsonm = new ResultJson();
            List<string> UserIDs = new List<string>();
            try
            {
                if (!string.IsNullOrEmpty(ids))
                {
                    string[] ss = ids.Split(',');
                    UserIDs.AddRange(ss);
                }
                jsonm.data = UserIDs;
                if (UserIDs.Count > 0)
                {
                    using (var db = SugarBase.GetIntance())
                    {
                        db.Updateable<UserInfo>().UpdateColumns(m => m.Status == false).Where(m => UserIDs.Contains(m.UserID)).ExecuteCommand();
                    }
                    //DB.Deleteable<UserInfo>().Where(m => UserIDs.Contains(m.UserID)).ExecuteCommand();
                    jsonm.status = 200;
                }
                else
                {
                    jsonm.status = 500;
                    jsonm.msg = "请选择用户";
                }
            }
            catch (Exception ex)
            {
                jsonm.status = 500;
                jsonm.msg = "删除失败";
                LogProvider.Error("删除用户", ex.StackTrace, ex.Message);
            }
            return Json(jsonm);
        }
        public JsonResult RecoverySubmit(string ids)
        {
            var jsonm = new ResultJson();
            List<string> UserIDs = new List<string>();
            try
            {
                if (!string.IsNullOrEmpty(ids))
                {
                    string[] ss = ids.Split(',');
                    UserIDs.AddRange(ss);
                }
                jsonm.data = UserIDs;
                if (UserIDs.Count > 0)
                {
                    using (var db = SugarBase.GetIntance())
                    {
                        db.Updateable<UserInfo>().UpdateColumns(m => m.Status == true).Where(m => UserIDs.Contains(m.UserID)).ExecuteCommand();
                    }
                    jsonm.status = 200;
                }
                else
                {
                    jsonm.status = 500;
                    jsonm.msg = "请选择用户";
                }
            }
            catch (Exception ex)
            {
                jsonm.status = 500;
                jsonm.msg = "恢复失败";
                LogProvider.Error("恢复用户", ex.StackTrace, ex.Message);
            }
            return Json(jsonm);
        }
        /// <summary>
        /// 彻底删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult ThoroughDeleteSubmit(string ids)
        {
            var jsonm = new ResultJson();
            List<string> UserIDs = new List<string>();
            try
            {
                if (!string.IsNullOrEmpty(ids))
                {
                    string[] ss = ids.Split(',');
                    UserIDs.AddRange(ss);
                }
                jsonm.data = UserIDs;
                if (UserIDs.Count > 0)
                {
                    //DB.Updateable<UserInfo>().UpdateColumns(m => m.Status == 1).Where(m => UserIDs.Contains(m.UserID)).ExecuteCommand();
                    using (var db = SugarBase.GetIntance())
                    {
                        db.Deleteable<UserInfo>().Where(m => UserIDs.Contains(m.UserID)).ExecuteCommand();
                    }
                    jsonm.status = 200;
                }
                else
                {
                    jsonm.status = 500;
                    jsonm.msg = "请选择用户";
                }
            }
            catch (Exception ex)
            {
                jsonm.status = 500;
                jsonm.msg = "彻底删除失败";
                LogProvider.Error("彻底删除用户", ex.StackTrace, ex.Message);
            }
            return Json(jsonm);
        }

        #endregion
    }
}