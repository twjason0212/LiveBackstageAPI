using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestAPI20171114.Models.Request;

namespace TestAPI20171114.Common
{
    public static class ConvertExtensionMethods
    {
        public static string ToOnOff(this bool value)
        {
            if (value)
                return "on";
            else
                return "off";
        }

        public static bool ToBoolByOnOffString(this string value)
        {
            if (value != null && value.ToLower() == "on")
                return true;
            else
                return false;
        }

        public static List<T> ConvertAnchor<T>(this List<AnchorReport> anchorReports) where T : class, new()
        {

            PropertyInfo[] propertyInfos = new T().GetType().GetProperties();

            List<T> listAnchor = new List<T>();

            foreach (var anchor in anchorReports)
            {
                var model = new T();
                foreach (var property in propertyInfos)
                {
                    string PropertyName = property.Name;
                    switch (PropertyName)
                    {
                        case "anchor_id":
                            property.SetValue(model, Convert.ChangeType(anchor.anchorId, property.PropertyType));
                            //anchorReport.anchor_id = anchor.anchorId;
                            break;
                        case "tipmoney_thismonth":
                            property.SetValue(model, Convert.ChangeType(anchor.currMonthAmount, property.PropertyType));
                            break;
                        case "tipmoney_lastmonth":
                            property.SetValue(model, Convert.ChangeType(anchor.lastMonthAmount, property.PropertyType));
                            break;
                        case "tipmoney_total":
                            property.SetValue(model, Convert.ChangeType(anchor.allAmount, property.PropertyType));
                            break;
                    }
                }
                listAnchor.Add(model);
            }


            return listAnchor;
        }

    }
}