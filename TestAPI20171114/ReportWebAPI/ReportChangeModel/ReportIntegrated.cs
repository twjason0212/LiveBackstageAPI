using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportChangeModel
{
    /// <summary>
    /// 报表总览实体类
    /// </summary>
    public class ReportIntegrated
    {
        /// <summary>
        /// 存款总额
        /// </summary>
        public string TotalInMoney { get; set; }
        //  public decimal TotalInMoney_Peo { get; set; }
        /// <summary>
        /// 投注总额
        /// </summary>
        public string Out_BettingAccount { get; set; }
        //  public decimal Out_BettingAccount_Peo { get; set; }
        /// <summary>
        /// 当日盈利
        /// </summary>
        public string NowProfit { get; set; }
        /// <summary>
        /// 快捷支付
        /// </summary>
        public string In_FastPrepaid { get; set; }
        //  public decimal In_FastPrepaid_Peo { get; set; }
        /// <summary>
        /// 中奖总额
        /// </summary>
        public string Out_WinningAccount { get; set; }
        //  public decimal Out_WinningAccount_Peo { get; set; }
        /// <summary>
        /// 当日盈率
        /// </summary>
        public string NowProfitRate { get; set; }
        /// <summary>
        /// 网银汇款
        /// </summary>
        public string In_BankPrepaid { get; set; }
        /// <summary>
        /// 支付宝
        /// </summary>
        public string In_AlipayPrepaid { get; set; }
        /// <summary>
        /// 微信支付
        /// </summary>
        public string In_WeChatPrepaid { get; set; }
        /// <summary>
        /// QQ支付
        /// </summary>
        public string In_QQ { get; set; }
        /// <summary>
        /// QQ支付new
        /// </summary>
        public string In_QQPrepaid { get; set; }
        /// <summary>
        /// 第四方支付
        /// </summary>
        public string In_FourthPrepaid { get; set; }
        /// <summary>
        /// 銀聯支付
        /// </summary>
        public string In_UnionpayPrepaid { get; set; }
        /// <summary>
        /// 投注单量
        /// </summary>
        public string Out_BettingAccount_Num { get; set; }
        //  public decimal Out_BettingAccount_Num_Peo { get; set; }

        /// <summary>
        /// 人工存款
        /// </summary>
        public string In_ArtificialDeposit { get; set; }
        /// <summary>
        /// 撤单总额
        /// </summary>
        public string Out_CancelAccount { get; set; }

        /// <summary>
        /// 取款总额
        /// </summary>
        public string Out_Account { get; set; }
        //   public decimal Out_Account_Peo { get; set; }
        /// <summary>
        /// 会员余额
        /// </summary>
        public string TotlaBalance { get; set; }
        /// <summary>
        /// 入款笔数
        /// </summary>
        public string InMoneyNum { get; set; }
        /// <summary>
        /// 人工提出＝误存提出+行政提出
        /// </summary>
        public string TotalArtificialOut { get; set; }
        /// <summary>
        /// 转账总额
        /// </summary>
        public string TotalTransferAccount { get; set; }
        /// <summary>
        /// 出款笔数
        /// </summary>
        public string Out_Account_Num { get; set; }
        /// <summary>
        /// 误存提出
        /// </summary>
        public string Out_MistakeAccount { get; set; }
        //  public string Out_MistakeAccount_Peo { get; set; }
        /// <summary>
        /// 拒绝总额
        /// </summary>
        public string Out_RefuseAccount { get; set; }
        //   public string Out_RefuseAccount_Peo { get; set; }
        /// <summary>
        /// 入款时间
        /// </summary>
        public string InMoneyTime { get; set; }
        /// <summary>
        /// 行政提出
        /// </summary>
        public string Out_AdministrationAccount { get; set; }
        //   public string Out_AdministrationAccount_Peo { get; set; }
        /// <summary>
        /// 代理总额
        /// </summary>
        public string TotalAgent { get; set; }
        /// <summary>
        /// 出款时间
        /// </summary>
        public string OutMoneyTime { get; set; }
        /// <summary>
        /// 返点总额
        /// </summary>
        public string Out_RebateAccount { get; set; }
        //   public string Out_RebateAccount_Poe { get; set; }
        /// <summary>
        /// 代理工资
        /// </summary>
        public string Wages { get; set; }
        //  public string Wages_Peo { get; set; }
        /// <summary>
        /// 新增会员
        /// </summary>
        public string NewMemberNum { get; set; }
        /// <summary>
        /// 优惠总额
        /// </summary>
        public string TotalDiscount { get; set; }
        // public string TotalDiscount_Peo { get; set; }
        /// <summary>
        /// 代理分红
        /// </summary>
        public string Bonus { get; set; }
        //   public string Bonus_Peo { get; set; }
        /// <summary>
        /// 首充人数
        /// </summary>
        public string NewPrepaidNum { get; set; }
        /// <summary>
        /// 活动礼金
        /// </summary>
        public string Out_ActivityDiscountAccount { get; set; }
        //   public string Out_ActivityDiscountAccount_Peo { get; set; }
        /// <summary>
        /// 在线人数
        /// </summary>
        public string OnLinessNum { get; set; }
        /// <summary>
        /// 其他优惠
        /// </summary>
        public string Out_OtherDiscountAccount { get; set; }
        //  public string Out_OtherDiscountAccount_Poe { get; set; }
        /// <summary>
        /// 快捷充值次数
        /// </summary>
        public string In_FastPrepaid_Num { get; set; }
        /// <summary>
        /// 银行转账次数
        /// </summary>
        public string In_BankPrepaid_Num { get; set; }
        /// <summary>
        /// 支付宝转账次数
        /// </summary>
        public string In_AlipayPrepaid_Num { get; set; }
        /// <summary>
        /// 微信支付次数
        /// </summary>
        public string In_WeChatPrepaid_Num { get; set; }
        /// <summary>
        /// QQ支付次数
        /// </summary>
        public string In_QQ_Num { get; set; }
        /// <summary>
        /// 人工转账次数
        /// </summary>
        public string In_ArtificialDeposit_Num { get; set; }
        /// <summary>
        /// 月中奖额
        /// </summary>
        public string MonthOut_WinningAccount { get; set; }
        /// <summary>
        /// 月投注额
        /// </summary>
        public string MonthOut_BettingAccount { get; set; }
        /// <summary>
        /// 本月盈利
        /// </summary>
        public string MonthProfitLoss { get; set; }
        /// <summary>
        /// 本月盈率
        /// </summary>
        public string MonthProfitRate { get; set; }
        /// <summary>
        /// 本月返点
        /// </summary>
        public string MonthOut_RebateAccount { get; set; }
        /// <summary>
        /// 本月活动优惠
        /// </summary>
        public string MonthOut_ActivityDiscountAccount { get; set; }
        /// <summary>
        /// 本月其他优惠
        /// </summary>
        public string MonthOut_OtherDiscountAccount { get; set; }
        /// <summary>
        /// 本月代理分红
        /// </summary>
        public string MonthBonus { get; set; }
        /// <summary>
        /// 本月代理工资
        /// </summary>
        public string MonthWages { get; set; }
        /// <summary>
        /// 本月拒绝出款
        /// </summary>
        public string MonthOut_RefuseAccount { get; set; }
        /// <summary>
        /// 本月行政提出
        /// </summary>
        public string MonthOut_AdministrationAccount { get; set; }
        /// <summary>
        /// 本月盈利
        /// </summary>
        public string MonthOut_Profit { get; set; }
        /// <summary>
        /// 本月损益
        /// </summary>
        public string MonthProfitAndLoss { get; set; }
        /// <summary>
        /// 本月毛率
        /// </summary>
        public string MonthGrossRate { get; set; }
        /// <summary>
        /// 当日损益
        /// </summary>
        public string CurrentProfitAndLoss { get; set; }
        /// <summary>
        /// 上月盈利
        /// </summary>
        public string LastMonthProfitLoss { get; set; }
        /// <summary>
        /// 上月投注
        /// </summary>
        public string LastMonthOut_BettingAccount { get; set; }
        /// <summary>
        /// 上月中奖额
        /// </summary>
        public string LastMonthOut_WinningAccount { get; set; }
        /// <summary>
        /// 上月盈率
        /// </summary>
        public string LastMonthProfitRate { get; set; }
        /// <summary>
        /// 上月损益
        /// </summary>
        public string LastMonthProfitAndLoss { get; set; }
        /// <summary>
        /// 上月毛率
        /// </summary>
        public string LastMonthGrossRate { get; set; }
        /// <summary>
        /// 抽成
        /// </summary>
        public string TakePercentage { get; set; }
        /// <summary>
        /// 上月返点金额
        /// </summary>
        public string LastMonthOut_RebateAccount { get; set; }
        /// <summary>
        /// 上月活动金额
        /// </summary>
        public string LastMonthOut_ActivityDiscountAccount { get; set; }
        /// <summary>
        /// 上月其他活动金额
        /// </summary>
        public string LastMonthOut_OtherDiscountAccount { get; set; }
        /// <summary>
        /// 上月分红
        /// </summary>
        public string LastMonthBonus { get; set; }
        /// <summary>
        /// 上月工资
        /// </summary>
        public string LastMonthWages { get; set; }
        /// <summary>
        /// 上月拒绝金额
        /// </summary>
        public string LastMonthOut_RefuseAccount { get; set; }
        /// <summary>
        /// 上月行政提出
        /// </summary>
        public string LastMonthOut_AdministrationAccount { get; set; }
        /// <summary>
        /// 打賞總額
        /// </summary>
        public string RewardMoney { get; set; }
        /// <summary>
        /// 打賞次數
        /// </summary>
        public string RewardCount { get; set; }
        /// <summary>
        /// 打賞人數
        /// </summary>
        public string RewardNum { get; set; }
        ///// <summary>
        ///// 构造函数
        ///// </summary>
        //public ReportSummary()
        //{
        //    string strDisplayValue = "0.00/0人";
        //    //上月盈利
        //    this.LastMonthProfitLoss = strDisplayValue;
        //    //月投注额
        //    this.MonthOut_BettingAccount = strDisplayValue;
        //    //本月盈利
        //    this.MonthProfitLoss = strDisplayValue;
        //    //月中奖额
        //    this.MonthOut_WinningAccount = strDisplayValue;
        //    //代理总额
        //    this.TotalAgent = strDisplayValue;
        //    //优惠总额
        //    this.TotalDiscount = strDisplayValue;
        //}
    }
}
