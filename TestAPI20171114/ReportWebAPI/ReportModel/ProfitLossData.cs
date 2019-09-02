using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportModel
{
    /// <summary>
    ///代理报表
    /// </summary>
    public class ProfitLossData
    {
        private decimal _balance;
        public string Balance//:1000,//余额
        {
            get { return _balance.ToString("#0.00"); }
            set { _balance = Convert.ToDecimal(value); }
        }
        private decimal _betting;
        public string Betting//:1000,//投注金额
        {
            get { return _betting.ToString("#0.00"); }
            set { _betting = Convert.ToDecimal(value); }
        }
        private decimal _bonusMoney;
        public string BonusMoney//:300,//中奖金额
        {
            get { return _bonusMoney.ToString("#0.00"); }
            set { _bonusMoney = Convert.ToDecimal(value); }
        }
        private decimal _activity;
        public string Activity//:100,//活动金额
        {
            get { return _activity.ToString("#0.00"); }
            set { _activity = Convert.ToDecimal(value); }
        }
        private decimal _rebate;
        public string Rebate//:52,//返点金额
        {
            get { return _rebate.ToString("#0.00"); }
            set { _rebate = Convert.ToDecimal(value); }
        }
        private decimal _recharge;
        public string Recharge//2000,//充值金额
        {
            get { return _recharge.ToString("#0.00"); }
            set { _recharge = Convert.ToDecimal(value); }
        }
        private decimal _withdraw;
        public string Withdraw//500,//提现金额
        {
            get { return _withdraw.ToString("#0.00"); }
            set { _withdraw = Convert.ToDecimal(value); }
        }
        public string AllProfitLoss//:200,//盈亏总额
        {
            get { return (_bonusMoney - _betting + _activity + _rebate).ToString("#0.00"); }
        }
    }
}
