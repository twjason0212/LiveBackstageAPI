using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportModel
{
    public class BeforeParameters
    {
        public string IdentityId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public byte IsAgent { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int LoginId { get; set; }
        public int Flog { get; set; }
    }
}
