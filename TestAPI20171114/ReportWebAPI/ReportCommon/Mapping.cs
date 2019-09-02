using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportCommon
{
    public static class Mapping
    {
        public static DataTable ToDataTable<T>(this List<T> models, bool NullCheck = true) where T : class, new()
        {
            Dictionary<string, PropertyInfo> dicPropertyInfo = new Dictionary<string, PropertyInfo>();
            PropertyInfo[] propertyInfos = new T().GetType().GetProperties();

            bool fuckit = (NullCheck && models != null && models.Count > 0);

            if (fuckit)
            {
                DataTable dt = new DataTable();

                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    DataColumn dataColumn = new DataColumn(propertyInfo.Name, propertyInfo.PropertyType);
                    dt.Columns.Add(dataColumn);
                }

                foreach (var model in models)
                {
                    var row = dt.NewRow();
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        row[propertyInfo.Name] = propertyInfo.GetValue(model);
                    }
                    dt.Rows.Add(row);
                }

              
                return dt;
            }
            return new DataTable();
        }

        public static string GetMoneyAndNumber(decimal money,int number)
        {
            return money.ToString("0.0") + "/" + number + "人";
        }
        public static string GetNumber( int number)
        {
            return number + "人";
        }
        public static string FormatSecond(double fSecond)
        {
            int num = (int)(fSecond / 3600.0 / 24.0);
            double num2 = fSecond % 86400.0;
            int num3 = (int)(num2 / 3600.0);
            num2 %= 3600.0;
            int num4 = (int)(num2 / 60.0);
            num2 %= 60.0;
            int num5 = (int)num2;
            string arg = string.Empty;
            bool flag = num > 0;
            if (flag)
            {
                arg = num + "天";
            }
            bool flag2 = num3 > 0;
            if (flag2)
            {
                arg = arg + num3 + "小时";
            }
            bool flag3 = num4 > 0;
            if (flag3)
            {
                arg = arg + num4 + "分";
            }
            return arg + num5 + "秒";
        }
    }
}
