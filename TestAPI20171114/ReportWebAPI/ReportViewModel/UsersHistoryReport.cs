using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportViewModel
{
    public class UsersHistoryReport
    {
        //public int UserId { get; set; }
        public string UserName { get; set; }
        public short GroupId { get; set; }
        public int InCount { get; set; }
        public int OutCount { get; set; }
        public decimal InMoney { get; set; }
        public decimal OutMoney { get; set; }
        public decimal BetMoney { get; set; }
        public decimal WinMoney { get; set; }
        public decimal RewardMoney { get; set; }
        public decimal RebateMoney { get; set; }
        public decimal ActivityMoney { get; set; }
        //public decimal  ProfitMoney   { get; set; }
        public decimal ProfitMoney { get { return WinMoney + ActivityMoney + RebateMoney - BetMoney - RewardMoney; } }
        public DateTime RegisterTime { get; set; }
    }
}
