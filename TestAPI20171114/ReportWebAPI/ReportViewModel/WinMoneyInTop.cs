using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportViewModel
{
    /// <summary>
    /// 中奖前十
    /// </summary>
    public class WinMoneyInTop
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 站长ID
        /// </summary>
        public string IdentityId { get; set; }
        /// <summary>
        /// 投注总额
        /// </summary>
        public decimal BetMoney { get; set; }
        /// <summary>
        /// 中奖总额
        /// </summary>
        public decimal WinMoney { get; set; }
        /// <summary>
        /// 盈利
        /// </summary>
        public decimal ProfitLoss { get; set; }
    }
}
