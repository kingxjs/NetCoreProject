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
                    children.Add(new
                    {
                        name = item.Name,
                        id = item.ID,
                        children = GetSysModuleJson(menuList, item.ID)
                    });
                }
            }
            return children;
        }
    }
}
