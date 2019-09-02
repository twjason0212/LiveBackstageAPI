using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportModel
{
    public class AgencyHeader
    {
        /// <summary>
        /// 團隊投注金額
        /// </summary>        
        public decimal BetMoney { get; set; }
        /// <summary>
        /// 團隊中獎金額
        /// </summary>
        public decimal Bonus { get; set; }
        /// <summary>
        /// 團隊活動金額
        /// </summary>
        public decimal ActivityMoney { get; set; }
        /// <summary>
        /// 團隊返點金額
        /// </summary>
        public decimal RebateMoney { get; set; }
        /// <summary>
        /// 團隊盈利(未做)
        /// </summary>
        public decimal ProfitMoney { get; set; }
        /// <summary>
        /// 團隊充值金額
        /// </summary>
        public decimal RechargeMoney { get; set; }
        /// <summary>
        /// 團隊提現金額
        /// </summary>
        public decimal WithdrawMoney { get; set; }
        /// <summary>
        /// 團隊投注人數
        /// </summary>
        public int BetNum { get; set; }
        /// <summary>
        /// 團隊首充人数
        /// </summary>
        public int FirstChargeNum { get; set; }
        /// <summary>
        /// 團隊註冊人數
        /// </summary>
        public int RegisterNum { get; set; }
        /// <summary>
        /// 下級人數
        /// </summary>
        public int TeamNum { get; set; }
        /// <summary>
        /// 團隊餘額
        /// </summary>
        public decimal TeamBalance { get; set; }
        /// <summary>
        /// 個人代理返點(未做/存疑?)
        /// </summary>
        public decimal AgentRebate { get; set; }
        /// <summary>
        /// 代理分紅
        /// </summary>
        public string AgentDividends { get; set; }
        /// <summary>
        /// 代理
        /// </summary>
        public string AgentWages { get; set; }
    }
}
