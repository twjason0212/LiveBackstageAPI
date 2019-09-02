using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestAPI20171114.Models.Request
{
    public class ManagerList
    {
        public int ID { get; set; }

        public string UserName { get; set; }

        public string RealName { get; set; }

        public string AdminRole { get; set; }

        public string AddTime  { get; set; }

        public int Status { get; set; }
    }
}