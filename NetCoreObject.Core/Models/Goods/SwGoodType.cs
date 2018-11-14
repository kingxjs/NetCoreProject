using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreObject.Core
{
    /// <summary>
    /// 文章类型
    /// </summary>
    [Serializable]
    [SugarTable("sw_good_type")]
    public class SwGoodType
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public string ID { get; set; } = CommonModel.GetGUID().ToString();
        /// <summary>
        /// 类型名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 副标题
        /// </summary>
        public string SubTitle { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 权重
        /// </summary>
        public int Sort { get; set; }

    }
}
