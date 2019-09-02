using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportViewModel
{
    /// <summary>
    /// 綜合報表
    /// </summary>
    public class IntegratedReport
    {
        public string IdentityId { get; set; }
        public decimal Profit { get { return BetMoney - WinMoney - ActivityMoney - RebateMoney + AdminMoney + RejectMoney; } }//盈利
        public string ProfitRate { get; set; }//盈利率
        public decimal BetMoney { get; set; }
        public int BetNumber { get; set; }
        public int BetCount { get; set; }
        public decimal WinMoney { get; set; }
        public int WinNumber { get; set; }
        public decimal InMoney { get { return FastpayMoney + BankpayMoney + AlipayMoney + WeChatpayMoney + QQpayMoney + UnionpayMoney + FourthpayMoney + ManualInMoney; } }//充值金額
        public int InNumber { get; set; }
        public int InCount { get; set; }
        public decimal FastpayMoney { get; set; }
        public int FastpayNumber { get; set; }
        public decimal BankpayMoney { get; set; }
        public int BankpayNumber { get; set; }
        public decimal AlipayMoney { get; set; }
        public int AlipayNumber { get; set; }
        public decimal WeChatpayMoney { get; set; }
        public int WechatpayNumber { get; set; }
        public decimal QQpayMoney { get; set; }
        public int QQpayNumber { get; set; }
        public decimal UnionpayMoney { get; set; }
        public int UnionpayNumber { get; set; }
        public decimal FourthpayMoney { get; set; }
        public int FourthpayNumber { get; set; }
        public decimal ManualInMoney { get; set; }
        public int ManualInNumber { get; set; }
        public decimal ActivityMoney { get { return SystemActivityMoney + OtherActivityMoney; } }
        public int ActivityNumber { get { return SystemActivityNumber + OtherActivityNumber; } }
        public decimal SystemActivityMoney { get; set; }
        public int SystemActivityNumber { get; set; }
        public decimal OtherActivityMoney { get; set; }
        public int OtherActivityNumber { get; set; }
        public decimal OutMoney { get; set; }
        public int OutNumber { get; set; }
        public int OutCount { get; set; }
        public decimal RebateMoney { get; set; }
        public int RebateNumber { get; set; }
        public int InOperateTime { get; set; }
        public int OutOperateTime { get; set; }
        public decimal ManualOutMoney { get { return MistakeMoney + AdminMoney; } }//人工提出
        public int ManualOutNumber { get { return MistakeNumber + AdminNumber; } }
        public decimal MistakeMoney { get; set; }
        public int MistakeNumber { get; set; }
        public decimal AdminMoney { get; set; }
        public int AdminNumber { get; set; }
        public decimal RejectMoney { get; set; }
        public int RejectNumber { get; set; }
        public decimal CancelMoney { get; set; }
        public int CancelNumber { get; set; }
        public decimal RewardMoney { get; set; }
        public int RewardNumber { get; set; }
        public int RewardCount { get; set; }

        /*↑出入帳金額&人數&筆數↑*/
        //↓月報表沒有↓
        public int RegisterNumber { get; set; }
        public int NewPayNumber { get; set; }
        public int OnlineNumber { get; set; }
        public decimal UsersCapital { get; set; }

        /*↑行政數值↑*/
        public decimal ThisMonthProfit { get; set; }
        public decimal ThisMonthBetMoney { get; set; }
        public decimal ThisMonthWinMoney { get; set; }
        public string ThisMonthProfitRate { get; set; }
        public decimal ThisMonthGross { get; set; }
        public string ThisMonthGrossRate { get; set; }

        public decimal LastMonthProfit { get; set; }
        public decimal LastMonthBetMoney { get; set; }
        public decimal LastMonthWinMoney { get; set; }
        public string LastMonthProfitRate { get; set; }
        public decimal LastMonthGross { get; set; }
        public string LastMonthGrossRate { get; set; }
        /*↑損益相關↑*/
    }
}
