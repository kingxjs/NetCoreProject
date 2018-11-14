using NetCoreObject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreObject
{
    public class Outsourcing
    {
        public List<object> GetSysModuleJson(List<SysModule> menuList, string ParentID)
        {
            var children = new List<object>();
            if (menuList.Count > 0)
            {
                foreach (var item in menuList.Where(m => m.ParentID == ParentID))
                {
                    var childrens = GetSysModuleJson(menuList, item.ID);
                    if (childrens.Count > 0)
                    {
                        children.Add(new
                        {
                            name = item.Name,
                            id = item.ID,
                            children = childrens,
                            open = false,
                            @checked = false
                        });
                    }
                    else
                    {
                        children.Add(new
                        {
                            name = item.Name,
                            id = item.ID,
                            open = false,
                            @checked = false
                        });
                    }
                }
            }
            return children;
        }
    }
}
