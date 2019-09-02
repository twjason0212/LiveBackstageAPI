using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportViewModel
{
    public class dt_anchor_tip_record
    {
        public string identityid { get; set; }//商戶ID
        public int anchor_id { get; set; }//荷官名稱
        public decimal tipmoney_today { get; set; }//本日打賞金額
        public decimal tipmoney_thismonth { get; set; }//本月打賞金額
        public decimal tipmoney_lastmonth { get; set; }//上月打賞金額
        public decimal tipmoney_total { get; set; }//累積打賞金額        
    }
    public class return_anchor_tip_record
    {
        public List<dt_anchor_tip_record> anchor_list { get; set; }
        public int anchor_count { get; set; }
        public int code { get; set; }
    }
}
