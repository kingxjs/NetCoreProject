using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreObject.Core
{
    /// <summary>
    /// 系统角色
    /// </summary>
    [Serializable]
    [SugarTable("sys_role")]
    public class SysRole
    {
        [SugarColumn(IsIgnore = true)]
        public List<SysModule> ModuleList { get; set; }

        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public string RoleID { get; set; } = CommonModel.GetGUID().ToString();
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; } = true;
        public DateTime CreateTime { get; set; } = DateTime.Now;
        [SugarColumn(IsIgnore = true)]
        public string CreateTime_toString
        {
            get
            {
                if (!string.IsNullOrEmpty(CreateTime.ToString()))
                {
                    return CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
