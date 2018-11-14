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

namespace NetCoreObject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GoodsController : BaseController
    {
        public GoodsController(IConfiguration config, IHostingEnvironment _hostingEnvironment) : base(config, _hostingEnvironment)
        {
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Change(string id)
        {
            var model = new SwGoods();

            Service.Command<SwGoods>((db, o) =>
            {
                if (!string.IsNullOrEmpty(id))
                {
                    model = db.Queryable<SwGoods>().Where(m => m.ID == id).First();
                }
                else
                {
                    model.ID = "";
                }
                var tList = db.Queryable<SwGoodType>().ToList();
                ViewBag.TypeList = tList;
            });


            return View(model);
        }
        public JsonResult GetList(int limit, int page, string Name)
        {
            var jsonm = new ResultJson();
            var lstRes = new List<SwGoods>();
            var total = 0;
            try
            {
                using (var db = SugarBase.GetIntance())
                {
                    if (!string.IsNullOrEmpty(Name))
                    {
                        lstRes = db.Queryable<SwGoods>()
                        .Where(m =>m.Status!=4 && m.Title.Contains(Name.Trim()))
                        .OrderBy(m => m.TopSort, OrderByType.Desc)
                        .OrderBy(m => m.Status)
                        .OrderBy(m => m.Sort, OrderByType.Desc)
                        .ToPageList(page, limit, ref total).ToList();
                    }
                    else
                    {
                        lstRes = db.Queryable<SwGoods>()
                        .Where(m => m.Status != 4 )
                        .OrderBy(m => m.TopSort, OrderByType.Desc)
                        .OrderBy(m => m.Status)
                        .OrderBy(m => m.Sort, OrderByType.Desc)
                        .ToPageList(page, limit, ref total).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogProvider.Error("获取文章类型", ex);
            }
            jsonm.code = 0;
            jsonm.data = lstRes;
            jsonm.count = total;
            return Json(jsonm);
        }

        public JsonResult ChangeSubmit(SwGoods Model)
        {
            var jsonm = new ResultJson();
            jsonm.data = Model;
            var model = new SwGoods();
            try
            {
                using (var db = SugarBase.GetIntance())
                {
                    if (!string.IsNullOrEmpty(Model.ID))
                    {
                        model = db.Queryable<SwGoods>().Where(m => m.ID == Model.ID).First();
                    }
                    if (model == null)
                    {
                        model = new SwGoods();
                    }
                    model.Title = Model.Title;
                    model.Content = Model.Content;

                    model.GoodsKeys = Model.GoodsKeys;
                    model.Sort = Model.Sort;
                    model.IsHot = Model.IsHot;
                    if (Model.IsTop)
                    {
                        if (!model.IsTop)
                        {
                            var TopSortModel = db.Queryable<SwGoods>().Where(m => m.ID != model.ID).Select(m => new { TopSort = SqlFunc.AggregateMax(m.TopSort) }).First();
                            model.TopSort = TopSortModel.TopSort + 1;
                        }
                    }
                    else
                    {
                        model.TopSort = 0;
                    }
                    model.IsTop = Model.IsTop;

                    model.Status = Model.Status;
                    model.TypeID = Model.TypeID;
                    if (!string.IsNullOrEmpty(model.TypeID))
                    {
                        var typeModel = db.Queryable<SwGoodType>().Where(m => m.ID == model.TypeID).First();
                        if (typeModel != null)
                        {
                            model.TypeName = typeModel.Title;
                        }
                    }
                    if (string.IsNullOrEmpty(Model.ID))
                    {
                        SetSysLog("【添加文章】" + Model.Title, 3, 1);
                        db.Insertable(model).ExecuteCommand();
                        jsonm.status = 200;
                    }
                    else
                    {
                        SetSysLog("【编辑文章】" + model.Title, 3, 1);
                        db.Updateable(model).ExecuteCommand();
                        jsonm.status = 200;
                    }

                }
            }
            catch (Exception ex)
            {
                LogProvider.Error("编辑文章", ex);

                jsonm.status = 500;
                jsonm.msg = "保存失败";
            }


            return Json(jsonm);
        }
        public JsonResult setStatus(string ID, string Name, bool Value)
        {
            var jsonm = new ResultJson();
            try
            {
                using (var db = SugarBase.GetIntance())
                {
                    if (!string.IsNullOrEmpty(ID))
                    {
                        var model = db.Queryable<SwGoods>().Where(m => m.ID == ID).First();
                        if (model != null)
                        {
                            if (Name == "IsHot")
                            {
                                model.IsHot = Value;
                            }
                            if (Name == "IsTop")
                            {
                                if (Value)
                                {
                                    if (!model.IsTop)
                                    {
                                        var TopSortModel = db.Queryable<SwGoods>().Where(m => m.ID != model.ID).Select(m => new { TopSort = SqlFunc.AggregateMax(m.TopSort) }).First();
                                        model.TopSort = TopSortModel.TopSort + 1;
                                    }
                                }
                                else
                                {
                                    model.TopSort = 0;
                                }

                                model.IsTop = Value;
                            }
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
                LogProvider.Error("改变文章属性", ex.StackTrace, ex.Message);
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
                        var list = db.Queryable<SwGoods>().Where(m => IDs.Contains(m.ID)).ToList();
                        foreach (var item in list)
                        {
                            names.Add(item.Title);
                        }
                        SetSysLog("【删除文章】" + string.Join(",", names), 3, 1);
                        db.Updateable<SwGoods>().UpdateColumns(m => new SwGoods { Status = 4 }).Where(m => IDs.Contains(m.ID)).ExecuteCommand();
                        //db.Deleteable<SwGoods>().Where(m => IDs.Contains(m.ID)).ExecuteCommand();
                        jsonm.status = 200;
                    }
                    else
                    {
                        jsonm.status = 500;
                        jsonm.msg = "请选择文章";
                    }
                }
            }
            catch (Exception ex)
            {
                jsonm.status = 500;
                jsonm.msg = "删除失败";
                LogProvider.Error("删除文章", ex);
            }
            return Json(jsonm);
        }
    }
}