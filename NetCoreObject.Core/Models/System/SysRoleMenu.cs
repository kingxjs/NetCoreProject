using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreObject.Core
{
    [Serializable]
    [SugarTable("sys_role_module")]
    public class SysRoleModule
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public string RoleID { get; set; }

        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public string ModuleID { get; set; }
    }
}
