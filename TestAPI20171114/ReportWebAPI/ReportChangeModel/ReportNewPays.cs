using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportChangeModel
{
    public class ReportNewPays
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Level { get; set; }
        public string UpAgent { get; set; }
        public decimal Balance { get; set; }
        public decimal RechargeMongy { get; set; }
        public string RechargeWay { get; set; }
        public string RechargeClient { get; set; }
        public DateTime Time { get; set; }
        public DateTime SignupTime { get; set; }
    }
}
