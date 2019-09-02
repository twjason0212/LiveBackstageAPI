using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportModel
{
    public class ResultInfoT<T>
    {
        public int Code { get; set; }
        public string StrCode { get; set; }
        public int DataCount { get; set; }
        public string BackUrl { get; set; }
        public T BackData { get; set; }
    }
}
