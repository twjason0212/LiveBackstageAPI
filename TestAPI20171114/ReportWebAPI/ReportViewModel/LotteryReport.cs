using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportViewModel
{
    public class LotteryReport
    {
        public string LotteryName { get; set; }
        public string LotteryCode { get; set; }
        public int LotteryBetNum { get; set; }
        public decimal LotteryBetMoney { get; set; }
        public decimal LotteryWinMoney { get; set; }
        public decimal LotteryCancelMoney { get; set; }
        public decimal LotteryRebateMoney { get; set; }
        public decimal LotteryProfitMoney { get; set; }
        public string LotteryProfitRate { get; set; }

    
    }
}
