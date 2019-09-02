using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportModel
{
    public class AfterParameters
    {
        public string   IdentityId  { get; set; }
        public string   UserId      { get; set; }
        public string   UserName    { get; set; }
        public string   AgentId     { get; set; }
        public string   AgentName   { get; set; }
        public byte     IsAgent     { get; set; }
        public DateTime Date        { get; set; }
        public DateTime StartTime   { get; set; }
        public DateTime EndTime     { get; set; }
        public string   LotteryCode { get; set; }
        public int      PageNum     { get; set; }
        public int      PageSize    { get; set; }
        public string   OrderBy     { get; set; }
    }
}
