using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportChangeModel
{
    /// <summary>
    ///代理报表
    /// </summary>
    public class ReportAgent
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 代理名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 团队人数
        /// </summary>
        public decimal TeamNum { get; set; }
        /// <summary>
        /// 注册人数
        /// </summary>
        public decimal RegNum { get; set; }
        /// <summary>
        /// 团队余额
        /// </summary>
        public decimal TeamMoney { get; set; }
        /// <summary>
        /// 投注人数
        /// </summary>
        public int TotalBettingNum { get; set; }
        /// <summary>
        /// 首充人数
        /// </summary>
        public int NewPrepaidNum { get; set; }
        /// <summary>
        ///自身返点
        /// </summary>
        public decimal RebateAccount { get; set; }
        /// <summary>
        /// 投注总额
        /// </summary>
        public decimal TotalBettingAccount { get; set; }
        /// <summary>
        /// 中奖总额
        /// </summary>
        public decimal TotalWinningAccount { get; set; }
        /// <summary>
        /// 活动礼金
        /// </summary>
        public decimal TotalActivityMoney { get; set; }
        /// <summary>
        /// 团队返点
        /// </summary>
        public decimal TotalRebateAccount { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal TotalInMoney { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal TotalOutMoney { get; set; }
        /// <summary>
        ///优惠总额
        /// </summary>
        public decimal TotalDiscountAccount { get; set; }
        /// <summary>
        ///盈利
        /// </summary>
        public decimal ProfitLoss { get; set; }
        /// <summary>
        /// 工资
        /// </summary>
        public decimal TotalWages { get; set; }
        /// <summary>
        /// 分红
        /// </summary>
        public decimal TotalBonus { get; set; }
    }
}
