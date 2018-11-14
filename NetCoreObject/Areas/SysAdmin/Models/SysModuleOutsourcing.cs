using NetCoreObject.Core;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreObject
{
    public class SysModuleOutsourcing
    {
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="db"></param>
        /// <param name="p"></param>
        /// <param name="i"></param>
        /// <param name="o"></param>
        public void ModuleSort(SqlSugarClient db, string p, string i, int o)
        {
            int a = 0, b = 0, c = 0;
            var list = db.Queryable<SysModule>().Where(m => m.ParentID == p).OrderBy(m => m.Sort).ToList();
            if (list.Count > 0)
            {
                var index = 0;
                foreach (var item in list)
                {
                    index++;
                    if (index == 1)
                    {
                        if (item.ID == i) //判断是否是头如果上升则不做处理
                        {
                            if (o == 1) //下降一位
                            {
                                a = Convert.ToInt32(item.Sort);
                                b = Convert.ToInt32(list[index].Sort);
                                c = a;
                                a = b;
                                b = c;
                                item.Sort = a;
                                db.Updateable(item).ExecuteCommand();
                                var nitem = list[index];
                                nitem.Sort = b;
                                db.Updateable(nitem).ExecuteCommand();
                                break;
                            }
                        }
                    }
                    else if (index == list.Count)
                    {
                        if (item.ID == i) //最后一条如果下降则不做处理
                        {
                            if (o == 0) //上升一位
                            {
                                a = Convert.ToInt32(item.Sort);
                                b = Convert.ToInt32(list[index - 2].Sort);
                                c = a;
                                a = b;
                                b = c;
                                item.Sort = a;
                                db.Updateable(item).ExecuteCommand();
                                var nitem = list[index - 2];
                                nitem.Sort = b;
                                db.Updateable(nitem).ExecuteCommand();
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (item.ID == i) //判断是否是头如果上升则不做处理
                        {
                            if (o == 1) //下降一位
                            {
                                a = Convert.ToInt32(item.Sort);
                                b = Convert.ToInt32(list[index].Sort);
                                c = a;
                                a = b;
                                b = c;
                                item.Sort = a;
                                db.Updateable(item).ExecuteCommand();
                                var nitem = list[index];
                                nitem.Sort = b;
                                db.Updateable(nitem).ExecuteCommand();
                                break;
                            }
                            else
                            {
                                a = Convert.ToInt32(item.Sort);
                                b = Convert.ToInt32(list[index - 2].Sort);
                                c = a;
                                a = b;
                                b = c;
                                item.Sort = a;
                                db.Updateable(item).ExecuteCommand();
                                var nitem = list[index - 2];
                                nitem.Sort = b;
                                db.Updateable(nitem).ExecuteCommand();
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
