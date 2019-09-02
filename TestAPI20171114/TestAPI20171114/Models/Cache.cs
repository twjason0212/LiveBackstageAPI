using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestAPI20171114.Common;

namespace TestAPI20171114.Models
{
    public class Cache
    {
        public static List<dt_ManagerRole> Role = new List<dt_ManagerRole>();        
        public static void refreshRole()
        {
            try
            {
                using (var db = new livecloudEntities())
                {
                    Role = db.dt_ManagerRole.OrderByDescending(r => r.Id).ToList();
                }
            }
            catch(Exception ex)
            {
                Log.Error("Cache", "Cache", ex.Message.ToString());
                if (ex.InnerException != null)
                {
                    Log.Error("Cache", "Cache InnerException", ex.InnerException.Message.ToString());
                }
            }
           
        }

    }
}