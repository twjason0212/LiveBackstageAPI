using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportChangeModel
{
    public class ReportPlatform
    {
        private decimal _In_Account;
        private decimal _Out_BettingAccount;
        private decimal _In_WinningAccount;
        private decimal _Out_RebateAccount;
        private decimal _Out_OtherDiscountAccount;
        private decimal _Out_ActivityDiscountAccount;
        private decimal _Out_Account;
        private decimal _Out_AdministrationAccount;
        private decimal _Out_RefuseAccount;
        private decimal _RewardMoney;

        public string identityid { get; set; }//站
        public string identityName { get; set; }//站
        public string AddDate { get; set; }//日期
        public decimal Out_BettingAccount
        {
            get { return _Out_BettingAccount; }
            set { _Out_BettingAccount = Math.Floor(value); }
        }//投注金额
        public int Out_BettingAccountNum { get; set; } //投注人数
        public int Out_BettingAccountCount { get; set; } //投注笔数
        public decimal In_WinningAccount
        {
            get { return _In_WinningAccount; }
            set { _In_WinningAccount = Math.Floor(value); }
        }//中奖金额
         // public int In_WinningAccountNum { get; set; } //中奖人数
        public decimal Out_RebateAccount
        {
            get { return _Out_RebateAccount; }
            set { _Out_RebateAccount = Math.Floor(value); }
        }//返点金额
         // public int Out_RebateAccountNum { get; set; } //返点人数
        public decimal In_Account
        {
            get { return _In_Account; }
            set { _In_Account = Math.Floor(value); }
        }//充值金额
        public int In_AccountNum { get; set; }//充值人数
        public int In_AccountCount { get; set; }//充值笔数
                                                // public decimal In_ArtificialDeposit { get; set; }//人工存款
                                                // public int In_ArtificialDepositNum { get; set; } //人工存款人数
        public decimal Out_ActivityDiscountAccount
        {
            get { return _Out_ActivityDiscountAccount; }
            set { _Out_ActivityDiscountAccount = Math.Floor(value); }
        }//系统活动
        public decimal Out_OtherDiscountAccount
        {
            get { return _Out_OtherDiscountAccount; }
            set { _Out_OtherDiscountAccount = Math.Floor(value); }
        }//其他活动
        public decimal Out_Account
        {
            get { return _Out_Account; }
            set { _Out_Account = Math.Floor(value); }
        }//提现金额
         // public int Out_AccountNum { get; set; } //提现人数
        public int Out_AccountCount { get; set; } //提现笔数
        public decimal Out_AdministrationAccount
        {
            get { return _Out_AdministrationAccount; }
            set { _Out_AdministrationAccount = Math.Floor(value); }
        }//行政提出
        public decimal Out_RefuseAccount
        {
            get { return _Out_RefuseAccount; }
            set { _Out_RefuseAccount = Math.Floor(value); }
        }//拒絕金額
        public int NewMemberNum { get; set; }//註冊人數
        public int NewPrepaidNum { get; set; }//首充人數
        public string ProfitLoss
        {
            get;
            set;
        }//盈利
        public string ProfitRate { get; set; }//盈率
        public int RewardNum { get; set; } //打賞人數
        public decimal RewardMoney    //打賞金額
        {
            get { return _RewardMoney; }
            set { _RewardMoney = Math.Floor(value); }
        }

    }
}
