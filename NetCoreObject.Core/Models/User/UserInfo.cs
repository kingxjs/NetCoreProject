using SqlSugar;
using System;
namespace NetCoreObject.Core
{
    /// <summary>
    /// user_info:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    [SugarTable("user_info")]
    public partial class UserInfo
    {
        public UserInfo()
        {

            this.UserID = CommonModel.GetGUID().ToString();
            this.CreateTime = DateTime.Now;
            this.WriteTime = DateTime.Now;
        }
        #region Model
        private string _userid;
        private string _useraccount;
        private string _mobile;
        private string _email;
        private DateTime _createtime;
        private DateTime _writetime;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 账号
        /// </summary>
        public string UserAccount
        {
            set { _useraccount = value; }
            get { return _useraccount; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile
        {
            set { _mobile = value; }
            get
            {
                if (!string.IsNullOrEmpty(_mobile))
                {
                    return _mobile;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// Email
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get
            {
                if (!string.IsNullOrEmpty(_email))
                {
                    return _email;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string UserPassword { get; set; }
        public string Remark { get; set; }
        public int Gender { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime
        {
            set
            { _createtime = value; }
            get { return _createtime; }
        }
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
        /// <summary>
        /// 操作日期
        /// </summary>
        public DateTime WriteTime
        {
            set
            { _writetime = value; }
            get { return _writetime; }
        }
        [SugarColumn(IsIgnore = true)]
        public string WriteTime_toString
        {
            get
            {
                if (!string.IsNullOrEmpty(WriteTime.ToString()))
                {
                    return WriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion Model

    }
}

