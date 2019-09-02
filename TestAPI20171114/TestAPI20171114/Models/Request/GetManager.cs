using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestAPI20171114.Models.Request
{
    public class GetManager
    {
        public int ID { get; set; }

        public string UserName { get; set; }

        public string RealName { get; set; }

        public int AdminRoleID { get; set; }
    }
}