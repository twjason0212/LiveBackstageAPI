using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportChangeModel
{
    /// <summary>
    /// 会员盈利实体类
    /// </summary>
    public class ReportMember
    {
        /// <summary>
        /// 会员Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 上级代理Id
        /// </summary>
        public string AgentId { get; set; }
        /// <summary>
        /// 会员账号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 会员等级
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 直接上级
        /// </summary>
        public string AgentName { get; set; }
        /// <summary>
        /// 会员余额
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// 入款总额
        /// </summary>
        public decimal InMoney { get; set; }
        /// <summary>
        /// 出款总额
        /// </summary>
        public decimal OutMoney { get; set; }
        /// <summary>
        /// 投注总额
        /// </summary>
        public decimal BetMoney { get; set; }
        /// <summary>
        /// 中奖总额
        /// </summary>
        public decimal WinMoney { get; set; }
        /// <summary>
        /// 返点总额
        /// </summary>
        public decimal RebateMoney { get; set; }
        /// <summary>
        /// 优惠总额
        /// </summary>
        public decimal DiscountMoney { get; set; }
        /// <summary>
        /// 打賞金額
        /// </summary>
        public decimal RewardMoney { get; set; }
        /// <summary>
        /// 盈利总额
        /// </summary>
        public decimal ProfitLoss { get; set; }
        /// <summary>
        /// 盈率
        /// </summary>
        public decimal ProfitRate { get; set; }
    }
}
