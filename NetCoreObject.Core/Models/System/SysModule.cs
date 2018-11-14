using SqlSugar;
using System;

namespace NetCoreObject.Core
{
    [Serializable]
    [SugarTable("sys_module")]
    public class SysModule
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public string ID { get; set; } = CommonModel.GetGUID().ToString();
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 菜单地址
        /// </summary>
        public string Href { get; set; }
        /// <summary>
        /// 父级菜单ID
        /// </summary>
        public string ParentID { get; set; } = "0";
        /// <summary>
        /// 类型 1:菜单  2：页面  3：功能
        /// </summary>
        public int Type { get; set; } = 1;
        /// <summary>
        /// 业务权限，Type=2,需填
        /// </summary>
        public string Business { get; set; }
        /// <summary>
        /// 状态：启用/不启用
        /// </summary>
        public bool Status { get; set; } = true;
        /// <summary>
        /// 权重
        /// </summary>
        public int Sort { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        [SugarColumn(IsIgnore = true)]
        public bool LAY_CHECKED { get; set; }


        [SugarColumn(IsIgnore = true)]
        public string Type_toString
        {
            get
            {
                if (Type == 1)
                {
                    return "菜单";
                }
                else if (Type == 2)
                {
                    return "页面";
                }
                else
                {
                    return "功能";
                }
            }
        }
    }
}
