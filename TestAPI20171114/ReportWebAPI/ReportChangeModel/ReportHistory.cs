using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportChangeModel
{
    public class ReportHistory
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public int User_id { get; set; }
        /// <summary>
        /// 会员账号
        /// </summary>
        [Description("会员账号")]
        public string User_name { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        [Description("等级")]
        public string groups { get; set; }
        /// <summary>
        /// 充值次数
        /// </summary>
        [Description("充值次数")]
        public int ruknum { get; set; }
        /// <summary>
        /// 提现次数
        /// </summary>
        [Description("提现次数")]
        public int chuknum { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        [Description("充值金额")]
        public decimal totalruk { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        [Description("提现金额")]
        public decimal totalchuk { get; set; }
        /// <summary>
        /// 投注金额
        /// </summary>
        [Description("投注金额")]
        public decimal totalplay { get; set; }
        /// <summary>
        /// 中奖金额
        /// </summary>
        [Description("中奖金额")]
        public decimal totalwin { get; set; }
        /// <summary>
        /// 返点金额
        /// </summary>
        [Description("返点金额")]
        public decimal totalfd { get; set; }
        /// <summary>
        /// 活动礼金
        /// </summary>
        [Description("活动礼金")]
        public decimal totalyh { get; set; }
        /// <summary>
        /// 打賞金額
        /// </summary>
        [Description("打賞金額")]
        public decimal TotalReward { get; set; }
        /// <summary>
        /// 盈利
        /// </summary>
        [Description("盈利")]
        public decimal Deficit { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        [Description("注册时间")]
        public DateTime zctime { get; set; }

    }
}
