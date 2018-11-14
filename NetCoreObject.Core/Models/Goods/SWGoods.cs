using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreObject.Core
{
    /// <summary>
    /// 文章
    /// </summary>
    [Serializable]
    [SugarTable("sw_goods")]
    public class SwGoods
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public string ID { get; set; } = CommonModel.GetGUID().ToString();
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 类型ID
        /// </summary>
        public string TypeID { get; set; }
        /// <summary>
        /// 关键字，便于搜索
        /// </summary>
        public string GoodsKeys { get; set; }
        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop { get; set; } = false;
        /// <summary>
        /// 是否热门
        /// </summary>
        public bool IsHot { get; set; } = false;
        /// <summary>
        /// 权重
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 点击量
        /// </summary>
        public int Hits { get; set; }
        /// <summary>
        /// 置顶权重
        /// </summary>
        public int TopSort { get; set; }
        /// <summary>
        /// 状态  1:未发布，2：已发布，3：已下架，4：已删除
        /// </summary>
        public int Status { get; set; } = 1;
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
