using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportModel
{
    /// <summary>
    /// 進出款耗時
    /// </summary>
    public class DealTime
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        public string IdentityId { get; set; }
        /// <summary>
        /// 提现时间
        /// </summary>
        public int WithdrawTime { get; set; }
        /// <summary>
        /// 充值时间
        /// </summary>
        public int RechargeTime { get; set; }
    }
}
