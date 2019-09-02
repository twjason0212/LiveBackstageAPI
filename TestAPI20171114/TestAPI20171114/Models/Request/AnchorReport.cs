using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestAPI20171114.Models.Request
{
    public class AnchorReport
    {
        public int anchorId { get; set; }

        public string anchorCode { get; set; }

        public string anchorName { get; set; }

        public decimal currMonthAmount { get; set; }

        public decimal lastMonthAmount { get; set; }

        public decimal allAmount { get; set; }
    }
}