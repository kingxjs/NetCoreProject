using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreObject.Core
{
    [Serializable]
    [SugarTable("sys_message")]
    public class SysMessage
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public string ID { get; set; }
        /// <summary>
        /// 发送者消息ID
        /// </summary>
        public string SendMID { get; set; }
        /// <summary>
        /// 发送人
        /// </summary>
        public string SenderID { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public string RecipientID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 如果回复，要回复的消息ID
        /// </summary>
        public string ParentID { get; set; }
        /// <summary>
        /// 如果回复，则为第一条的ID
        /// </summary>
        public string FirstID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 消息类型，1：通知，2：私信，3：我发送的通知，4：我发送的私信
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 状态, 0：未读，1：已读，2：删除
        /// </summary>
        public int Status { get; set; } = 0;


        #region 自定义字段
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
        [SugarColumn(IsIgnore = true)]
        public string Status_toString
        {
            get
            {
                if (Status == 1)
                {
                    return "已读";
                }
                else
                {
                    return "未读";
                }
            }
        }
        #endregion
    }
}
