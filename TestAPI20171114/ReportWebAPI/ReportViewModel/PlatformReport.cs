using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportViewModel
{
    /// <summary>
    /// 月度報表
    /// </summary>
    public class PlatformReport
    {
        public string DateRange { get; set; }
        public string IdentityId { get; set; }
        public string IdentityName { get; set; }
        public int RegisterNumber { get; set; }
        public int NewPayNumber { get; set; }
        public int BetNumber { get; set; }
        public int InNumber { get; set; }
        public int OutNumber { get; set; }
        public int BetCount { get; set; }
        public decimal BetMoney { get; set; }
        public decimal WinMoney { get; set; }
        public decimal InMoney { get; set; }
        public int InCount { get; set; }
        public decimal OutMoney { get; set; }
        public int OutCount { get; set; }
        public decimal ActivityMoney { get; set; }
        public decimal RewardMoney { get; set; }
        public int RewardNumber { get; set; }
        public decimal Gross { get { return BetMoney - WinMoney; } }
        public string ProfitRate { get { return (BetMoney > 0 ? ((BetMoney - WinMoney - ActivityMoney - RebateMoney + AdminMoney + RejectMoney) / BetMoney * 100) : 0).ToString("0.00") + "%"; } }
        //↓計算盈率用欄位↓
        public decimal RebateMoney { get; set; }
        public decimal AdminMoney { get; set; }
        public decimal RejectMoney { get; set; }
    }
}
