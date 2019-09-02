using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestAPI20171114.Models.Request
{
    public class ManageLog
    {
        public string IP { get; set; }

        public int ID { get; set; }

        public string Mark { get; set; }

        public string Time { get; set; }

        public string Type { get; set; }

        public string UserName { get; set; }
    }
}