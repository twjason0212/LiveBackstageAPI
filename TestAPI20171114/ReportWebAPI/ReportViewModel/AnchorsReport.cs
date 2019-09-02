using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportViewModel
{
    public class AnchorsReport
    {
        //public string  IdentityId        { get; set; }//商戶ID
        public int AnchorId { get; set; }//荷官名稱
        public decimal TipMoneyToday { get; set; }//本日打賞金額
        public decimal TipMoneyThisMonth { get; set; }//本月打賞金額
        public decimal TipMoneyLastMonth { get; set; }//上月打賞金額
        public decimal TipMoneyTotal { get; set; }//累積打賞金額  
    }
    public class AnchorListReport
    {
        public List<AnchorsReport> AnchorList { get; set; }
        public int AnchorCount { get; set; }
        //public int                 Code        { get; set; }
    }
}
