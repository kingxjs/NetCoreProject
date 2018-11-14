using SqlSugar;
using System;
using System.Collections.Generic;

namespace NetCoreObject.Core
{
    /// <summary>
    /// system_user:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    [SugarTable("sys_user")]
    public partial class SysUser
    {
        public SysUser()
        {
            this.SysUserID = CommonModel.GetGUID().ToString();
        }
        #region Model
        private string _sysuserid;
        private string _sysusername;
        private string _sysuserpwd;
        private string _sysnickname;
        /// <summary>
        /// 0停用，1启用，2删除
        /// </summary>
        public int Status { get; set; } = 1;
        public DateTime CreateTime { get; set; } = DateTime.Now;


        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public string SysUserID
        {
            set { _sysuserid = value; }
            get { return _sysuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SysUserName
        {
            set { _sysusername = value; }
            get { return _sysusername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SysUserPwd
        {
            set { _sysuserpwd = value; }
            get { return _sysuserpwd; }
        }

        public string SysNickName
        {
            set { _sysnickname = value; }
            get { return _sysnickname; }
        }
        #endregion Model


        #region 自定义字段
        [SugarColumn(IsIgnore = true)]
        public List<SysRole> RoleList { get; set; }
        
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
        #endregion
    }
}

