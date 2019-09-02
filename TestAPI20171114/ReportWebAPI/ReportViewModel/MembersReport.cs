using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportViewModel
{
    public class MembersReport
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserGrade { get; set; }
        public string UserAgentName { get; set; }
        public decimal UserCapital { get; set; }
        public decimal UserDepositMoney { get; set; }
        public decimal UserWithdrawMoney { get; set; }
        public decimal UserBetMoney { get; set; }
        public decimal UserWinMoney { get; set; }
        public decimal UserRewardMoney { get; set; }
        public decimal UserRebateMoney { get; set; }
        public decimal UserActivityMoney { get; set; }
        //盈利＝中奖-投注+返点+活动+其他优惠
        public decimal UserProfitMoney
        {
            get;
            set;
        }
        public decimal UserProfitRate
        {
            get;
            set;
        }
    }
}
