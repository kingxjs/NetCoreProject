using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreObject.Core
{
    [Serializable]
    [SugarTable("sys_user_role")]
    public class SysUserRole
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public string RoleID { get; set; }

        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public string UserID { get; set; }
    }
}
