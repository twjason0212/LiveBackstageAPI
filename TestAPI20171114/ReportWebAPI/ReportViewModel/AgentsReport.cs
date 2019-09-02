using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportViewModel
{
    public class AgentsReport
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int RegisterNum { get; set; }
        public int BetNum { get; set; }
        public int NewPayNum { get; set; }
        public decimal BetMoney { get; set; }
        public decimal WinMoney { get; set; }
        public decimal ActivityMoney { get; set; }
        public decimal RebateMoney { get; set; }
        public decimal RechargeMoney { get; set; }
        public decimal WithdrawMoney { get; set; }
        public decimal ProfitMoney { get; set; }
        public decimal AgentRebate { get; set; }
    }
}
