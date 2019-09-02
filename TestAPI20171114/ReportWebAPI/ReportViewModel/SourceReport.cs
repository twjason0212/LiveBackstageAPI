using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportViewModel
{
    public class SourceReport
    {
        public string SourceName { get; set; }
        public int SourceBetNum { get; set; }
        public int SourceRegisterNum { get; set; }
        public int SourceNewPayNum { get; set; }
        public int SourceDepositNum { get; set; }
        public decimal SourceDepositMoney { get; set; }
        public decimal SourceWithdrawMoney { get; set; }
        public decimal SourceBetMoney { get; set; }
        public decimal SourceWinMoney { get; set; }
        public int SourceBetCount { get; set; }
        public decimal SourceActivityMoney { get; set; }
        public decimal SourceRebateMoney { get; set; }
        public decimal SourceAdminRejectMoney { get; set; }
        public decimal SourceProfitMoney { get; set; }
        public string SourceProfitRate { get; set; }
    }
}
