using SqlSugar;
using System;

namespace NetCoreObject.Core
{
    [Serializable]
    [SugarTable("sys_log")]
    public class SysLog
    {
        /// <summary>
        /// Desc:自动递增
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public string ID { get; set; } = CommonModel.GetGUID().ToString();

        /// <summary>
        /// Desc:操作人
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string UserName { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Desc:日志内容
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Title { get; set; }
        /// <summary>
        /// Desc:IP地址
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string IP { get; set; }
        /// <summary>
        /// 日志类型
        /// 1:用户登录日志
        /// 2:内容操作日志
        /// 3:菜单操作日志
        /// 4:模板管理日志
        /// 5:数据备份日志
        /// 6:组件操作日志
        /// </summary>
        public int LogType { get; set; }

        /// <summary>
        /// Desc:日志级别
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string LogGrade { get; set; }

        /// <summary>
        /// Desc:请求地址
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string RequestUrl { get; set; }

        /// <summary>
        /// Desc:添加时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? AddDate { get; set; }
    }
}