using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportChangeModel
{
    /// <summary>
    /// 分组报表实体类
    /// </summary>
    public class ReportGroup
    {
        /// <summary>
        /// 等级
        /// </summary>
        public string group_name { get; set; }
        /// <summary>
        /// 投注人数
        /// </summary>
        public int BetNum { get; set; }
        /// <summary>
        /// 充值人数
        /// </summary>
        public int PrepaidNum { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal PrepaidMoney { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal OutMoney { get; set; }
        /// <summary>
        /// 投注金额
        /// </summary>
        public decimal BetMoney { get; set; }
        /// <summary>
        /// 中奖金额
        /// </summary>
        public decimal WinMoney { get; set; }
        /// <summary>
        /// 投注单量
        /// </summary>
        public int BetOrderNum { get; set; }
        /// <summary>
        /// 活动礼金
        /// </summary>
        public decimal ActivityDiscountAccountMoney { get; set; }
        /// <summary>
        /// 返点金额
        /// </summary>
        public decimal Out_RebateAccount { get; set; }
        /// <summary>
        /// 行政+拒絕
        /// </summary>
        public decimal GradeAdminRejectMoney { get; set; }
        /// <summary>
        /// 盈利
        /// </summary>
        public decimal ProfitLoss
        {
            get { return (BetMoney - WinMoney - Out_RebateAccount - ActivityDiscountAccountMoney + GradeAdminRejectMoney); }
            set { ProfitLoss = BetMoney - WinMoney - Out_RebateAccount - ActivityDiscountAccountMoney + GradeAdminRejectMoney; }
        }
        /// <summary>
        /// 盈率
        /// </summary>
        public decimal WinRate
        {
            get { return BetMoney > 0 ? (ProfitLoss / BetMoney)*100 : 0; }
            set { WinRate = BetMoney > 0 ? (ProfitLoss / BetMoney)*100 : 0; }
        }
    }
}
