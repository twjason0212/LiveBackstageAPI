using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportViewModel
{
    public class NewPaysReport
    {
        public string UserName { get; set; }
        public string UserGrade { get; set; }
        public string UserAgentName { get; set; }
        public decimal UserCapitalMoney { get; set; }
        public decimal UserDepositMoney { get; set; }
        public string UserDepositType { get; set; }
        public string UserDepositSource { get; set; }
        public DateTime UserDepositTime { get; set; }
        public DateTime UserRegisterTime { get; set; }
    }
}
