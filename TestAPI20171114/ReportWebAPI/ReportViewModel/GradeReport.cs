using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportViewModel
{
    public class GradeReport
    {
        public string GradeName { get; set; }
        public int GradeBetNum { get; set; }
        public int GradeDepositNum { get; set; }
        public decimal GradeDepositMoney { get; set; }
        public decimal GradeWithdrawMoney { get; set; }
        public decimal GradeBetMoney { get; set; }
        public decimal GradeWinMoney { get; set; }
        public int GradeBetCount { get; set; }
        public decimal GradeActivityMoney { get; set; }
        public decimal GradeRebateMoney { get; set; }
        public decimal GradeProfitMoney { get; set; }
        public string GradeProfitRate { get; set; }
        public decimal GradeAdminRejectMoney { get; set; }
    }
}
