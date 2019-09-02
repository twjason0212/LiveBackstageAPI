using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;

namespace TestAPI20171114.Models
{
    public class Conf
    {
        public static string WSUrl
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["WSUrl"] ?? "";
            }
        }

        public static string SLCMUrl
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["SLCMUrl"] != "" ? System.Configuration.ConfigurationManager.AppSettings["SLCMUrl"] + "UpdateStatic" : "";
            }
        }

        public static string SLCM2Url
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["SLCM2Url"] != "" ? System.Configuration.ConfigurationManager.AppSettings["SLCM2Url"] + "UpdateStatic" : "";
            }
        }

        public static string SLCMFUrl
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["SLCMUrl"] != "" ? System.Configuration.ConfigurationManager.AppSettings["SLCMUrl"] + "SendMessage": "";
            }
        }

        public static int MaxSystemBarrageCount
        {
            get
            {
                int maxCount = int.TryParse(System.Configuration.ConfigurationManager.AppSettings["MaxSystemBarrageCount"] ?? "",out maxCount)
                    ? maxCount
                    : 60;

                return maxCount;
            }
        }

        public static string ReportUri = "";

        public static BlockingCollection<string> MseeageQueue = new BlockingCollection<string>();

    }
}