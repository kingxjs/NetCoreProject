using SqlSugar;
using System;

namespace NetCoreObject.Core
{
    [Serializable]
    [SugarTable("sys_task")]
    public class SysTask
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public string TaskID { get; set; } = CommonModel.GetGUID().ToString();
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 任务周期
        /// </summary>
        public int Cycle { get; set; } = 1;
        /// <summary>
        /// 如果选择每周，应选择周几
        /// </summary>
        public int Day { get; set; } = 7;
        /// <summary>
        /// 任务时间Time
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 根据周期，第几天，时间，计算出本次时间
        /// </summary>
        public DateTime ThisTime { get; set; }
        /// <summary>
        /// 下一次运行时间
        /// </summary>
        public DateTime NextTime { get; set; }
        /// <summary>
        /// 任务进行次数
        /// </summary>
        public int Number { get; set; } = 0;

        public DateTime CreateTime { get; set; }


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
        public string ThisTime_toString
        {
            get
            {
                if (!string.IsNullOrEmpty(ThisTime.ToString()))
                {
                    return ThisTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    return "";
                }
            }
        }

        [SugarColumn(IsIgnore = true)]
        public string NextTime_toString
        {
            get
            {
                if (!string.IsNullOrEmpty(NextTime.ToString()))
                {
                    return NextTime.ToString("yyyy-MM-dd HH:mm:ss");
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
