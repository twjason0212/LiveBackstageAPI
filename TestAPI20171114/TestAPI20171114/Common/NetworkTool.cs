using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace TestAPI20171114.Common
{
    public class NetworkTool
    {

        private static readonly string IPv4 = "^((2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.){3}(2[0-4]\\d|25[0-5]|[01]?\\d\\d?)$";

        public static string GetClientIP(HttpContext context)
        {
            string ip = string.Empty;

            string forward = context.Request.ServerVariables["HTTP_X_FORWARD_FOR"];
            string forwarded = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            string incap = context.Request.ServerVariables["HTTP_INCAP_CLIENT_IP"];
            string real = context.Request.ServerVariables["HTTP_X_REAL_IP"];
            string addr = context.Request.ServerVariables["REMOTE_ADDR"];

            if (!string.IsNullOrEmpty(forward) && forward.Split(',').Length > 1 && IPv4Check(forward.Split(',')[0]))
                ip = forward.Split(',')[0];
            else if (!string.IsNullOrEmpty(forwarded) && forwarded.Split(',').Length > 1 && IPv4Check(forwarded.Split(',')[0]))
                ip = forwarded.Split(',')[0];
            else if (!string.IsNullOrEmpty(incap) && IPv4Check(incap))
                ip = incap;
            else if (!string.IsNullOrEmpty(real) && IPv4Check(real))
                ip = real;
            else if (!string.IsNullOrEmpty(addr) && IPv4Check(addr))
                ip = addr;
            else
                ip = "0.0.0.0";

            return ip;
        }

        public static bool IPv4Check(string str)
        {
            return Regex.IsMatch(str, IPv4);
        }
    }
}