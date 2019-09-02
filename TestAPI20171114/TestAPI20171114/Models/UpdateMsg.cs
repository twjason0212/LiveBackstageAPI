using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;


namespace TestAPI20171114.Models
{
    public static class UpdateMsg
    {
        public static bool PostUpdate(string msg)
        {
            Conf.MseeageQueue.TryAdd(msg);
            return true;

        }

        public static bool SendMessage(string msg)
        {
            bool flog = false;
            using (var client = new WebClient() { Encoding = Encoding.UTF8 })
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                var data = "msg=" + msg;
                var result = client.UploadString(Conf.SLCMUrl, "POST", data);
                if (result == "true")
                    flog = true;
                else
                    flog = false;
            }
            using (var client = new WebClient() { Encoding = Encoding.UTF8 })
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                var data = "msg=" + msg;
                var result = client.UploadString(Conf.SLCM2Url, "POST", data);
                if (result == "true")
                    flog = true;
                else
                    flog = false;
            }

            return flog;
        }
    }
}